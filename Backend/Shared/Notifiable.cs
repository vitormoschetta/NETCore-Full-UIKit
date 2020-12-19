using System.Collections.Generic;
using System.Linq;

namespace Shared
{
    public abstract class Notifiable
    {
        private readonly List<string> _notifications;
        public Notifiable()
        {
            _notifications = new List<string>();
        }

        public IReadOnlyCollection<string> Notifications => _notifications;

        public void AddNotification(string message)
        {
            _notifications.Add(message);
        }

        public void AddNotification(List<string> notifications)
        {
            _notifications.AddRange(notifications);
        }

        public void AddNotification(Notifiable item)
        {
            AddNotifications(item.Notifications);
        }
        public void AddNotifications(Notifiable item)
        {
            AddNotifications(item.Notifications);
        }

        public void AddNotifications(IReadOnlyCollection<string> items)
        {
            foreach (var item in items)
                AddNotification(item);
        }

        public void Clear()
        {
            _notifications.Clear();
        }

        public bool Invalid => _notifications.Any();
        public bool Valid => !Invalid;
        
    }
}