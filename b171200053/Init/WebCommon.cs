using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TheStory.Common;
using TheStory.Entities;

namespace b171200053.Init
{
    public class WebCommon : ICommon
    {
        public String GetCurrentUsername()
        {
            if (HttpContext.Current.Session["login"] != null)
            {
                TheStoryUser user = HttpContext.Current.Session["login"] as TheStoryUser;
                return user.Username;
            }

            return null;
        }

    }
}