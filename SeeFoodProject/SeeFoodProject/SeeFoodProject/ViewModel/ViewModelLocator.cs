using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;

namespace SeeFoodProject.ViewModel
{
    public class ViewModelLocator
    {

        static ViewModelLocator()
        {
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<MenuListingViewModel>();
        }

        public MainViewModel MainVM
        {
            get
            {
                return SimpleIoc.Default.GetInstance<MainViewModel>();
            }
        }
        public MenuListingViewModel MenuListingVM
        {
            get
            {
                return SimpleIoc.Default.GetInstance<MenuListingViewModel>();
            }
        }

    }
}
