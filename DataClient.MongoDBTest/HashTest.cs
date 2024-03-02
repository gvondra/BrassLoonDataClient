using BrassLoon.DataClient.MongoDB;

namespace DataClient.MongoDBTest
{
    [TestClass]
    public class HashTest
    {
        [TestMethod]
        public void ComputeTest()
        {
            string[] results = new string[]
            {
                Hash.Compute(" TEST  VALUE   ONE "),
                Hash.Compute("test value one "),
                Hash.Compute("testvalue one "),
                Hash.Compute("test value 2")
            };
            Assert.AreEqual(results[0], results[1]);
            Assert.AreNotEqual(results[0], results[2]);
            Assert.AreNotEqual(results[0], results[3]);
        }
    }
}
