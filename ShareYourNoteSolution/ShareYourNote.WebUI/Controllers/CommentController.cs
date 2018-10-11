using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShareYourNote.Business;
using ShareYourNote.Entities;
using ShareYourNote.WebUI.Filters;
using ShareYourNote.WebUI.Models;

namespace ShareYourNote.WebUI.Controllers
{    
    public class CommentController : Controller
    {
        private NoteManager _noteManager = new NoteManager();
        private CommentManager _commentManager = new CommentManager();
        public ActionResult ShowNoteComments(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Note note = _noteManager.Find(x => x.Id == id);

            if (note == null)
            {
                return HttpNotFound();
            }

            return PartialView("_PartialComment", note.Comments);
        }

        [Auth]
        [HttpPost]
        public ActionResult Edit(int? id, string text)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Comment comment = _commentManager.Find(x => x.Id == id);

            if (comment == null)
            {
                return new HttpNotFoundResult();
            }

            comment.Text = text;
            if (_commentManager.Update(comment) > 0)
            {
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = false }, JsonRequestBehavior.AllowGet);
        }
        [Auth]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Comment comment = _commentManager.Find(x => x.Id == id);

            if (comment == null)
            {
                return new HttpNotFoundResult();
            }

            if (_commentManager.Delete(comment) > 0)
            {
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = false }, JsonRequestBehavior.AllowGet);
        }
        [Auth]
        [HttpPost]
        public ActionResult Create(Comment comment, int? noteid)
        {
            if (ModelState.IsValid)
            {
                if (noteid == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                Note note = _noteManager.Find(x => x.Id == noteid);

                if (note == null)
                {
                    return new HttpNotFoundResult();
                }

                comment.Note = note;
                comment.Owner = SessionManager.User;

                if (_commentManager.Insert(comment) > 0)
                {
                    return Json(new { result = true }, JsonRequestBehavior.AllowGet);
                }

            }
            return Json(new { result = false }, JsonRequestBehavior.AllowGet);
        }
    }
}