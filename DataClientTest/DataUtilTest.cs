using BrassLoon.DataClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Xml;

namespace DataClientTest
{
    [TestClass]
    public class DataUtilTest
    {
        [TestMethod]
        public void CreateParameterTest()
        {
            Mock<IDataParameter> mockParameter = new Mock<IDataParameter>();
            mockParameter.SetupAllProperties();
            Mock<IDbProviderFactory> providerFactory = new Mock<IDbProviderFactory>();
            providerFactory.Setup<IDataParameter>(f => f.CreateParameter()).Returns(mockParameter.Object);
            IDataParameter parameter = DataUtil.CreateParameter(providerFactory.Object, DbType.AnsiString);
            providerFactory.Verify<IDataParameter>(f => f.CreateParameter(), Times.Once);
            Assert.AreSame(mockParameter.Object, parameter);
            Assert.IsNull(parameter.ParameterName);
            Assert.AreEqual(DbType.AnsiString, parameter.DbType);
        }
        [TestMethod]
        public void CreateNamedParameterTest()
        {
            Mock<IDataParameter> mockParameter = new Mock<IDataParameter>();
            mockParameter.SetupAllProperties();
            Mock<IDbProviderFactory> providerFactory = new Mock<IDbProviderFactory>();
            providerFactory.Setup<IDataParameter>(f => f.CreateParameter()).Returns(mockParameter.Object);
            IDataParameter parameter = DataUtil.CreateParameter(providerFactory.Object, "testName", DbType.AnsiString);
            providerFactory.Verify<IDataParameter>(f => f.CreateParameter(), Times.Once);
            Assert.AreSame(mockParameter.Object, parameter);
            Assert.AreEqual("testName", parameter.ParameterName);
            Assert.AreEqual(DbType.AnsiString, parameter.DbType);
        }

        [TestMethod]
        public void GetParameterValueGuidTest()
        {
            Guid testGuid = Guid.NewGuid();
            Assert.AreEqual(testGuid.ToString("N"), ((Guid)DataUtil.GetParameterValue(testGuid)).ToString("N"));
        }

        [TestMethod]
        public void GetParameterValueGuidZeroTest() => Assert.IsTrue(DBNull.Value == DataUtil.GetParameterValue(Guid.Empty));

        [TestMethod]
        public void GetParameterValueGuidNullTest()
        {
            Guid? testGuid = null;
            Assert.IsTrue(DBNull.Value == DataUtil.GetParameterValue(testGuid));
        }

        [TestMethod]
        public void GetParameterValueStringTest() => Assert.AreEqual(" Test value", (string)DataUtil.GetParameterValue(" Test value ")); // notice that it will time spaces from the end

        [TestMethod]
        public void GetParameterValueStringNullTest() => Assert.AreSame(DBNull.Value, DataUtil.GetParameterValue(null, false));

        [TestMethod]
        public void GetParameterValueStringNullToEmptyTest() => Assert.AreEqual(string.Empty, (string)DataUtil.GetParameterValue(null, true));

        [TestMethod]
        public void GetParameterValueDecimalTest() => Assert.AreEqual(14.41M, (decimal)DataUtil.GetParameterValue(14.41M));

        [TestMethod]
        public void GetParameterValueDecimalNullTest()
        {
            decimal? value = null;
            Assert.AreSame(DBNull.Value, DataUtil.GetParameterValue(value));
        }

        [TestMethod]
        public void GetParameterValueDoubleTest() => Assert.AreEqual(14.41, (double)DataUtil.GetParameterValue(14.41));

        [TestMethod]
        public void GetParameterValueDoubleNullTest()
        {
            double? value = null;
            Assert.AreSame(DBNull.Value, DataUtil.GetParameterValue(value));
        }

        [TestMethod]
        public void GetParameterValueLongTest() => Assert.AreEqual(1441L, (long)DataUtil.GetParameterValue(1441L));

        [TestMethod]
        public void GetParameterValueLongNullTest()
        {
            long? value = null;
            Assert.AreSame(DBNull.Value, DataUtil.GetParameterValue(value));
        }

        [TestMethod]
        public void GetParameterValueIntegerTest()
        {
            int? value = 1441;
            Assert.AreEqual(value.Value, (int)DataUtil.GetParameterValue(value));
        }

