using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.PushNotifications;
using Windows.UI.Xaml;

namespace Gritsenko.Universal.Common
{
    public class PushHelper
    {
        private static PushNotificationChannel _channel;
        private static Exception _error;
        private static string _token;

        public static async Task<String> GetPushToken(Action<string> pushCallback = null)
        {
            try
            {
                _pushCallback = pushCallback;
                if (_channel != null || _error != null)
                {
                    return _token;
                }

                _channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
                _channel.PushNotificationReceived += OnPushNotification;
#if  TRACE
                Debug.WriteLine(_channel.Uri);
#endif
                _token = _channel.Uri.Substring(_channel.Uri.IndexOf("=", StringComparison.Ordinal)+1);
#if  TRACE
                Debug.WriteLine("Extracted token:" + _token);
#endif
                return _token;
            }
            catch (System.Exception ex)
            {
                var exMessage = "Exception in (PushHelper.cs)\\[PushHelper.GetPushToken] " + ex.Message;
                Logger.LogException(ex, exMessage);
                _error = ex;
            }

            return null;
        }

        string content = null;
        private static Action<string> _pushCallback;

        private static async void OnPushNotification(PushNotificationChannel sender, PushNotificationReceivedEventArgs e)
        {
            String notificationContent = String.Empty;

            switch (e.NotificationType)
            {
                case PushNotificationType.Badge:
                    notificationContent = e.BadgeNotification.Content.GetXml();
                    break;

                case PushNotificationType.Tile:
                    notificationContent = e.TileNotification.Content.GetXml();
                    break;

                case PushNotificationType.Toast:
                    notificationContent = e.ToastNotification.Content.GetXml();
                    break;

                case PushNotificationType.Raw:
                    notificationContent = e.RawNotification.Content;
                    break;
            }

            if (_pushCallback != null)
            {
                _pushCallback(notificationContent);
            }
//            e.Cancel = true;
        }
    }
}
