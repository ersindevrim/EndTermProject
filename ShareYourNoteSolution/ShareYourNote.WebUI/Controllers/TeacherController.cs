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

namespace ShareYourNote.WebUI.Controllers
{
    [Auth]
    [AuthAdmin]
    public class TeacherController : Controller
    {
        private TeacherManager _teacherManager = new TeacherManager();
        private SchoolManager _schoolManager = new SchoolManager();

        public ActionResult Index()
        {
            return View(_teacherManager.List());
        }

        public ActionResult Create()
        {
            ViewBag.SchoolId = new SelectList(_schoolManager.List(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                _teacherManager.Insert(teacher);
                return RedirectToAction("Index");
            }

            return View(teacher);
        }

        public ActionResult Edit(int? id)
        {
            ViewBag.SchoolId = new SelectList(_schoolManager.List(), "Id", "Name");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Teacher teacher = _teacherManager.Find(x => x.Id == id.Value);
            if (teacher == null)
            {
                return HttpNotFound();
            }

            return View(teacher);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                //TODO : Burayı düzenle - Done

                Teacher _teacher = _teacherManager.Find(x => x.Id == teacher.Id);

                _teacher.Name = teacher.Name;
                _teacher.SchoolId = teacher.SchoolId;
                _teacher.IsApproved = teacher.IsApproved;

                _teacherManager.Update(_teacher);

                return RedirectToAction("Index");
            }
            return View(teacher);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teacher teacher = _teacherManager.Find(x => x.Id == id.Value);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            return View(teacher);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Teacher teacher = _teacherManager.Find(x => x.Id == id);

            _teacherManager.Delete(teacher);

            return RedirectToAction("Index");
        }
    }
}
