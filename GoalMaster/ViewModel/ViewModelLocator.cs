/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:GoalMaster"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

namespace GoalMaster.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<WelcomeWindowViewModel>();
            SimpleIoc.Default.Register<RegisterWindowViewModel>();
            SimpleIoc.Default.Register<MainUserWindowViewModel>();
            SimpleIoc.Default.Register<AddGoalDefinitionViewModel>();
            SimpleIoc.Default.Register<OptionsViewModel>();
            SimpleIoc.Default.Register<EditOrDeleteGoalDefViewModel>();
        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }
        public MainViewModel Welcome
        {
            get
            {
                return ServiceLocator.Current.GetInstance<WelcomeWindowViewModel>();
            }
        }
        public MainViewModel Registration
        {
            get
            {
                return ServiceLocator.Current.GetInstance<RegisterWindowViewModel>();
            }
        }
        public MainViewModel MainUserWindow
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainUserWindowViewModel>();
            }
        }
        public MainViewModel AddGoalDefinition
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AddGoalDefinitionViewModel>();
            }
        }
        public MainViewModel Options
        {
            get
            {
                return ServiceLocator.Current.GetInstance<OptionsViewModel>();
            }
        }
        public MainViewModel EditOrDeleteGoalDef
        {
            get
            {
                return ServiceLocator.Current.GetInstance<EditOrDeleteGoalDefViewModel>();
            }
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}