        [TestMethod]
        public void GetParameterValueIntegerNullTest()
        {
            int? value = null;
            Assert.AreSame(DBNull.Value, DataUtil.GetParameterValue(value));
        }

        [TestMethod]
        public void GetParameterValueShortTest() => Assert.AreEqual((short)1441, (short)DataUtil.GetParameterValue(1441));

        [TestMethod]
        public void GetParameterValueShortNullTest()
        {
            short? value = null;
            Assert.AreSame(DBNull.Value, DataUtil.GetParameterValue(value));
        }

        [TestMethod]
        public void GetParameterValueByteTest() => Assert.AreEqual((byte)128, (byte)DataUtil.GetParameterValue(128));

        [TestMethod]
        public void GetParameterValueByteNullTest()
        {
            byte? value = null;
            Assert.AreSame(DBNull.Value, DataUtil.GetParameterValue(value));
        }

        [TestMethod]
        public void GetParameterValueDateTest()
        {
            DateTime? value = new DateTime(2020, 02, 02);
            Assert.AreEqual(value.Value, (DateTime)DataUtil.GetParameterValue(value));
        }

        [TestMethod]
        public void GetParameterValueDateNullTest()
        {
            DateTime? value = null;
            Assert.AreSame(DBNull.Value, DataUtil.GetParameterValue(value));
        }

        [TestMethod]
        public void GetParameterValueByteArrayTest()
        {
            byte[] value = new byte[] { 1, 0, 1 };
            byte[] result = (byte[])DataUtil.GetParameterValue(value);
            Assert.AreEqual(value.Length, result.Length);
        }

        [TestMethod]
        public void GetParameterValueByteArrayNullTest()
        {
            byte[] value = null;
            Assert.AreSame(DBNull.Value, DataUtil.GetParameterValue(value));
        }

        [TestMethod]
        public void GetParameterValueXmlTest()
        {
            XmlDocument document = new XmlDocument();
            document.AppendChild(document.CreateElement("test"));

            string result = (string)DataUtil.GetParameterValue(document.DocumentElement);
            Assert.IsNotNull(result);
            Assert.AreEqual("<test />", result);
        }

        [TestMethod]
        public void GetParameterValueXmlNullTest()
        {
            XmlNode value = null;
            Assert.AreSame(DBNull.Value, DataUtil.GetParameterValue(value));
        }

        [TestMethod]
        public void GetParameterValueBooleanTest()
        {
            bool? value = true;
            Assert.AreEqual(value.Value, (bool)DataUtil.GetParameterValue(value));
        }

        [TestMethod]
        public void GetParameterValueBooleanNullTest()
        {
            bool? value = null;
            Assert.AreSame(DBNull.Value, DataUtil.GetParameterValue(value));
        }

        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void GetParameterValueBooleanStringTest(bool value)
        {
            string expected = "Y";
            if (!value) expected = "N";
            Assert.AreEqual(expected, (string)DataUtil.GetParameterValueString(value));
        }

        [TestMethod]
        public void GetParameterValueBooleanStringNullTest()
        {
            bool? value = null;
            Assert.AreSame(string.Empty, (string)DataUtil.GetParameterValueString(value));
        }

        [TestMethod]
        public void AddParameterTest()
        {
            Mock<IDataParameter> dataParameter = new Mock<IDataParameter>();
            dataParameter.SetupAllProperties();
            Mock<IDbProviderFactory> providerFactory = new Mock<IDbProviderFactory>();
            providerFactory.Setup<IDataParameter>(f => f.CreateParameter()).Returns(dataParameter.Object);
            List<IDataParameter> parameters = new List<IDataParameter>();
            string name = "tacoTown";
            DbType dbType = DbType.Int32;
            int value = 7227;

            DataUtil.AddParameter(providerFactory.Object, parameters, name, dbType, value);
            providerFactory.Verify<IDataParameter>(f => f.CreateParameter(), Times.Once);
            Assert.AreEqual(1, parameters.Count);
            Assert.AreEqual("tacoTown", parameters[0].ParameterName);
            Assert.AreEqual(dbType, parameters[0].DbType);
            Assert.AreEqual(value, parameters[0].Value);
        }
    }
}
