using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace b171200053.ViewModels
{
    public class NotifyyViewModelBase<T>
    {
        public List<T> Items { get; set; }
        public string Header { get; set; }
        public string Title { get; set; }
        public bool IsRedirecting { get; set; }
        public string RedirectingUrl { get; set;}
        public int RedirectingTimeout { get; set; }

        public NotifyyViewModelBase()
        {
            Header = "Yönlendiriliyorsunuz!";
            Title = "Geçersiz İşlem";
            IsRedirecting = true;
            RedirectingUrl = "/Home/Index";
            RedirectingTimeout = 10000;
            Items = new List<T>();
        }

    }
}