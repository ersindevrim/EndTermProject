using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShareYourNote.Business;
using ShareYourNote.Business.Results;
using ShareYourNote.Entities;
using ShareYourNote.WebUI.Filters;

namespace ShareYourNote.WebUI.Controllers
{
    [Auth]
    [AuthAdmin]
    public class UserController : Controller
    {
        private UserManager _userManager = new UserManager();

        public ActionResult Index()
        {
            return View(_userManager.List());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShareYourNoteUser shareYourNoteUser = _userManager.Find(x => x.Id == id);
            if (shareYourNoteUser == null)
            {
                return HttpNotFound();
            }
            return View(shareYourNoteUser);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ShareYourNoteUser shareYourNoteUser)
        {
            if (ModelState.IsValid)
            {
                BusinessResult<ShareYourNoteUser> result = _userManager.Insert(shareYourNoteUser);
                //TODO : Düzenle 
                if (result.Errors.Count > 0)
                {
                    result.Errors.ForEach(x => ModelState.AddModelError("", x.Messages));
                    return View(shareYourNoteUser);
                }
                return RedirectToAction("Index");
            }

            return View(shareYourNoteUser);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShareYourNoteUser shareYourNoteUser = _userManager.Find(x => x.Id == id);
            if (shareYourNoteUser == null)
            {
                return HttpNotFound();
            }
            return View(shareYourNoteUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ShareYourNoteUser shareYourNoteUser)
        {
            BusinessResult<ShareYourNoteUser> result = _userManager.Update(shareYourNoteUser);
            //TODO : Düzenle 
            if (result.Errors.Count > 0)
            {
                result.Errors.ForEach(x => ModelState.AddModelError("", x.Messages));
                return View(shareYourNoteUser);
            }
            return View(shareYourNoteUser);
        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShareYourNoteUser shareYourNoteUser = _userManager.Find(x => x.Id == id);
            if (shareYourNoteUser == null)
            {
                return HttpNotFound();
            }
            return View(shareYourNoteUser);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ShareYourNoteUser shareYourNoteUser = _userManager.Find(x => x.Id == id);
            _userManager.Delete(shareYourNoteUser);

            return RedirectToAction("Index");
        }


    }
}
