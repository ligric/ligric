using System.Collections.Generic;

namespace Ligric.Application.Notifications
{
    public interface INotificationHandler
    {
        void Add(string key, string value);
        void AddRange(List<Notification> notifications);
        List<string> GetErrors();
        List<Notification> GetList();
        List<Notification> GetListAndReset();
        bool HasAny();
        void Reset();

    }
}
