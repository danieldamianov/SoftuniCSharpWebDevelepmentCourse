using NUnit.Framework;
using SIS.MvcFramework.ViewEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SIS.MvcFramework.Tests
{
    [TestFixture]
    public class TestViewEngine
    {
        [Test]
        [TestCase("TestSimpleView")]
        [TestCase("TestBasicOperators")]
        [TestCase("TestViewModel")]
        public void TestView(string viewName)
        {
            SISViewEngine sISViewEngine = new SISViewEngine();

            string viewPath = $"ViewTests/{viewName}.html";
            string viewExpectedPath = $"ViewTests/{viewName}.Result.html";

            string inputView = File.ReadAllText(viewPath);
            string expectedOutputView = File.ReadAllText(viewExpectedPath);

            string actualOutput = sISViewEngine.TransformView<object>(inputView,new ExampleViewModel());

            Assert.AreEqual(expectedOutputView.TrimEnd(), actualOutput.TrimEnd());
        }
    }
}
