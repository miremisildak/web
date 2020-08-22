using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheStory.DataAccessLayer.EntityFramework;
using TheStory.Entities;
using TheStory.Entities.Messages;
using TheStory.Entities.ValueObjects;

namespace TheStory.BusinessLayer
{
   public class TheStoryUserManager
    {
        private Repository<TheStoryUser> repo_user = new Repository<TheStoryUser>();

        public BusinessLayerResult<TheStoryUser> RegisterUser(RegisterViewModel data)
        {
          TheStoryUser user =  repo_user.Find(x => x.Username == data.Username || x.Email == data.EMail);
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
                    Password= data.Password,
                    ActivateGuid = Guid.NewGuid(),
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                });
                
                if (dbResult > 0)
                {
                  res.Result = repo_user.Find(x => x.Email == data.EMail && x.Username == data.Username);
                    //layerResult.Result.ActivateGuid
                }
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
    }
}
