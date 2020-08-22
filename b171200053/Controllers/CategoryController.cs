using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TheStory.BusinessLayer;
using TheStory.Entities;

namespace b171200053.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Category
        //public ActionResult Select(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadGateway);
                
        //    }

        //    CategoryManager cm = new CategoryManager();
        //    Category cat = cm.GetCategoryById(id.Value);

        //    if (cat == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    TempData["mm"] = cat.Notes;
        //    return RedirectToAction("Index","Home");
        //}
    }
}