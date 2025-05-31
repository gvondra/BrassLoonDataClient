using BrassLoon.DataClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataClientTest
{
    [TestClass]
    public class ColumnMappingTest
    {
        [TestMethod]
        [DataRow("Name2")]
        [DataRow("")]
        [DataRow(null)]
        public void SetValueTest(string name)
        {
            ModelTest model = new ModelTest { Name = "Name1" };
            ColumnMapping mapping = new ColumnMapping() { Info = model.GetType().GetProperty("Name") };
            mapping.SetValue(model, name);
            Assert.AreEqual(name, model.Name);
        }

        private sealed class ModelTest
        {
            public string Name { get; set; }
        }
    }
}