namespace Domain.SubDomains.Authentication.Commands
{
    public class CommandResultToken : CommandResult
    {
        public CommandResultToken(
        bool success,
        string message,
        object objeto        
        ) : base(success, message, objeto)
        {
        }

        public string Token { get; set; }
    }
}