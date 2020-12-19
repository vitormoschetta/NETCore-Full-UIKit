namespace Domain
{
    public class CommandResult
    {
        public CommandResult(
            bool success,
            string message,
            object objeto           
        )
        {
            Success = success;
            Message = message;
            Object = objeto;            
        }

        public bool Success { get; set; }
        public string Message { get; set; }
        public object Object { get; set; }        
    }
}