using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShareYourNote.Business;
using ShareYourNote.Business.Results;
using ShareYourNote.Entities;
using ShareYourNote.Entities.Messages;
using ShareYourNote.Entities.ValueObjects;
using ShareYourNote.WebUI.Filters;
using ShareYourNote.WebUI.Models;
using ShareYourNote.WebUI.ViewModels;


namespace ShareYourNote.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private NoteManager nm = new NoteManager();
        private UserManager um = new UserManager();
        // GET: Home
        public ActionResult Index()
        {
            return View(nm.ListQueryable().Where(x => x.IsApproved).OrderByDescending(x => x.UploadDate).Take(6).ToList());
        }

        public ActionResult AllNotes(string q)
        {

            if (string.IsNullOrEmpty(q) == false)
            {
                return View(nm.ListQueryable()
                    .Where(i => i.Title.Contains(q) ||
                                i.Description.Contains(q) ||
                                i.Teacher.Name.Contains(q) ||
                                i.UploadedFile.Contains(q) ||
                                i.Teacher.School.Name.Contains(q))
                    .Where(x => x.IsApproved)
                    .ToList());
            }

            return View(nm.ListQueryable().Where(x => x.IsApproved).ToList());

        }

        [Auth]
        public ActionResult ShowProfile()
        {
            BusinessResult<ShareYourNoteUser> result = um.GetUserById(SessionManager.User.Id);

            if (result.Errors.Count > 0)
            {
                //TODO: Hata ekranına yönlendir.
                ErrorViewModel notifyObject = new ErrorViewModel()
                {
                    Title = "Hata Oluştu",
                    Items = result.Errors
                };
                return View("Error", notifyObject);
            }

            return View(result.Result);
        }
        [Auth]
        public ActionResult EditProfile()
        {
            BusinessResult<ShareYourNoteUser> result = um.GetUserById(SessionManager.User.Id);

            if (result.Errors.Count > 0)
            {
                //TODO: Hata ekranına yönlendir.
                ErrorViewModel notifyObject = new ErrorViewModel()
                {
                    Title = "Hata Oluştu",
                    Items = result.Errors
                };
                return View("Error", notifyObject);
            }

            return View(result.Result);
        }
        [Auth]
        [HttpPost]
        public ActionResult EditProfile(ShareYourNoteUser model, HttpPostedFileBase ProfileImage)
        {
            if (ModelState.IsValid)
            {
                if (ProfileImage != null &&
                    (
                        ProfileImage.ContentType == "image/jpeg" ||
                        ProfileImage.ContentType == "image/jpg" ||
                        ProfileImage.ContentType == "image/png"
                    ))
                {
                    string fileName = $"user_{model.Id}.{ProfileImage.ContentType.Split('/')[1]}";
                    ProfileImage.SaveAs(Server.MapPath($"~/UploadedFiles/ProfileImages/{fileName}"));
                    model.ProfilePhotoName = fileName;
                }

                BusinessResult<ShareYourNoteUser> result = um.UpdateProfile(model);

                if (result.Errors.Count > 0)
                {
                    ErrorViewModel notifyObject = new ErrorViewModel()
                    {
                        Title = "Profil güncellenemedi..",
                        Items = result.Errors,
                        RedirectingUrl = "/Home/EditProfile"
                    };
                    return View("Error", notifyObject);
                }

                SessionManager.Set<ShareYourNoteUser>("login", result.Result);

                return RedirectToAction("ShowProfile");
            }

            return View(model);
        }
        [Auth]
        public ActionResult RemoveProfile()
        {
            BusinessResult<ShareYourNoteUser> result = um.RemoveUserById(SessionManager.User.Id);

            if (result.Errors.Count > 0)
            {
                ErrorViewModel messages = new ErrorViewModel()
                {
                    Items = result.Errors,
                    Title = "Profil Silinemedi",
                    RedirectingUrl = "/Home/ShowProfile"
                };
                return View("Error", messages);
            }
            Session.Clear();
            return RedirectToAction("Index");
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
                BusinessResult<ShareYourNoteUser> result = um.RegisterNewUser(model);

                if (result.Errors.Count > 0)
                {
                    result.Errors.ForEach(x => ModelState.AddModelError("", x.Messages));
                    return View(model);
                }

                OkViewModel notifyObject = new OkViewModel()
                {
                    Title = "Kayıt Başarılı",
                    RedirectingUrl = "/Home/Login"
                };
                notifyObject.Items.Add("Lütfen E-Posta adresinize gönderilen aktivasyon linkini kontrol ediniz.");
                return View("Ok", notifyObject);
            }

            return View(model);

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
                BusinessResult<ShareYourNoteUser> result = um.LoginUser(model);

                if (result.Errors.Count > 0)
                {
                    result.Errors.ForEach(x => ModelState.AddModelError("", x.Messages));
                    return View(model);
                }

                SessionManager.Set<ShareYourNoteUser>("login", result.Result);
                return RedirectToAction("Index");
            }

            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }

        public ActionResult ActivateUser(Guid id)
        {
            BusinessResult<ShareYourNoteUser> user = um.ActivateUser(id);

            if (user.Errors.Count > 0)
            {
                ErrorViewModel notifyObject = new ErrorViewModel()
                {
                    Title = "Aktivasyon sırasında hata meydana geldi.",
                    Items = user.Errors
                };
                return View("Error", notifyObject);
            }

            OkViewModel notifyObj = new OkViewModel()
            {
                Title = "Kayıt aktif edildi",
                RedirectingUrl = "/Home/Login"
            };
            notifyObj.Items.Add("Aktivasyon işlemi başarı ile gerçekleşti.");
            return View("Ok", notifyObj);
        }

        public ActionResult PopularNotes()
        {
            return View("Index", nm.ListQueryable().Where(x => x.IsApproved).OrderByDescending(x => x.Comments.Count>2).Take(10).ToList());
        }

        public ActionResult AccessDenied()
        {
            return View();
        }

    }
}