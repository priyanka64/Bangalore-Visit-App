using System;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Net.NetworkInformation;

using Windows.UI.Xaml;

using AppStudio.Services;
using AppStudio.Data;

namespace AppStudio.ViewModels
{
    public class MainViewModel : BindableBase
    {
       private HomeViewModel _homeModel;
       private AboutBangaloreViewModel _aboutBangaloreModel;
       private GalleryViewModel _galleryModel;
       private FactSheetsViewModel _factSheetsModel;
       private SightSeeingViewModel _sightSeeingModel;
        private PrivacyViewModel _privacyModel;

        private ViewModelBase _selectedItem = null;

        public MainViewModel()
        {
            _selectedItem = HomeModel;
            _privacyModel = new PrivacyViewModel();

        }
 
        public HomeViewModel HomeModel
        {
            get { return _homeModel ?? (_homeModel = new HomeViewModel()); }
        }
 
        public AboutBangaloreViewModel AboutBangaloreModel
        {
            get { return _aboutBangaloreModel ?? (_aboutBangaloreModel = new AboutBangaloreViewModel()); }
        }
 
        public GalleryViewModel GalleryModel
        {
            get { return _galleryModel ?? (_galleryModel = new GalleryViewModel()); }
        }
 
        public FactSheetsViewModel FactSheetsModel
        {
            get { return _factSheetsModel ?? (_factSheetsModel = new FactSheetsViewModel()); }
        }
 
        public SightSeeingViewModel SightSeeingModel
        {
            get { return _sightSeeingModel ?? (_sightSeeingModel = new SightSeeingViewModel()); }
        }

        public void SetViewType(ViewTypes viewType)
        {
            HomeModel.ViewType = viewType;
            AboutBangaloreModel.ViewType = viewType;
            GalleryModel.ViewType = viewType;
            FactSheetsModel.ViewType = viewType;
            SightSeeingModel.ViewType = viewType;
        }

        public ViewModelBase SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                SetProperty(ref _selectedItem, value);
                UpdateAppBar();
            }
        }

        public Visibility AppBarVisibility
        {
            get
            {
                return SelectedItem == null ? AboutVisibility : SelectedItem.AppBarVisibility;
            }
        }

        public Visibility AboutVisibility
        {

      get { return Visibility.Collapsed; }
        }

        public void UpdateAppBar()
        {
            OnPropertyChanged("AppBarVisibility");
            OnPropertyChanged("AboutVisibility");
        }

        /// <summary>
        /// Load ViewModel items asynchronous
        /// </summary>
        public async Task LoadDataAsync(bool forceRefresh = false)
        {
            var loadTasks = new Task[]
            { 
                HomeModel.LoadItemsAsync(forceRefresh),
                AboutBangaloreModel.LoadItemsAsync(forceRefresh),
                GalleryModel.LoadItemsAsync(forceRefresh),
                FactSheetsModel.LoadItemsAsync(forceRefresh),
                SightSeeingModel.LoadItemsAsync(forceRefresh),
            };
            await Task.WhenAll(loadTasks);
        }

        //
        //  ViewModel command implementation
        //
        public ICommand RefreshCommand
        {
            get
            {
                return new DelegateCommand(async () =>
                {
                    await LoadDataAsync(true);
                });
            }
        }

        public ICommand AboutCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    NavigationServices.NavigateToPage("AboutThisAppPage");
                });
            }
        }

        public ICommand PrivacyCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    NavigationServices.NavigateTo(_privacyModel.Url);
                });
            }
        }
    }
}
