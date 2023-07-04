using BrassLoon.DataClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataClientTest
{
    [TestClass]
    public class DataStateManagerTest
    {
        [TestMethod]
        public void GetStateNewTest()
        {
            DataStateManager dataStateManager = new DataStateManager();
            Assert.AreEqual(DataState.New, dataStateManager.GetState(new { Test = 1 }));
        }

        [TestMethod]
        public void GetStateUpdateTest()
        {
            TestModel model = new TestModel()
            {
                Name = "Homer"
            };
            DataStateManager dataStateManager = new DataStateManager(model.Clone());
            model.Name = "Marge";
            Assert.AreEqual(DataState.Updated, dataStateManager.GetState(model));
        }

        [TestMethod]
        public void GetStateUnchangedTest()
        {
            TestModel model = new TestModel()
            {
                Name = "Homer"
            };
            DataStateManager dataStateManager = new DataStateManager(model.Clone());
            model.Name = "Homer";
            Assert.AreEqual(DataState.Unchanged, dataStateManager.GetState(model));
        }

        [TestMethod]
        public void GetStateTargetNullTest()
        {
            TestModel model = new TestModel()
            {
                Name = "Homer"
            };
            DataStateManager dataStateManager = new DataStateManager(model.Clone());
            Assert.AreEqual(DataState.Unchanged, dataStateManager.GetState(null));
        }

        [TestMethod]
        public void IsChangedStringTest()
        {
            TestModel original = new TestModel() { Name = "Homer" };
            TestModel target = new TestModel() { Name = "Marge" };
            DataStateManager dataStateManager = new DataStateManager();
            Assert.IsTrue(dataStateManager.IsChanged(typeof(TestModel).GetProperty("Name"), original, target));
        }

        [TestMethod]
        public void IsChangedByteArrayTest()
        {
            TestModel original = new TestModel() { Name = "Homer", Data = new byte[] { 0, 0 } };
            TestModel target = new TestModel() { Name = "Homer", Data = new byte[] { 1, 1 } };
            DataStateManager dataStateManager = new DataStateManager();
            Assert.IsTrue(dataStateManager.IsChanged(typeof(TestModel).GetProperty("Data"), original, target));
        }

        [TestMethod]
        public void IsChangedNullableTest()
        {
            TestModel original = new TestModel() { Name = "Homer", Date = new DateTime(2010, 1, 1) };
            TestModel target = new TestModel() { Name = "Homer", Date = new DateTime(2010, 1, 2) };
            DataStateManager dataStateManager = new DataStateManager();
            Assert.IsTrue(dataStateManager.IsChanged(typeof(TestModel).GetProperty("Date"), original, target));
        }

        [TestMethod]
        public void IsChangedNullTest()
        {
            TestModel original = new TestModel() { Name = "Homer" };
            TestModel target = new TestModel() { Name = null };
            DataStateManager dataStateManager = new DataStateManager();
            Assert.IsTrue(dataStateManager.IsChanged(typeof(TestModel).GetProperty("Name"), original, target));
        }

        [TestMethod]
        public void IsByteArrayChangedTest()
        {
            byte[] array1 = new byte[] { 0, 0, 1 };
            byte[] array2 = new byte[] { 0, 0, 1 };
            DataStateManager dataStateManager = new DataStateManager();
            Assert.IsFalse(dataStateManager.IsByteArrayChanged(array1, array2));
        }

        [TestMethod]
        public void IsByteArrayChangedDiffTest()
        {
            byte[] array1 = new byte[] { 0, 0, 1 };
            byte[] array2 = new byte[] { 0, 1, 1 };
            DataStateManager dataStateManager = new DataStateManager();
            Assert.IsTrue(dataStateManager.IsByteArrayChanged(array1, array2));
        }

        [TestMethod]
        public void IsByteArrayChangedLengthDiffTest()
        {
            byte[] array1 = new byte[] { 0, 0 };
            byte[] array2 = new byte[] { 0, 0, 0 };
            DataStateManager dataStateManager = new DataStateManager();
            Assert.IsTrue(dataStateManager.IsByteArrayChanged(array1, array2));
        }

        private class TestModel : ICloneable
        {
            [ColumnMapping("Name")] public string Name { get; set; }
            [ColumnMapping("Data")] public byte[] Data { get; set; }
            [ColumnMapping("Date")] public DateTime? Date { get; set; }

            public object Clone() => this.MemberwiseClone();
        }
    }
}
