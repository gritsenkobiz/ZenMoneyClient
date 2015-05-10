using System;
using Windows.UI.Xaml.Navigation;

namespace Gritsenko.Universal.Abstract
{
    public interface INavigationService
    {
        Type HomePage { get; set; }

        string CurrentPath { get; }

        NavigationMode NavigationMode { get; }
        Type LastPageType { get; }

        void Navigate(Type sourcePageType);
        void Navigate(Type sourcePageType, object parameter);
        void Navigate(string sourcePageTypeName, string parameter);
        void ClearBackStack();

        bool CanGoBack();
        bool GoBack();

        object GetLastParameter();
        bool HasParameter();
        TParam GetLastParameter<TParam>();
    }
}