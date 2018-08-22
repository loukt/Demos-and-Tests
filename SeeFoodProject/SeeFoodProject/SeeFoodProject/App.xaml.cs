using GalaSoft.MvvmLight.Ioc;
using SeeFoodProject.View;
using SeeFoodProject.ViewModel;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace SeeFoodProject
{
	public partial class App : Application
	{/*
        private static ViewModelLocator _locator;
        public static ViewModelLocator Locator
        {
            get { return _locator ?? (_locator = new ViewModelLocator()); }
        }
        */
        private static INavigation _navigationService;
        public static INavigation NavigationService
        {
            get { return _navigationService; }
        }

        public App ()
		{
            //NavigationService.Configure(PageLocator.MainPage, typeof(MainPage));
            //NavigationService.Configure(PageLocator.MenuListingPage, typeof(MenuListingPage));
            //SimpleIoc.Default.Register<GalaSoft.MvvmLight.Views.INavigationService>(() => NavigationService);
            //var firstPage = new NavigationPage(new MainPage());
            //NavigationService.Initialize(firstPage);

            InitializeComponent();
            
            MainPage = new ScanPage() ;
            _navigationService = MainPage.Navigation;
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
