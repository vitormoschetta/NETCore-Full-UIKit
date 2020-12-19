using System;

namespace Domains.Authentication.Commands.UserAuthCommands
{
    public class UpdatePasswordUserCommand
    {        
        public string Username { get; set; }
        public string Password { get; set; }    
        public string NewPassword { get; set; }        

    }
}