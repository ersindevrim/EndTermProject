using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShareYourNote.Business.Abstract;
using ShareYourNote.Business.Results;
using ShareYourNote.Common.Helpers;
using ShareYourNote.DataAccess.EntityFramework;
using ShareYourNote.Entities;
using ShareYourNote.Entities.Messages;
using ShareYourNote.Entities.ValueObjects;

namespace ShareYourNote.Business
{
    public class UserManager : ManagerBase<ShareYourNoteUser>
    {        
        public BusinessResult<ShareYourNoteUser> RegisterNewUser(RegisterViewModel model)
        {
            ShareYourNoteUser user = Find(x => x.Username == model.UserName || x.Email == model.Email);
            BusinessResult<ShareYourNoteUser> result = new BusinessResult<ShareYourNoteUser>();

            if (user != null)
            {
                if (user.Username == model.UserName)
                {
                    result.AddError(ErrorMessages.UsernameAlreadyExist, "Kullanıcı adı kayıtlı.");
                }

                if (user.Email == model.Email)
                {
                    result.AddError(ErrorMessages.EmailAlreadyExist, "E-Posta Kullanımda.");
                }
            }
            else
            {
                int dbresult = base.Insert(new ShareYourNoteUser()
                {
                    Username = model.UserName,
                    Email = model.Email,
                    Password = model.Password,
                    ProfilePhotoName = "defaultImage.png",
                    ActivateGuid = Guid.NewGuid(),
                    IsActive = false,
                    IsAdmin = false
                });

                if (dbresult > 0)
                {
                    result.Result = Find(x => x.Email == model.Email && x.Username == model.UserName);

                    //TODO : AKTIVASYON MAILI ATILACAK.

                    string siteUrl = ConfigHelper.Get<string>("SiteRootUrl");
                    string activateUrl = $"{siteUrl}/Home/ActivateUser/{result.Result.ActivateGuid}";
                    string body = $"Hesabınızı aktifleştirmek için lütfen <a href='{activateUrl}' target='_blank'>tıklayınız</a>.";

                    MailHelper.SendMail(body, result.Result.Email, "Share Your Note Activation");
                }
            }

            return result;
        }

        public BusinessResult<ShareYourNoteUser> GetUserById(int id)
        {
            BusinessResult<ShareYourNoteUser> result = new BusinessResult<ShareYourNoteUser>();
            result.Result = Find(x => x.Id == id);

            if (result.Result == null)
            {
                result.AddError(ErrorMessages.UserNotFound, "Kullanıcı Bulunamadı");
            }

            return result;
        }

        public BusinessResult<ShareYourNoteUser> LoginUser(LoginViewModel model)
        {

            BusinessResult<ShareYourNoteUser> result = new BusinessResult<ShareYourNoteUser>();
            result.Result = Find(x => x.Username == model.UserName && x.Password == model.Password);


            if (result.Result != null)
            {
                if (!result.Result.IsActive)
                {
                    result.AddError(ErrorMessages.UserIsNotActive, "Kullanıcı aktifleştirilmemiştir.");
                }
            }
            else
            {
                result.AddError(ErrorMessages.UsernameOrPassWrong, "Kullanıcı adı yada şifre hatalı.");
            }

            return result;
        }

        public BusinessResult<ShareYourNoteUser> UpdateProfile(ShareYourNoteUser model)
        {
            ShareYourNoteUser user = Find(x => x.Id != model.Id && (x.Username == model.Username || x.Email == model.Email));
            BusinessResult<ShareYourNoteUser> result = new BusinessResult<ShareYourNoteUser>();

            if (user != null && user.Id != model.Id)
            {
                if (user.Username == model.Username)
                {
                    result.AddError(ErrorMessages.UsernameAlreadyExist, "Kullanıcı Adı Kullanımda");

                }
                if (user.Email == model.Email)
                {
                    result.AddError(ErrorMessages.EmailAlreadyExist, "E-Posta Adresi kullanımda");
                }
                return result;
            }

            result.Result = Find(x => x.Id == model.Id);
            result.Result.Email = model.Email;
            result.Result.Name = model.Name;
            result.Result.Surname = model.Surname;
            result.Result.Password = model.Password;
            result.Result.Username = model.Username;

            if (string.IsNullOrEmpty(model.ProfilePhotoName) == false)
            {
                result.Result.ProfilePhotoName = model.ProfilePhotoName;
            }

            if (base.Update(result.Result) == 0)
            {
                result.AddError(ErrorMessages.ProfileCouldNotUpdate, "Profil güncellenemedi");
            }

            return result;
        }

