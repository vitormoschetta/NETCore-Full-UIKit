using System;
using Domain.SubDomains.Authentication.Commands;
using Domains.Authentication.Commands.UserAuthCommands;

namespace Domain.SubDomains.Authentication.Contracts.Handlers
{
    public interface IUserAuthHandler
    {
        CommandResult Register(RegisterUserAuthCommand command);
        CommandResult RegisterAdmin(RegisterAdminUserAuthCommand command, string userIdentity);
        CommandResult ActivateFirstAccess(ActivateUserCommand command, string userIdentity);
        CommandResultToken Login(string userName, string password);
        CommandResult UpdateRoleActive(UpdateRoleActiveCommand command, string userIdentity);
        CommandResult UpdatePassword(UpdatePasswordUserCommand command, string userIdentity);
        CommandResult Delete(Guid id, string userIdentity);
    }
}