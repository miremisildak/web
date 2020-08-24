
using b171200053.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TheStory.BusinessLayer;
using TheStory.Entities;
using TheStory.Entities.Messages;
using TheStory.Entities.MessagesCode;
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

        public ActionResult ShowProfile()
        {
            TheStoryUser currentUser = Session["login"] as TheStoryUser;

            TheStoryUserManager eum = new TheStoryUserManager();
            BusinessLayerResult<TheStoryUser> res = eum.GetUserById(currentUser.Id);

            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Hata Oluştu",
                    Items = res.Errors
                };

                return View("Error", errorNotifyObj);
            }

            return View(res.Result);
        }

        public ActionResult EditProfile()
        {
            TheStoryUser currentUser = Session["login"] as TheStoryUser;

            TheStoryUserManager eum = new TheStoryUserManager();
            BusinessLayerResult<TheStoryUser> res = eum.GetUserById(currentUser.Id);

            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Hata Oluştu",
                    Items = res.Errors
                };

                return View("Error", errorNotifyObj);
            }

            return View(res.Result);
        }

        [HttpPost]
        public ActionResult EditProfile(TheStoryUser model , HttpPostedFileBase ProfileImage)
        {
            if (ProfileImage != null &&
            (ProfileImage.ContentType == "image/jpeg" ||
             ProfileImage.ContentType == "image/jpg" ||
              ProfileImage.ContentType == "image/png"))
            {
                string filename = $"user_{model.Id}.{ProfileImage.ContentType.Split('/')[1]}";

                ProfileImage.SaveAs(Server.MapPath($"~/images/{filename}"));
                model.ProfileImageFilename = filename;
            }
            TheStoryUserManager eum = new TheStoryUserManager();
            BusinessLayerResult<TheStoryUser> res = TheStoryUserManager.UpdateProfile(model);

            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Items = res.Errors,
                    Title = "Profil Güncellenemedi.",
                    RedirectingUrl = "/Home/EditProfile"
                };

                return View("Error", errorNotifyObj);
            }

            Session["login"] = res.Result;

            return RedirectToAction("ShowProfile");
                   
    }

        public ActionResult RemoveProfile()
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

                OkViewModel okNotifyObj = new OkViewModel()
                {
                    Title = "Kayıt Başarılı",
                    RedirectingUrl = "/Home/Login",
                };

                okNotifyObj.Items.Add("Lütfen e-posta adresinize gönderdiğimiz aktivasyon link'ine tıklayarak hesabınızı aktive ediniz. Hesabınızı aktive etmeden not ekleyemez ve beğenme yapamazsınız.");

                return View("Ok", okNotifyObj);
            }

            return View(model);
        }


        public ActionResult UserActivate(Guid id)
        {

            TheStoryUserManager eum = new TheStoryUserManager();
            BusinessLayerResult<TheStoryUser> res = eum.ActivateUser(id);

            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Geçersiz İşlem",
                    Items = res.Errors
                };

                return View("Error", errorNotifyObj);
            }

            OkViewModel okNotifyObj = new OkViewModel()
            {
                Title = "Hesap Aktifleştirildi",
                RedirectingUrl = "/Home/Login"
            };

            okNotifyObj.Items.Add("Hesabınız aktifleştirildi. Artık not paylaşabilir ve beğenme yapabilirsiniz.");

            return View("Ok", okNotifyObj);
        }

        public ActionResult UserActivateOk()
        {


            return View();
        }

        public ActionResult UserActivateCancel()
        {
            List<ErrorMessageObj> errors = null;

            if (TempData["errors"] != null)
            {
                errors = TempData["errors"] as List<ErrorMessageObj>;
            }

            return View(errors);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }

    }
}