        public BusinessResult<ShareYourNoteUser> RemoveUserById(int id)
        {
            BusinessResult<ShareYourNoteUser> result = new BusinessResult<ShareYourNoteUser>();
            ShareYourNoteUser user = Find(x => x.Id == id);

            if (user != null)
            {
                if (Delete(user) == 0)
                {
                    result.AddError(ErrorMessages.UserCouldNotRemove, "Kullanıcı Silinemedi..");
                    return result;
                }
            }
            else
            {
                result.AddError(ErrorMessages.UserCouldNotFind, "Kullanıcı Bulunamadı.");
            }

            return result;
        }

        public BusinessResult<ShareYourNoteUser> ActivateUser(Guid activateGuid)
        {
            BusinessResult<ShareYourNoteUser> result = new BusinessResult<ShareYourNoteUser>();
            result.Result = Find(x => x.ActivateGuid == activateGuid);

            if (result.Result != null)
            {
                if (result.Result.IsActive)
                {
                    result.AddError(ErrorMessages.UserIsAlreadyActive, "Kullanıcı zaten aktif.");
                    return result;
                }

                result.Result.IsActive = true;
                Update(result.Result);
            }
            else
            {
                result.AddError(ErrorMessages.ActiveteError, "Böyle bir Kullanıcı Bulunamadı");
            }

            return result;
        }

        public new BusinessResult<ShareYourNoteUser> Insert(ShareYourNoteUser model)
        {
            //Method gizledik.
            ShareYourNoteUser user = Find(x => x.Username == model.Username || x.Email == model.Email);
            BusinessResult<ShareYourNoteUser> result = new BusinessResult<ShareYourNoteUser>();

            result.Result = model;

            if (user != null)
            {
                if (user.Username == model.Username)
                {
                    result.AddError(ErrorMessages.UsernameAlreadyExist, "Kullanıcı adı kayıtlı.");
                }

                if (user.Email == model.Email)
                {
                    result.AddError(ErrorMessages.EmailAlreadyExist, "E-Posta Kullanımda.");
                }
            }
            else
            {
                result.Result.ProfilePhotoName = "defaultImage.png";
                result.Result.ActivateGuid = Guid.NewGuid();

                if ( base.Insert(result.Result) == 0)
                {
                    result.AddError(ErrorMessages.ProfileCouldNotUpdate, "Kullanıcı kayıt edilemedi");
                }
               
            }

            return result;
        }

        public new BusinessResult<ShareYourNoteUser> Update(ShareYourNoteUser model)
        {
            ShareYourNoteUser user = Find(x => x.Id != model.Id && (x.Username == model.Username || x.Email == model.Email));
            BusinessResult<ShareYourNoteUser> result = new BusinessResult<ShareYourNoteUser>();

            result.Result = model;

            if (user != null && user.Id != model.Id)
            {
                if (user.Username == model.Username)
                {
                    result.AddError(ErrorMessages.UsernameAlreadyExist, "Kullanıcı Adı Kullanımda");

                }
                if (user.Email == model.Email)
                {
                    result.AddError(ErrorMessages.EmailAlreadyExist, "E-Posta Adresi kullanımda");
                }
                return result;
            }

            result.Result = Find(x => x.Id == model.Id);
            result.Result.Email = model.Email;
            result.Result.Name = model.Name;
            result.Result.Surname = model.Surname;
            result.Result.Password = model.Password;
            result.Result.Username = model.Username;
            result.Result.IsActive = model.IsActive;
            result.Result.IsAdmin = model.IsAdmin;          

            if (base.Update(result.Result) == 0)
            {
                result.AddError(ErrorMessages.ProfileCouldNotUpdate, "Profil güncellenemedi");
            }

            return result;
        }
    }
}
