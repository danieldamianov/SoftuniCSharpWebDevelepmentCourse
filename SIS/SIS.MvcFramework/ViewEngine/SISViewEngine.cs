using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace SIS.MvcFramework.ViewEngine
{
    public class SISViewEngine : IViewEngine
    {
        public string TransformView<T>(string viewContent, T model)
        {
            string cSharpCodeForFillingTheStringBuilder = GetCSharpCodeForFillingTheStringBuilder(viewContent);

            string virtualMethod = model != null ? $@"
using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using SIS.MvcFramework.ViewEngine;

namespace CustomRazor
{{
    public class CustomViewEngine : IView
    {{
        public string GetHtml(object model)
        {{
            var Model = model as {model.GetType().FullName};
            
            var html = new StringBuilder();

            {cSharpCodeForFillingTheStringBuilder}

            return html.ToString();

        }}
    }}
}}
" :
$@"
using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using SIS.MvcFramework.ViewEngine;

namespace CustomRazor
{{
    public class CustomViewEngine : IView
    {{
        public string GetHtml(object model)
        {{
            
            var html = new StringBuilder();

            {cSharpCodeForFillingTheStringBuilder}

            return html.ToString();

        }}
    }}
}}
"
;
            IView view = CompileAndIntance(virtualMethod, model?.GetType().Assembly);

            return view.GetHtml(model);
        }

        private IView CompileAndIntance(string virtualMethod, Assembly assembly)
        {
            var compilation = CSharpCompilation.Create("ViewEngineAssembly")
                .WithOptions(new CSharpCompilationOptions(Microsoft.CodeAnalysis.OutputKind.DynamicallyLinkedLibrary))
                .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
                .AddReferences(MetadataReference.CreateFromFile(typeof(IView).Assembly.Location));

            if (assembly != null)
            {
                compilation = compilation.AddReferences(MetadataReference.CreateFromFile(assembly.Location));
            }

            var netStandardAssembly = Assembly.Load(new AssemblyName("netstandard")).GetReferencedAssemblies();
            foreach (var assemblyStandart in netStandardAssembly)
            {
                compilation = compilation.AddReferences(
                    MetadataReference.CreateFromFile(Assembly.Load(assemblyStandart).Location));
            }

            compilation = compilation.AddSyntaxTrees(SyntaxFactory.ParseSyntaxTree(virtualMethod));

            using (var memoryStr = new MemoryStream())
            {
                var compilated = compilation.Emit(memoryStr);

                if (compilated.Success == false)
                {
                    foreach (var error in compilated.Diagnostics)
                    {
                        Console.WriteLine(error.GetMessage());
                    }
                }

                memoryStr.Seek(0, SeekOrigin.Begin);

                var assemblyBytes = memoryStr.ToArray();

                var assemblyInstance = Assembly.Load(assemblyBytes);

                var viewType = assemblyInstance.GetType("CustomRazor.CustomViewEngine");

                var typeInstance = Activator.CreateInstance(viewType);

                return typeInstance as IView;
            }
        }

        private string GetCSharpCodeForFillingTheStringBuilder(string viewContent)
        {
            string CSharpCodeForAppendingTheHtml = string.Empty;
            var supportedOperators = new string[] { "for", "foreach", "if", "else" };

            foreach (var line in viewContent.Split(Environment.NewLine))
            {
                if (line.TrimStart().StartsWith("{") || line.TrimStart().StartsWith("}"))
                {
                    CSharpCodeForAppendingTheHtml += line;
                }
                else if (supportedOperators.Any(oper => line.TrimStart().StartsWith("@" + oper)))
                {
                    var lineWithoutAt = line.Remove(line.IndexOf("@"), 1);
                    CSharpCodeForAppendingTheHtml += lineWithoutAt;
                }
                else
                {
                    if (line.Contains("@") == false)
                    {
                        CSharpCodeForAppendingTheHtml += $"html.AppendLine(@\"{line.Replace("\"", "\"\"")}\");";
                    }
                    else
                    {
                        var appendLineCode = $"html.AppendLine(@\""; // html.AppendLine(@"

                        var lineCopy = line;

                        while (lineCopy.Contains("@"))
                        {
                            int idexOfAt = lineCopy.IndexOf("@");

                            appendLineCode += lineCopy.Substring(0, idexOfAt).Replace("\"", "\"\"");

                            var CSharpCodeRegex = new Regex(@"[^\s""<&]+", RegexOptions.Compiled);

                            var CSharpExpr = CSharpCodeRegex.Match(lineCopy.Substring(idexOfAt + 1))?.Value;

                            //appendLineCode += "\"" + $" + {CSharpExpr} + " + @\"";
                            appendLineCode += "\" + " + CSharpExpr + " + @\"";

                            if (lineCopy.Length <= idexOfAt + CSharpExpr.Length + 1)
                            {
                                lineCopy = string.Empty;
                            }
                            else
                            {
                                lineCopy = lineCopy.Substring(idexOfAt + 1 + CSharpExpr.Length);
                            }
                        }

                        appendLineCode += $"{lineCopy.Replace("\"", "\"\"")}\");";
                        CSharpCodeForAppendingTheHtml += appendLineCode;
                    }
                }
            }

            return CSharpCodeForAppendingTheHtml;
        }
    }
}
