using System.Linq;
using Domain.SubDomains.Authentication.Handlers;
using Domains.Authentication.Commands.UserAuthCommands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.Mock;

namespace Tests.Handlers
{
    [TestClass]
    public class UserAuthHandlerTest
    {
        [TestMethod]
        public void UserAuthHandler_Register_Valid()
        {
            var repository = new FakeUserAuthRepository();
            var logRepository = new FakeAccessLogRepository();
            var handler = new UserAuthHandler(repository, logRepository);
            var command = new RegisterUserAuthCommand();
            command.Username = "vitor";
            command.Password = "1234";
            var result = handler.Register(command);
            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        public void UserAuthHandler_RegisterAdmin_Valid()
        {
            var repository = new FakeUserAuthRepository();
            var logRepository = new FakeAccessLogRepository();
            var handler = new UserAuthHandler(repository, logRepository);
            var command = new RegisterAdminUserAuthCommand();
            command.Username = "vitor";
            command.Password = "1234";
            command.Role = "user";
            command.Active = true;
            var result = handler.RegisterAdmin(command, "userIdentity");
            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        public void UserAuthHandler_Login_Valid()
        {
            var repository = new FakeUserAuthRepository();
            var logRepository = new FakeAccessLogRepository();
            var handler = new UserAuthHandler(repository, logRepository);
            var result = handler.Login("admin", "123456");
            Assert.IsTrue(result.Success);
        }


        [TestMethod]
        public void UserAuthHandler_Login_Invalid()
        {
            var repository = new FakeUserAuthRepository();
            var logRepository = new FakeAccessLogRepository();
            var handler = new UserAuthHandler(repository, logRepository);
            var result = handler.Login("admin", "1234");
            Assert.IsFalse(result.Success);
        }

        [TestMethod]
        public void UserAuthHandler_Login_user_inactive_Invalid()
        {
            var repository = new FakeUserAuthRepository();
            var logRepository = new FakeAccessLogRepository();
            var handler = new UserAuthHandler(repository, logRepository);
            var result = handler.Login("newUser", "123456");
            Assert.IsFalse(result.Success);
        }

        [TestMethod]
        public void UserAuthHandler_Register_Pass_less_than_4_Characters_Invalid()
        {
            var repository = new FakeUserAuthRepository();
            var logRepository = new FakeAccessLogRepository();
            var handler = new UserAuthHandler(repository, logRepository);
            var command = new RegisterUserAuthCommand();
            command.Username = "vitor";
            command.Password = "123";
            var result = handler.Register(command);
            Assert.IsFalse(result.Success);
        }

        [TestMethod]
        public void UserAuthHandler_Register_User_less_than_4_Characters_Invalid()
        {
            var repository = new FakeUserAuthRepository();
            var logRepository = new FakeAccessLogRepository();
            var handler = new UserAuthHandler(repository, logRepository);
            var command = new RegisterUserAuthCommand();
            command.Username = "vit";
            command.Password = "1234";
            var result = handler.Register(command);
            Assert.IsFalse(result.Success);
        }

        
        [TestMethod]
        public void UserAuthHandler_Activate_Valid()
        {
            var repository = new FakeUserAuthRepository();
            var logRepository = new FakeAccessLogRepository();
            var handler = new UserAuthHandler(repository, logRepository);
            var command = new ActivateUserCommand();
            command.Id = repository.GetInactivesFirstAccess().FirstOrDefault().Id;
            command.Role = "user";           
            var result = handler.ActivateFirstAccess(command, "userIdentity");
            Assert.IsTrue(result.Success);
        }


        [TestMethod]
        public void UserAuthHandler_UpdatePassword_Valid()
        {
            var repository = new FakeUserAuthRepository();
            var logRepository = new FakeAccessLogRepository();
            var handler = new UserAuthHandler(repository, logRepository);
            var command = new UpdatePasswordUserCommand();
            command.Username = repository.GetAll().FirstOrDefault().Username;
            command.Password = "123456";    
            command.NewPassword = "1234";     
            var result = handler.UpdatePassword(command, "userIdentity");
            Assert.IsTrue(result.Success);
        }       

    }
}