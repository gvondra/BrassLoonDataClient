﻿using BrassLoon.DataClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;

namespace DataClientTest
{
    [TestClass]
    public class LoaderTest
    {
        private const int TEST_ID = 42;
        private const string TEST_NAME = "Homer Simpson ";
        private readonly DateTime _testTimestamp = new DateTime(2020, 02, 02, 14, 20, 0);

        [TestMethod]
        public void LoadTest()
        {
            TestModel model = new TestModel();
            LoaderFactory loaderFactory = new LoaderFactory();
            ILoader loader = loaderFactory.CreateLoader();
            Assert.IsInstanceOfType(loader, typeof(Loader));
            using (DataTableReader reader = CreateReader())
            {
                Assert.IsTrue(reader.Read());
                loader.Load(model, reader);
                reader.Close();
            }
            Assert.AreEqual(TEST_ID, model.Id);
            Assert.AreEqual(TEST_NAME.TrimEnd(), model.Name);
            Assert.AreEqual(_testTimestamp, model.Timestamp);
            Assert.AreEqual(1.01, model.Value);
        }

        private DataTableReader CreateReader()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Id", typeof(int));
            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Columns.Add("Timestamp", typeof(DateTime));

            dataTable.Rows.Add(TEST_ID, TEST_NAME, _testTimestamp);

            dataTable.AcceptChanges();
            return dataTable.CreateDataReader();
        }

        private sealed class TestModel
        {
            [ColumnMapping("Id")] public int Id { get; set; }
            [ColumnMapping] public string Name { get; set; }
            [ColumnMapping("Timestamp")] public DateTime Timestamp { get; set; }
            [ColumnMapping(IsOptional = true)] public double? Value { get; set; } = 1.01;
        }
    }
}
