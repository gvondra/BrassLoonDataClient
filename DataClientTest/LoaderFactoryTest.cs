using BrassLoon.DataClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataClientTest
{
    [TestClass]
    public class LoaderFactoryTest
    {
        [TestMethod]
        public void CreateTest()
        {
            LoaderFactory loaderFactory = new LoaderFactory();
            ILoader loader = loaderFactory.CreateLoader();
            Assert.IsNotNull(loader);
            Assert.IsInstanceOfType(loader, typeof(Loader));
            Assert.IsNotNull(((Loader)loader).Components);
            Assert.AreEqual(12, ((Loader)loader).Components.Count);
        }
    }
}
