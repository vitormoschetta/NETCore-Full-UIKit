using System;

namespace Domains.Authentication.Commands.UserAuthCommands
{
    public class UpdateUserAuthCommand
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public bool Active { get; set; }
    }
}