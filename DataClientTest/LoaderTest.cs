using BrassLoon.DataClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DataClientTest
{
    [TestClass]
    public class LoaderTest
    {
        private const int TEST_ID = 42;
        private const string TEST_NAME = "Homer Simpson ";
        private DateTime TEST_TIMESTAMP = new DateTime(2020, 02, 02, 14, 20, 0);

        [TestMethod]
        public void LoadTest()
        {
            TestModel model = new TestModel();
            LoaderFactory loaderFactory = new LoaderFactory();
            ILoader loader = loaderFactory.CreateLoader();
            Assert.IsInstanceOfType(loader, typeof(Loader));
            using (IDataReader reader = CreateReader())
            {
                Assert.IsTrue(reader.Read());
                loader.Load(model, reader);
                reader.Close();
            }
            Assert.AreEqual(TEST_ID, model.Id);
            Assert.AreEqual(TEST_NAME.TrimEnd(), model.Name);
            Assert.AreEqual(TEST_TIMESTAMP, model.Timestamp);
        }

        private IDataReader CreateReader()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Id", typeof(int));
            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Columns.Add("Timestamp", typeof(DateTime));

            dataTable.Rows.Add(TEST_ID, TEST_NAME, TEST_TIMESTAMP);

            dataTable.AcceptChanges();
            return dataTable.CreateDataReader();
        }

        private class TestModel
        {
            [ColumnMapping("Id")] public int Id { get; set; }
            [ColumnMapping("Name")] public string Name { get; set; }
            [ColumnMapping("Timestamp")] public DateTime Timestamp { get; set; }
        }
    }
}
