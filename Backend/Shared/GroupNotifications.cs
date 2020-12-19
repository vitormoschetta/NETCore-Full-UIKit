using System.Collections.Generic;

namespace Shared
{
    public class GroupNotifications
    {
        public static string Group(IReadOnlyCollection<string> notifications)
        {
            var message = string.Empty;
            foreach (var notification in notifications)
            {
                message += $"{notification}. ";
            }
            return message;
        }
    }
}