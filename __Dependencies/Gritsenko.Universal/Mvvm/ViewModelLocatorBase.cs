/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:RocketBank"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace Gritsenko.Universal.Mvvm
{
    public abstract class ViewModelLocatorBase 
    {

        /**********************************************************************************/
        // Functions
        /**********************************************************************************/

        protected ViewModelLocatorBase()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
        }

        /**********************************************************************************/

        public T GetViewModel<T>() where T : new()
        {
            try
            {
                T vm;

                if (ViewModelBase.IsInDesignModeStatic)
                {
                    vm = new T();
                }
                else
                {
                    vm = ServiceLocator.Current.GetInstance<T>();
                }

                var asyncVm = vm as AsyncPageViewModelBase;
                if (asyncVm != null)
                {
                    asyncVm.Initialize();
                }

                return vm;
            }
            catch (System.Exception ex)
            {
                const string exMessage = "Exception in (ViewModelLocator.cs)\\[ViewModelLocator.GetViewModel] ";
                Logger.LogException(ex, exMessage);
                throw new Exception(exMessage, ex);
            }
        }

        /**********************************************************************************/

        public static void Cleanup()
        {
            SimpleIoc.Default.Reset();
        }

    }
}