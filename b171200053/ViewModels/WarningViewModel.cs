using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace b171200053.ViewModels
{
    public class WarningViewModel:NotifyyViewModelBase<string>
    {
        public WarningViewModel()
        {
            Title = "Uyarı!";
        }
    }
}