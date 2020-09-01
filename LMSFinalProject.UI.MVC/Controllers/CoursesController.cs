using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LMSFinalProject.DATA.EF;

namespace LMSFinalProject.UI.MVC.Controllers
{
    public class CoursesController : Controller
    {
        private LMSFinalEntities1 db = new LMSFinalEntities1();

        #region AJAX Delete
        [HttpPost]
        public JsonResult AjaxDelete(int id)
        {
            Course cour = db.Courses.Find(id);

            db.Courses.Remove(cour);

            db.SaveChanges();

            var message = $"Deleted Course {cour.CourseName} from the database! Refresh to remove";
            return Json(
                new
                {
                    id = id,
                    message = message
                });

        }
        #endregion

        #region AJAX Details
        [HttpGet]
        public PartialViewResult CourseDetails(int id)
        {
            Course cour = db.Courses.Find(id);

            return PartialView(cour);

        }
        #endregion

        #region AJAX Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult CourseCreate(Course course)
        {
            db.Courses.Add(course);
            db.SaveChanges();
            return Json(course);
        }
        #endregion

        #region AJAX Edit
        public PartialViewResult CourseEdit(int id)
        {
            Course cour = db.Courses.Find(id);
            return PartialView(cour);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AjaxEdit(Course course)
        {
            db.Entry(course).State = EntityState.Modified;
            db.SaveChanges();
            return Json(course);
        }

        #endregion



        // GET: Courses
        public ActionResult Index()
        {
            return View(db.Courses.ToList());
        }

        // GET: Courses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // GET: Courses/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CourseId,CourseName,CourseDescription,IsActive")] Course course)
        {
            if (ModelState.IsValid)
            {
                db.Courses.Add(course);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(course);
        }

        // GET: Courses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CourseId,CourseName,CourseDescription,IsActive")] Course course)
        {
            if (ModelState.IsValid)
            {
                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(course);
        }

        // GET: Courses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Course course = db.Courses.Find(id);
            db.Courses.Remove(course);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
