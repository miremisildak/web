
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TheStory.BusinessLayer;
using TheStory.Entities;
using TheStory.Entities.Messages;
using TheStory.Entities.ValueObjects;

namespace b171200053.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            //if (TempData["mm"] != null)
            //{ controller üzerinden gelen yakalama
            //    return View(TempData["mm"] as List<Note>);
            //}

            NoteManager nm = new NoteManager();

          return View(nm.GetAllNote().OrderByDescending(x => x.ModifiedOn).ToList());
            //   return View(nm.GetAllNoteQueryable().OrderByDescending(x => x.ModifiedOn).ToList());

        }

        public ActionResult ByCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadGateway);

            }

            CategoryManager cm = new CategoryManager();
            Category cat = cm.GetCategoryById(id.Value);

            if (cat == null)
            {
                return HttpNotFound();
            }

            return View("Index", cat.Notes.OrderByDescending(x => x.ModifiedOn).ToList());
        }

        public ActionResult MostLiked()
        {
            NoteManager nm = new NoteManager();

            return View("Index",nm.GetAllNote().OrderByDescending(x => x.LikeCount).ToList());
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Login()
        { 

           
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                TheStoryUserManager eum = new TheStoryUserManager();
                BusinessLayerResult<TheStoryUser> res = eum.LoginUser(model);

                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));

                    if (res.Errors.Find(x => x.Code == ErrorMessageCode.UserIsNotActive) != null)
                    {
                        ViewBag.SetLink = "http://Home/Activate/1234-4567-78980";
                    }

                   
                    return View(model);
                }

                Session["login"] = res.Result;//session a kullanıcı bilgi saklama...
                return RedirectToAction("Index");//yönlendirme
            }

         
            return View();
        }
      

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {

            if (ModelState.IsValid)
            {
                TheStoryUserManager eum = new TheStoryUserManager();
                BusinessLayerResult<TheStoryUser> res = eum.RegisterUser(model);

                if (res.Errors.Count > 0)
                {
                   

                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(model);
                }

                

                return RedirectToAction("RegisterOk");

            }
            return View(model);
        }

        public ActionResult RegisterOk()
        {
            return View();
        }

        public ActionResult UserActivate(Guid activate_id)
        {
            //kullanıcı aktivasyonu sağlanacak
            return View();
        }
        

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }

    }
}