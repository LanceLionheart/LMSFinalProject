using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Migrations;//Added
using LMSFinalProject.DATA.EF;
using LMSFinalProject.UI.MVC.Models;//added

namespace LMSFinalProject.UI.MVC.Controllers
{
    public class LessonsController : Controller
    {
        private LMSFinalEntities1 db = new LMSFinalEntities1();

        #region AJAX Delete
        [HttpPost]
        public JsonResult AjaxDelete(int id)
        {
            Lesson less = db.Lessons.Find(id);

            db.Lessons.Remove(less);

            db.SaveChanges();

            var message = $"Deleted Lesson {less.LessonTitle} from the database!";
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
        public PartialViewResult LessonDetails(int id)
        {
            Lesson less = db.Lessons.Find(id);
            //Courses course = db.Courses.Find(id)
            //Grab all lessons from that course and add it to a list. Send that list into the view. 
         

            return PartialView(less);

        }
        #endregion

        #region AJAX Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult LessonCreate(Lesson lesson, HttpPostedFileBase pdfUpload) //Do I need binds here?
        {
            {
                if (ModelState.IsValid)
                {
                    #region Pdf Upload (Create)

                    string pdfName = "dummypdf.pdf";

                    if (pdfUpload != null)
                    {
                        pdfName = pdfUpload.FileName;

                        string ext = pdfName.Substring(pdfName.LastIndexOf("."));

                        string[] goodExts = new string[] { ".pdf" };

                        if (goodExts.Contains(ext.ToLower()))
                        {
                            pdfName = Guid.NewGuid() + ext;

                            pdfUpload.SaveAs(Server.MapPath("~/App_Data/PDF" + pdfName));
                        }
                        else
                        {
                            pdfName = "dummypdf.pdf";
                        }

                    }
                    lesson.PdfFilename = pdfName;
                    #endregion


                    db.Lessons.Add(lesson);
                    db.SaveChanges();

                }
                #endregion
                return Json(lesson);
            }
        }

        #region AJAX Edit
        public PartialViewResult LessonEdit(int id)
        {
            Lesson less = db.Lessons.Find(id);
            return PartialView(less);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AjaxEdit(Lesson lesson, HttpPostedFileBase pdfUpload)
        {
            if (ModelState.IsValid)
            {
                #region Pdf Upload (Edit)
                if (pdfUpload != null)
                {
                    string pdfName = "dummypdf.pdf";
                    pdfName = pdfUpload.FileName;
                    string ext = pdfName.Substring(pdfName.LastIndexOf("."));
                    string[] goodExts = new string[] { ".pdf" };
                    if (goodExts.Contains(ext.ToLower()))
                    {

                        pdfName = Guid.NewGuid() + ext;
                        pdfUpload.SaveAs(Server.MapPath("~/" + pdfName));

                        if (lesson.PdfFilename != null && lesson.PdfFilename != "dummypdf.pdf")
                        {
                            System.IO.File.Delete(Server.MapPath("~/" + Session["currentPdf"].ToString()));
                        }

                        lesson.PdfFilename = pdfName;
                    }


                }
                else
                {
                    var pdfs = db.Lessons.ToList();
                    var test = from l in pdfs
                               where l.LessonId == lesson.LessonId
                               select l.PdfFilename;

                    lesson.PdfFilename = test.FirstOrDefault().ToString();

                }

                db.Set<Lesson>().AddOrUpdate(lesson);
                db.SaveChanges();

                #endregion
                //    db.Entry(lesson).State = EntityState.Modified;
                //db.SaveChanges();
                
            }
            return Json(lesson);
        }

        #endregion

        #region AJAX Index
        [HttpGet]
        public PartialViewResult LessonIndex(int id)
        {
            //Lesson less = db.Lessons.Find(id);
            Course course = db.Courses.Find(id);

            //Courses course = db.Courses.Find(id)
            //Grab all lessons from that course and add it to a list. Send that list into the view.
            Lesson lv = db.Lessons.Find(course.Lessons.FirstOrDefault().LessonId);

            var lessons = db.Lessons.Where(l => l.Course.CourseId == course.CourseId);

            return PartialView(lessons.ToList());

        }
        #endregion


        // GET: Lessons
        public ActionResult Index()
        {
            var lessons = db.Lessons.Include(l => l.Course);
            return View(lessons.ToList());
        }

        // GET: Lessons/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lesson lesson = db.Lessons.Find(id);
            if (lesson == null)
            {
                return HttpNotFound();
            }
            return View(lesson);
        }

        // GET: Lessons/Create
        public ActionResult Create()
        {
            ViewBag.CourseId = new SelectList(db.Lessons, "LessonId", "LessonName");
            return View();
        }

        // POST: Lessons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LessonId,LessonTitle,CourseId,Introduction,VideoURL,PdfFilename,IsActive")] Lesson lesson, HttpPostedFileBase pdfUpload)
        {
            if (ModelState.IsValid)
            {
                #region Pdf Upload (Create)

                string pdfName = "dummypdf.pdf";

                if (pdfUpload != null)
                {
                    pdfName = pdfUpload.FileName;

                    string ext = pdfName.Substring(pdfName.LastIndexOf("."));

                    string[] goodExts = new string[] { ".pdf" };

                    if (goodExts.Contains(ext.ToLower()))
                    {
                        pdfName = Guid.NewGuid() + ext;

                        pdfUpload.SaveAs(Server.MapPath("~/" + pdfName));
                    }
                    else
                    {
                        pdfName = "dummypdf.pdf";
                    }
                   
                }
                lesson.PdfFilename = pdfName;
                #endregion

                db.Lessons.Add(lesson);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CourseId = new SelectList(db.Lessons, "LessonId", "LessonName", lesson.LessonId);
            return View(lesson);
        }

        // GET: Lessons/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lesson lesson = db.Lessons.Find(id);
            if (lesson == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseId = new SelectList(db.Lessons, "LessonId", "LessonName", lesson.LessonId);
            return View(lesson);
        }

        // POST: Lessons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LessonId,LessonTitle,CourseId,Introduction,VideoURL,PdfFilename,IsActive")] Lesson lesson, HttpPostedFileBase pdfUpload)
        {
            if (ModelState.IsValid)
            {
                #region Pdf Upload (Edit)
                if (pdfUpload != null)
                {
                    string pdfName = "dummypdf.pdf";
                    pdfName = pdfUpload.FileName;
                    string ext = pdfName.Substring(pdfName.LastIndexOf("."));
                    string[] goodExts = new string[] { ".pdf" };
                    if (goodExts.Contains(ext.ToLower()))
                    {

                        pdfName = Guid.NewGuid() + ext;
                        pdfUpload.SaveAs(Server.MapPath("~/" + pdfName));

                        if (lesson.PdfFilename != null && lesson.PdfFilename != "dummypdf.pdf")
                        {
                            System.IO.File.Delete(Server.MapPath("~/" + Session["currentPdf"].ToString()));
                        }

                        lesson.PdfFilename = pdfName;
                    }


                }
                else
                {
                    var pdfs = db.Lessons.ToList();
                    var test = from l in pdfs
                               where l.LessonId == lesson.LessonId
                               select l.PdfFilename;

                    lesson.PdfFilename = test.FirstOrDefault().ToString();
                    
                }

                db.Set<Lesson>().AddOrUpdate(lesson);
                db.SaveChanges();

                #endregion

                //db.Entry(lesson).State = EntityState.Modified;
                //db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CourseId = new SelectList(db.Lessons, "LessonId", "LessonName", lesson.LessonId);
            return View(lesson);
        }

        // GET: Lessons/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lesson lesson = db.Lessons.Find(id);
            if (lesson == null)
            {
                return HttpNotFound();
            }
            return View(lesson);
        }

        // POST: Lessons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Lesson lesson = db.Lessons.Find(id);
            db.Lessons.Remove(lesson);
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
