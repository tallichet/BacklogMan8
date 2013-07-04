using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BacklogMan.Client.Core.Service
{
    /// <summary>
    /// This class provide entry point for Story Service
    /// Story service must be implemented by Client project and registered on the ViewModel
    /// </summary>
    public interface IStorageService
    {
        void SaveSetting<T>(string key, T value);

        bool TryLoadSetting<T>(string key, out T value);
    }
}
