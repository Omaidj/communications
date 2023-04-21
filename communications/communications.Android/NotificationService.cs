using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using communications.Droid.Services;
using communications.Droid;
using Xamarin.Forms;
using AndroidX.Core.App;

[assembly: Dependency(typeof(NotificationService))]
namespace communications.Droid.Services
{
    public class NotificationService : INotificationService
    {
        private const string ChannelId = "default";

        public void ShowNotification(string title, string message)
        {
            var notificationManager = (NotificationManager)Forms.Context.GetSystemService(Context.NotificationService);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channel = new NotificationChannel(ChannelId, "Default", NotificationImportance.Default);
                notificationManager.CreateNotificationChannel(channel);
            }

            var notificationBuilder = new NotificationCompat.Builder(Forms.Context, ChannelId)
                .SetContentTitle(title)
                .SetContentText(message)
                .SetSmallIcon(Resource.Drawable.logo);

            var notification = notificationBuilder.Build();
            notificationManager.Notify(0, notification);
        }
    }
}
