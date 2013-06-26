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
                }
            }
            else
            {
                // Create run time view services and models
                SimpleIoc.Default.Register<IMainViewModel, ViewModel.Runtime.MainViewModel>();
            }
        }

        public IMainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<IMainViewModel>();
            }
        }
    }
}
