using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.Mock;

namespace Tests.Repository
{
    [TestClass]
    public class UserRepositoryTest
    {
        [TestMethod]
        public void UserSearch_Valid()
        {
            var repository = new FakeUserAuthRepository();  
            var list = repository.Search("user");
            Assert.AreEqual(3, list.Count);         
        }

    }
}