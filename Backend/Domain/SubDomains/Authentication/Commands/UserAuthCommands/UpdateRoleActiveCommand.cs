using System;

namespace Domains.Authentication.Commands.UserAuthCommands
{
    public class UpdateRoleActiveCommand
    {
        public Guid Id { get; set; }
        public string Role { get; set; }
        public bool Active { get; set; }
    }
}