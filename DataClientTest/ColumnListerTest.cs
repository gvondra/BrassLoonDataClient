using BrassLoon.DataClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace DataClientTest
{
    [TestClass]
    public class ColumnListerTest
    {
        [TestMethod]
        public void GetColumnsTest()
        {
            TestModel testModel = new TestModel { Ignored = "hold" };

            string[] columns = DataUtil.GetColumns<TestModel>();
            Assert.IsNotNull(columns);
            Assert.AreEqual(2, columns.Length);
            Assert.IsTrue(columns.Any(c => c == "Id"));
            Assert.IsTrue(columns.Any(c => c == "Name"));
            Assert.AreSame(columns, DataUtil.GetColumns(testModel.GetType()));
        }

        private class TestModel
        {
            [ColumnMapping]
            public int Id { get; set; }
            [ColumnMapping("Name")]
            public string Nnaammee { get; set; }
            public string Ignored { get; set; }
        }
    }
}
