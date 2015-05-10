using System;
using System.Linq;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Navigation;
using GalaSoft.MvvmLight.Ioc;
using Gritsenko.Universal.Abstract;
using Gritsenko.Universal.Extensions;
using Gritsenko.Universal.Mvvm;

namespace Gritsenko.Universal.Services
{
    public class NavigationService : INavigationService
    {
        private Frame _frame;
        private object _parameter;
        private Type _currentPageType;
        private Type _lastPageType;

        public string CurrentPath { get; internal set; }

        public  NavigationMode NavigationMode { get; private set; }

        public Type LastPageType
        {
            set
            {
                _lastPageType = value;
            }
            get { return _lastPageType; }
        }

        /**********************************************************************************/

        protected Frame RootFrame
        {
            get
            {
                if (_frame == null)
                {
                    _frame = Window.Current.Content as Frame;

                    _frame.Navigated += FrameOnNavigated;
                }
                return _frame;
            }
        }

        /**********************************************************************************/


        public Type HomePage { get; set; }

        /**********************************************************************************/

        private Type CurrentPage
        {
            get { return RootFrame.SourcePageType; }
        }


        /**********************************************************************************/
        // Properties
        /**********************************************************************************/
        public void Navigate(Type sourcePageType)
        {
            _parameter = null;
            Navigate(sourcePageType, _parameter);
        }

        /**********************************************************************************/

        public void Navigate(Type sourcePageType, object parameter)
        {
            try
            {
                Logger.LogEvent("Переход на эран {1} (параметр: {2})", CurrentPage.Name, sourcePageType.Name, parameter ?? "null");
                NavigationMode = NavigationMode.New;
                _parameter = parameter;
                RootFrame.Navigate(sourcePageType, parameter);
            }
            catch (Exception ex)
            {
                var message = "Ужас в [NavigationService.Navigate]: " + ex.Message;
                Logger.LogException(ex, message);
                throw new Exception(message, ex);
            }
        }

        /**********************************************************************************/

        private void FrameOnNavigated(object sender, NavigationEventArgs e)
        {
            try
            {
                var page = e.Content as FrameworkElement;

                if (_currentPageType != null)
                    LastPageType = _currentPageType;

                if (page != null)
                {
                    _currentPageType = page.GetType();
                    SaveCurrentPage(_currentPageType);
                }

                (page.DataContext as AsyncPageViewModelBase).Do(x =>
                {
                    CurrentPath = x.Title;
                    if(!string.IsNullOrEmpty(x.CurrentStateName))
                        CurrentPath += "(" + x.CurrentStateName + ")";

                    x.OnNavigatedTo(e.Parameter, e.NavigationMode);
                });
            }
            catch (Exception ex)
            {
                var message = "Ужас в [NavigationService.FrameOnNavigated]: " + ex.Message;
                Logger.LogException(ex, message);
                throw new Exception(message, ex);
            }
        }

        /**********************************************************************************/

        public void Navigate(string sourcePageTypeName, string parameter)
        {
            //var type = Type.GetType(sourcePageTypeName);
            var type = ((IXamlMetadataProvider)Application.Current).GetXamlType(sourcePageTypeName);

            if (type != null)
            {
                Navigate(type.UnderlyingType, parameter);
            }
        }

        /**********************************************************************************/

        public bool CanGoBack()
        {
            return RootFrame.CanGoBack;
        }

        /**********************************************************************************/

        public bool GoBack()
        {
            Logger.LogEvent("Navigating Back from {0}", CurrentPage.Name);

#if(DEBUG)
            if (HomePage != null && !RootFrame.BackStack.Any() && CurrentPage != HomePage)
            {
                Logger.LogEvent("### BACKSTACK IS EMPTY, GOING TO HOMEPAGE", CurrentPage.Name);

                Navigate(HomePage);
                return true;
            }
#endif
            NavigationMode = NavigationMode.Back;
            _parameter = null;

            if (CanGoBack() && RootFrame.BackStack.Any())
            {
                RootFrame.GoBack();
                return true;
            }

            return false;
        }

        /**********************************************************************************/

        public void ClearBackStack()
        {
                RootFrame.BackStack.Clear();
        }

        /**********************************************************************************/

        public object GetLastParameter()
        {
            return _parameter;
        }

        public bool HasParameter()
        {
            return _parameter != null;
        }

        /**********************************************************************************/

        public TParam GetLastParameter<TParam>()
        {
            return (TParam)GetLastParameter();
        }

        /**********************************************************************************/

        /**********************************************************************************/
        private void SaveCurrentPage(Type value)
        {
            var settings = SimpleIoc.Default.GetInstance<ISettingsService>();

            settings.Save("LastPageType", value);
        }

    }
}