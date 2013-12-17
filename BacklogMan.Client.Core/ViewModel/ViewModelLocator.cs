using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BacklogMan.Client.Core.ViewModel
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (ViewModelBase.IsInDesignModeStatic)
            {
                if (!SimpleIoc.Default.IsRegistered<IMainViewModel>())
                {
                    // Create design time view services and models
                    SimpleIoc.Default.Register<IMainViewModel, ViewModel.Design.DesignMainViewModel>();
                    SimpleIoc.Default.Register<ISearchViewModel, ViewModel.Design.DesignSearchViewModel>();
                    SimpleIoc.Default.Register<IInternalNotificationViewModel, ViewModel.Design.DesignNotificationViewModel>();
                }
            }
            else
            {
                if (SimpleIoc.Default.IsRegistered<IMainViewModel>() == false)
                {
                    // Create runtime view services and models
                    SimpleIoc.Default.Register<IMainViewModel, ViewModel.Runtime.MainViewModel>();
                    SimpleIoc.Default.Register<ISearchViewModel, ViewModel.Runtime.SearchViewModel>();
                    SimpleIoc.Default.Register<IInternalNotificationViewModel, ViewModel.Runtime.InternalNotificationViewModel>();
                    SimpleIoc.Default.Register<Service.INetworkService, Service.NetworkService>();
                }
            }
        }

        public static void RegisterStorageService<T>() where T : class, Service.IStorageService
        {
            SimpleIoc.Default.Register<Service.IStorageService, T>();
        }

        public static void RegisterLocalizationService<T>() where T : class, Service.ILocalizationService
        {
            SimpleIoc.Default.Register<Service.ILocalizationService, T>();
        }

        public IMainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<IMainViewModel>();
            }
        }

        public ISearchViewModel Search
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ISearchViewModel>();
            }
        }

        public IInternalNotificationViewModel Notifications
        {
            get
            {
                return ServiceLocator.Current.GetInstance<IInternalNotificationViewModel>();
            }
        }
    }
}
