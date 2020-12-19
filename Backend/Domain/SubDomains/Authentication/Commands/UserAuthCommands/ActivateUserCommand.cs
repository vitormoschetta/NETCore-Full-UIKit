using System;

namespace Domains.Authentication.Commands.UserAuthCommands
{
    public class ActivateUserCommand
    {
        public Guid Id { get; set; }               
        public string Role { get; set; }        
    }
}