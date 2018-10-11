using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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
    public class NoteController : Controller
    {
        private NoteManager _noteManager = new NoteManager();
        private TeacherManager _teacherManager = new TeacherManager();        
        [Auth]
        public ActionResult Index()
        {
            var notes = _noteManager.ListQueryable().Include("Teacher")
                .Where(
                    x => x.OwnerId == SessionManager.User.Id);
            return View(notes.ToList());
        }

        public ActionResult Details(int? id)
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
            return View(note);
        }
        [Auth]
        public ActionResult Create()
        {
            ViewBag.TeacherId = new SelectList(_teacherManager.List(), "Id", "Name");
            return View();
        }
        [Auth]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Note note, HttpPostedFileBase attechedFile)
        {
            ModelState.Remove("UploadedFile");
            if (ModelState.IsValid)
            {               
                if (attechedFile != null && attechedFile.ContentType == "application/pdf")
                {
                    string fileName = $"note_{note.Id}.{attechedFile.ContentType.Split('/')[1]}";
                    attechedFile.SaveAs(Server.MapPath($"~/UploadedFiles/UserNotes/{fileName}"));
                    note.UploadedFile = fileName;
                }

                note.UploadDate = DateTime.Now;
                note.OwnerId = SessionManager.User.Id;
                _noteManager.Insert(note);
                return RedirectToAction("Index");
            }

            ViewBag.TeacherId = new SelectList(_teacherManager.List(), "Id", "Name", note.TeacherId);
            return View(note);
        }
        [Auth]
        public ActionResult Edit(int? id)
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
            ViewBag.TeacherId = new SelectList(_teacherManager.List(), "Id", "Name", note.TeacherId);
            return View(note);
        }
        [Auth]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Note note)
        {
            if (ModelState.IsValid)
            {
                //TODO: BURAYI DÜZENLE
                Note dbNote = _noteManager.Find(x => x.Id == note.Id);

                dbNote.IsApproved = dbNote.IsApproved;
                dbNote.TeacherId = note.TeacherId;
                dbNote.Description = note.Description;
                dbNote.Title = note.Title;
                dbNote.UploadedFile = note.UploadedFile;
                dbNote.UploadDate = DateTime.Now;
                dbNote.OwnerId = SessionManager.User.Id;

                _noteManager.Update(dbNote);

                return RedirectToAction("Index");
            }
            ViewBag.TeacherId = new SelectList(_teacherManager.List(), "Id", "Name", note.TeacherId);
            return View(note);
        }
        [Auth]
        public ActionResult Delete(int? id)
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
            return View(note);
        }
        [Auth]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Note note = _noteManager.Find(x => x.Id == id);
            _noteManager.Delete(note);
            return RedirectToAction("Index");
        }

        [Auth]
        [AuthAdmin]
        public ActionResult AdminNotes()
        {
            var notes = _noteManager.ListQueryable().Include("Teacher");
            return View(notes.ToList());
        }
        [Auth]
        [AuthAdmin]
        public ActionResult AdminEdit(int? id)
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
            ViewBag.TeacherId = new SelectList(_teacherManager.List(), "Id", "Name", note.TeacherId);
            return View(note);
        }
        [Auth]
        [AuthAdmin]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdminEdit(Note note)
        {
            if (ModelState.IsValid)
            {
                //TODO: BURAYI DÜZENLE
                Note dbNote = _noteManager.Find(x => x.Id == note.Id);

                dbNote.IsApproved = note.IsApproved;
                dbNote.TeacherId = note.TeacherId;
                dbNote.Description = note.Description;
                dbNote.Title = note.Title;
                dbNote.UploadedFile = note.UploadedFile;
                dbNote.UploadDate = DateTime.Now;
                dbNote.OwnerId = SessionManager.User.Id;

                _noteManager.Update(dbNote);

                return RedirectToAction("Index");
            }
            ViewBag.TeacherId = new SelectList(_teacherManager.List(), "Id", "Name", note.TeacherId);
            return View(note);
        }
    }
}
