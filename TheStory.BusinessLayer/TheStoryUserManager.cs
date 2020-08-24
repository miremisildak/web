
using TheStory.Common.Helpers;
using TheStory.DataAccessLayer.EntityFramework;
using TheStory.Entities;
using TheStory.Entities.Messages;
using TheStory.Entities.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyEvernote.Common.Helpers;

namespace TheStory.BusinessLayer
{
   public class TheStoryUserManager
    {
        private Repository<TheStoryUser> repo_user = new Repository<TheStoryUser>();

        public BusinessLayerResult<TheStoryUser> RegisterUser(RegisterViewModel data)
        {
          TheStoryUser user = repo_user.Find(x => x.Username == data.Username || x.Email == data.EMail);
            BusinessLayerResult<TheStoryUser> res = new BusinessLayerResult<TheStoryUser>();

            if (user != null)
            {
                if (user.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı adı kayıtlı.");
                }

                if (user.Email == data.EMail)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExists, "E-posta adresi kayıtlı.");

                }
            }

            else {
                int dbResult = repo_user.Insert(new TheStoryUser()
                {
                    Username = data.Username,
                    Email = data.EMail,
                    ProfileImageFilename ="user1.png",
                    Password = data.Password,
                    ActivateGuid = Guid.NewGuid(),
                    IsActive = false,
                    IsAdmin = false
                });
                
                if (dbResult > 0)
                {
                    res.Result = repo_user.Find(x => x.Email == data.EMail && x.Username == data.Username);

                    string siteUri = Confighelper.Get<string>("SiteRootUri");
                    string activateUri = $"{siteUri}/Home/UserActivate/{res.Result.ActivateGuid}";
                    string body = $"Merhaba {res.Result.Username};<br><br>Hesabınızı aktifleştirmek için <a href='{activateUri}' target='_blank'>tıklayınız</a>.";

                    MailHelper.SendMail(body, res.Result.Email, "MyEvernote Hesap Aktifleştirme");

                }
            }

            return res;
        }

        public BusinessLayerResult<TheStoryUser> GetUserById(int id)
        {
            BusinessLayerResult<TheStoryUser> res = new BusinessLayerResult<TheStoryUser>();
            res.Result = repo_user.Find(x => x.Id == id);

            if (res.Result == null)
            {
                res.AddError(ErrorMessageCode.UserNotFound, "Kullanıcı bulunamadı.");
            }

            return res;
        }

        public BusinessLayerResult<TheStoryUser> LoginUser(LoginViewModel data)
        {
            BusinessLayerResult<TheStoryUser> res = new BusinessLayerResult<TheStoryUser>();
           res.Result = repo_user.Find(x => x.Username == data.Username && x.Password == data.PassWord);
           


            if (res.Result != null)
            {
                if (!res.Result.IsActive)
                {
                    res.AddError(ErrorMessageCode.UserIsNotActive,
                        "Kullanıcı aktifleştirilmiştir. ");
                    res.AddError(ErrorMessageCode.CheckYourEmail,
                       "Lütfen e-posta adresinizi kontrol ediniz.");

                }
            }

            else
            {
                res.AddError(ErrorMessageCode.UsernameOrPassWrong, "kullanıcı adı ya da şifre uyuşmuyor.");
            }

            return res;
        }

        public BusinessLayerResult<TheStoryUser> ActivateUser(Guid activateId)
        {
            BusinessLayerResult<TheStoryUser> res = new BusinessLayerResult<TheStoryUser>();
            res.Result = repo_user.Find(x => x.ActivateGuid == activateId);

            if (res.Result != null)
            {
                if (res.Result.IsActive)
                {
                    res.AddError(ErrorMessageCode.UserAlreadyActivate, "Kullanıcı zaten aktif edilmiştir.");
                    return res;
                }

                res.Result.IsActive = true;
                repo_user.Update(res.Result);
            }

            else
            {
                res.AddError(ErrorMessageCode.ActivateIdDoesNotExists, "Aktifleştirilecek kullanıcı bulunamadı.");

            }

            return res;

        }

        public static BusinessLayerResult<TheStoryUser> UpdateProfile(TheStoryUser data)
        {
            TheStoryUser db_user = repo_user.Find(x => x.Username == data.Username || x.Email == data.Email);
            BusinessLayerResult<TheStoryUser> res = new BusinessLayerResult<TheStoryUser>();
            res.Result = data;

            if (db_user != null && db_user.Id != data.Id)
            {
                if (db_user.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı adı kayıtlı.");
                }

                if (db_user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExists, "E-posta adresi kayıtlı.");
                }

                return res;
            }

            res.Result = Find(x => x.Id == data.Id);
            res.Result.Email = data.Email;
            res.Result.Name = data.Name;
            res.Result.Surname = data.Surname;
            res.Result.Password = data.Password;
            res.Result.Username = data.Username;
            res.Result.IsActive = data.IsActive;
            res.Result.IsAdmin = data.IsAdmin;

            if (base.Update(res.Result) == 0)
            {
                res.AddError(ErrorMessageCode.UserCouldNotUpdated, "Kullanıcı güncellenemedi.");
            }

            return res;
        }
    }
}
