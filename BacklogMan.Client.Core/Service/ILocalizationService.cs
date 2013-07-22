using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BacklogMan.Client.Core.Service
{
    public interface ILocalizationService
    {
        string GetStringForKey(string key);
    }
}
