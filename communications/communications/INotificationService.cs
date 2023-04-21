using System;
using System.Collections.Generic;
using System.Text;

namespace communications
{
    public interface INotificationService
    {
        void ShowNotification(string title, string message);
    }
}
