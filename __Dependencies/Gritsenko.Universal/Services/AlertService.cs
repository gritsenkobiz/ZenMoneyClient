using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using GalaSoft.MvvmLight.Ioc;
using Gritsenko.Universal.Abstract;
using Gritsenko.Universal.Common;
using Gritsenko.Universal.Extensions;

namespace Gritsenko.Universal.Services
{
    public static class AlertService
    {
        private static Color ErrorColor = Color.FromArgb(255, 217, 55, 74);
        private static Color NotifyColor = Color.FromArgb(255, 76, 213, 99);

        public async static Task Alert(string message)
        {
            Logger.LogEvent("Показываю сообщение: " + message);

            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            try
            {
                var dialog = new MessageDialog(message);
                await dialog.ShowAsync();
            }
            catch (System.Exception ex)
            {
#if TRACE
                Debug.WriteLine(ex.Message);
#endif
            }
        }

        /**********************************************************************************/

        public async static Task<bool> Confirmation(string header, string message)
        {
            Logger.LogEvent("Запрос подтверждения: " + header + " " + message);

            var result = false;
            if (string.IsNullOrEmpty(message))
            {
                return result;
            }

            try
            {

                var dialog = new MessageDialog(message, header);

                var okBtn = new UICommand("OK") {Invoked = command => result = true};
                var cancelBtn = new UICommand("Отмена") { Invoked = command => result = false };

                dialog.Commands.Add(okBtn);
                dialog.Commands.Add(cancelBtn);
                
                await dialog.ShowAsync();
            }
            catch (System.Exception ex)
            {
#if TRACE
                Debug.WriteLine(ex.Message);
#endif
            }
            return result;
        }

        /**********************************************************************************/

        public static void Error(string statusText)
        {
            Logger.LogEvent("Показываю ошибку: " + statusText);

            SimpleIoc.Default.GetInstance<IStatusBarService>().Do(x=>x.ShowHeader(statusText, Colors.White, ErrorColor));

            DefferedActionManager.Run(8000, HideNotification);
        }

        private static void HideNotification()
        {
            SimpleIoc.Default.GetInstance<IStatusBarService>().Do(x => x.HideHeader());
        }

        public static void Notify(string statusText)
        {
            Logger.LogEvent("Показываю уведомление: " + statusText);

            SimpleIoc.Default.GetInstance<IStatusBarService>().Do(x => x.ShowHeader(statusText, Colors.White, NotifyColor));

            DefferedActionManager.Run(5000, HideNotification);
        }

    }
}