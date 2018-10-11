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
    public class SchoolController : Controller
    {
        private SchoolManager _schoolManager = new SchoolManager();

        public ActionResult Index()
        {
            return View(_schoolManager.List());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(School school)
        {
            if (ModelState.IsValid)
            {
                _schoolManager.Insert(school);
                return RedirectToAction("Index");
            }

            return View(school);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            School school = _schoolManager.Find(x => x.Id == id.Value);
            if (school == null)
            {
                return HttpNotFound();
            }
            return View(school);
        }

        // POST: School/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(School school)
        {
            if (ModelState.IsValid)
            {
                School findschool = _schoolManager.Find(x => x.Id == school.Id);
                findschool.Name = school.Name;
                findschool.IsApproved = school.IsApproved;

                return RedirectToAction("Index");
            }
            return View(school);
        }

        // GET: School/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            School school = _schoolManager.Find(x => x.Id == id.Value);
            if (school == null)
            {
                return HttpNotFound();
            }
            return View(school);
        }

        // POST: School/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            School school = _schoolManager.Find(x => x.Id == id);
            _schoolManager.Delete(school);
            return RedirectToAction("Index");
        }

    }
}
