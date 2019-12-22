using SIS.Data;
using SIS.HTTP.Enums;
using SIS.HTTP.Requests;
using SIS.HTTP.Responses;
using SIS.WebServer.Results;
using System;
using System.IO;
using System.Linq;

namespace SIS.Demo
{
    internal class LoginControler
    {
        public LoginControler()
        {
            
        }
        public IHttpResponse Login(IHttpRequest httpRequest)
        {
            string content = "<h1>Hello from login page</h1>" +
                File.ReadAllText(@"D:\C# Web Basics\SIS\SIS.Demo\loginform.html");

            return new HtmlResult(content, HttpResponseStatusCode.Ok);
        }

        public IHttpResponse Register(IHttpRequest httpRequest)
        {
            string content = "<h1>Hello from login page</h1>" +
                File.ReadAllText(@"D:\C# Web Basics\SIS\SIS.Demo\registerform.html");

            return new HtmlResult(content, HttpResponseStatusCode.Ok);
        }

        internal IHttpResponse RegisterHandle(IHttpRequest request)
        {
            string username = (string)request.FormData["username"];
            string password = (string)request.FormData["password"];
            string confirmPassword = (string)request.FormData["confirmPassword"];

            using (TestDbContext testDbContext = new TestDbContext())
            {
                if (password != confirmPassword)
                {
                    return new RedirectResult(@"\register");
                }
                testDbContext.Users.Add(new Models.User()
                {
                    Name = username,
                    Password = password
                });
                testDbContext.SaveChanges();
            }
            return new RedirectResult(@"\home");
        }

        internal IHttpResponse LoginHandle(IHttpRequest request)
        {
            string username = (string)request.FormData["username"];
            string password = (string)request.FormData["password"];

            using (TestDbContext testDbContext = new TestDbContext())
            {
                if (testDbContext.Users.SingleOrDefault(u => u.Name == username && u.Password == password) == null)
                {
                    return new RedirectResult(@"\login");
                }
            }

            return new RedirectResult(@"\home");
        }
    }
}