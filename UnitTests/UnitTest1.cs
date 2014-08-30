using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebServiceTestStudio;
using WebServiceTestStudio.Core;
using WebServiceStudio;

namespace UnitTests
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestWsdlReset()
        {
            var wsdl = new Wsdl();
            wsdl.Reset();
        }

        [TestMethod]
        public void TestWsdlLoad()
        {
            var path = @"f:\Dev\rts\source\00\bin\release\TransportationWebService.wsdl";
            var wsdl = new Wsdl();
            //wsdl.Reset();
            wsdl.Paths.Add(path);
            wsdl.Generate();
            Assert.IsNotNull(wsdl.ProxyAssembly);
        }

        [TestMethod]
        public void TestVersionInfoMethodExists()
        {
            var path = @"f:\Dev\rts\source\00\bin\release\TransportationWebService.wsdl";
            var wsdl = new Wsdl();
            //wsdl.Reset();
            wsdl.Paths.Add(path);
            wsdl.Generate();
            Assert.IsNotNull(wsdl.ProxyAssembly);

        }
    }
}
