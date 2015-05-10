using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Navigation;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using Gritsenko.Universal.Abstract;
using Gritsenko.Universal.Common;
using Gritsenko.Universal.Services;

namespace Gritsenko.Universal.Mvvm
{
    public abstract class AsyncPageViewModelBase<TDataService> : AsyncPageViewModelBase
        where TDataService : class 
    {
        protected static TDataService _dataService;

        protected virtual TDataService DataService
        {
            get { return _dataService ?? (_dataService = GetService<TDataService>()); }
        }

        /**********************************************************************************/
        
    }

    public abstract class AsyncPageViewModelBase : ViewModelBase
    {
        private IDispatcherHelper _uiThread;

        protected IDispatcherHelper UiThread
        {
            get { return _uiThread ?? (_uiThread = GetService<IDispatcherHelper>()); }
        }

        /**********************************************************************************/

        private static INavigationService _navigationService;
        private string _title;
        private bool _isDataLoading;
        private string _currentStateName;
        private static ISettingsService _settingservice;
        private string _statusText = "Загрузка...";
        private IStatusBarService _statusBarService;
        private ICommand _actualizeDataCommand;

        /**********************************************************************************/

        public ICommand ActualizeDataCommand
        {
            get { return _actualizeDataCommand ?? (_actualizeDataCommand = new RelayCommand(OnActualizeDataCommandExecute)); }
        }

        protected INavigationService NavigationService
        {
            get { return _navigationService ?? (_navigationService = GetService<INavigationService>()); }
        }

        /**********************************************************************************/

        protected ISettingsService Settings
        {
            get { return _settingservice ?? (_settingservice = GetService<ISettingsService>()); }
        }

        /**********************************************************************************/

        protected IStatusBarService StatusBarService
        {
            get { return _statusBarService ?? (_statusBarService = GetService<IStatusBarService>()); }
        }

        /**********************************************************************************/

        public bool IsInitialized { get; set; }

        /**********************************************************************************/

        public bool ReloadAlways { get; set; }

        /**********************************************************************************/

        protected TService GetService<TService>()
        {
            return SimpleIoc.Default.GetInstance<TService>();
        }

        /**********************************************************************************/

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value; 
                RaisePropertyChanged();
            }
        }

        /**********************************************************************************/

        public string CurrentStateName
        {
            get { return _currentStateName; }
            set
            {
                _currentStateName = value; 
                RaisePropertyChanged();
            }
        }

        /**********************************************************************************/

        public bool IsDataLoading
        {
            get { return _isDataLoading; }
            set
            {
                if (_isDataLoading == value)
                {
                    return;
                }

                _isDataLoading = value;

                DefferedActionManager.Run(_isDataLoading ? 0 : 500, OnIsDataLodaingChaged, "IsDataLoadingChaged");
            }
        }

        /**********************************************************************************/

        public string StatusText
        {
            get { return _statusText; }
            set
            {
                _statusText = value;

                UiThread.Run(() =>
                {
                    UpdateBusyIndicator();
                    RaisePropertyChanged(() => StatusText);
                });
            }
        }

        /**********************************************************************************/

        protected abstract void UpdateBusyIndicator();

        /**********************************************************************************/


        public async void LoadData(bool forceUiTrhead = false)
        {
            if (IsDataLoading)
            {
                return;
            }

            if (IsInDesignMode)
            {
                OnLoadData();
                return;
            }


            try
            {
                if (forceUiTrhead)
                {
                    UiThread.Run(async () => await LoadDataAsync());
                }
                else
                {
                    await LoadDataAsync();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                IsDataLoading = false;
            }
        }

        /**********************************************************************************/

        private async Task LoadDataAsync()
        {
            try
            {
                IsDataLoading = true;

                await OnLoadData();

                IsDataLoading = false;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "Exception in (AsyncLoadViewModelBase.cs)\\[AsyncLoadViewModelBase.LoadDataAsync] ");
                throw;
            }
        }

        /**********************************************************************************/

        public abstract Task OnLoadData();

        /**********************************************************************************/

        public void Initialize()
        {
            try
            {
                if (IsInitialized && !ReloadAlways)
                {
                    return;
                }
            
                IsInitialized = true;
                LoadData();
            }
            catch (System.Exception ex)
            {
                const string exMessage = "Exception in (AsyncPageViewModelBase.cs)\\[AsyncPageViewModelBase.Initialize] ";
                Logger.LogException(ex, exMessage);
                throw new Exception(exMessage, ex);
            }
        }

        /**********************************************************************************/

        protected void SaveSetting(string key, object value)
        {
            Settings.Save(key, value);
        }

        /**********************************************************************************/

        protected T LoadSetting<T>(string key)
        {
            return Settings.Load<T>(key);
        }

        /**********************************************************************************/

        public void SetState(string stateName)
        {
            Logger.LogEvent("Changing state from {0} to {1} in Page: {2}", CurrentStateName, stateName, this.GetType().Name);

            CurrentStateName = stateName;
        }

        /**********************************************************************************/

        protected async Task ShowMessage(string message)
        {
            await AlertService.Alert(message);
        }

        /**********************************************************************************/
        private void OnActualizeDataCommandExecute()
        {
            LoadData();
        }

        /**********************************************************************************/

        public void OnNavigatedTo(object parameter, NavigationMode mode)
        {
            OnNavigatedToOverride(parameter, mode);
        }

        /**********************************************************************************/

        internal void OnIsDataLodaingChaged()
        {
            UiThread.Run(() =>
            {
                UpdateBusyIndicator();
                RaisePropertyChanged(() => IsDataLoading);
            });
        }

        /**********************************************************************************/

        protected virtual void OnNavigatedToOverride(object parameter, NavigationMode mode)
        {
            
        }

        public virtual bool OnBackButtonPressed()
        {
            if (NavigationService.CanGoBack())
            {
                NavigationService.GoBack();
                return true;
            }

            return false;
        }
    }
}