using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace b171200053.ViewModels
{
    public class OkViewModel:NotifyyViewModelBase<string>
    {
        public OkViewModel()
        {
            Title = "İşlem Başarılı.";
        }
    }
}