using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LMSFinalProject.DATA.EF;
using System.Net.Mail;
using LMSFinalProject.UI.MVC.Models; //Added For Contact

using Microsoft.AspNet.Identity; //Added these two
using Microsoft.AspNet.Identity.Owin;

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

        //****************LESSON COMPLETE DOMINO EFFECT*******************
        public ActionResult LessonComplete(int id)  //Added ? back
                                                    //called line 80 in LessonIndex
        {

            LMSFinalEntities1 db = new LMSFinalEntities1();

            Lesson lesson = db.Lessons.Find(id);

            string userID = User.Identity.GetUserId();

            if (User.IsInRole("Employee")) //If user is logged in as "Employee", Lesson/Course clicks saved
            {//START OF BIG FUNCTION

                var viewedlessons = db.LessonViews.Where(l => l.UserId == userID);
                bool hasviewed = true;

                foreach (var Lessonitem in viewedlessons)
                {
                    if (id == Lessonitem.LessonId)
                    {
                        hasviewed = false;
                    }
                    else
                    {
                        hasviewed = true;
                    }
                }
                if (hasviewed == true)
                {
                    LessonView lv = new LessonView();
                    lv.DateViewed = DateTime.Now;
                    lv.UserId = userID;
                    //lv.LessonViewId
                    lv.LessonId = lesson.LessonId;

                    db.LessonViews.Add(lv);
                    db.SaveChanges();
                }




                //**************CourseCompletion*******************
                //Courses viewed is not saved, only completed courses



                var coursecompleted = db.CourseCompletions.Where(cou => cou.UserId == userID);

                //bool for checking if all lessons done
                bool coursecomplete = true;

                //variable to find total number of lessons related to specific course
                var totallesson = db.Lessons.Count(cl => cl.CourseId == cl.LessonId);

                //variable to grab each LessonID related to course
                var courselesson = db.Lessons.Where(cl => cl.CourseId == lesson.CourseId).Select(l => l.LessonId);

                //variable that keeps count of employee-viewed lessons // If this equals totallesson, Create CC
                var employeeviewedlessons = 0;

                foreach (var lessonview in db.LessonViews.Where(l => l.UserId == userID))
                {
                    foreach (var lessonitem in courselesson)
                    {
                        if (lessonview.LessonId == lessonitem)

                            employeeviewedlessons++;
                    }
                }


                foreach (var comp in coursecompleted)
                {
                    if (comp.CourseId == lesson.CourseId)
                        coursecomplete = true;
                }

                if (employeeviewedlessons == totallesson && coursecomplete == true)
                {
                    CourseCompletion cc = new CourseCompletion();
                    cc.DateCompleted = DateTime.Now;
                    cc.UserId = userID;
                    //cc.CourseCompletionId = id;
                    cc.CourseId = lesson.CourseId;
                    db.CourseCompletions.Add(cc);
                    db.SaveChanges();

                }

                //var fname = db.UserDetails.Where(f => f.FirstName == userID);
                //foreach (var finished in db.CourseCompletions.Where(oo => oo.CourseCompletionId == 6))
                //Make sure this entire function is saving under one person)
                //Courses Done, Now Email Manager
                //bool alldone = true;
                //When UserIds in CourseCompletion match current logged in userId
                //var trainingDone = db.CourseCompletions.Where(oo => oo.UserId == userID);

                //This works. When the UserIds in CourseCompletion that match current logged in userId = 6, email annual.
                var trainingsDone = db.CourseCompletions.Count(oo => oo.UserId == userID);

               
                if (trainingsDone == 6) //was if (trainingDone != null). Emailed everytime (might be good for single course)
                {

                    MailMessage msg = new MailMessage(
                    "admin@lancevogel.com",
                    "lzvogel@outlook.com",
                     "Employee:" + userID + "completed training",
                     "Employee:" + userID + "has completed their Annual training.");

                    msg.IsBodyHtml = true;
                    msg.Priority = MailPriority.High;

                    //msg.ReplyToList.Add(cvm.Email);
                    //msg.CC.Add("metalsquidlance@gmail.com");

                    SmtpClient client = new SmtpClient("mail.lancevogel.com");
                    client.Credentials = new NetworkCredential("admin@lancevogel.com", "Turtle333!");

                    client.Port = 8889;

                    try
                    {
                        client.Send(msg);
                    }
                    catch (System.Exception ex)
                    {
                        ViewBag.ErrorMessage = "There was a problem . . . </br>" + ex.StackTrace;

                        return View("Index");
                    }

                    //return View("Courses");
                    /* return View("EmailConfirmation", "Courses");*/ // Index might need to be Courses. Send the user to a email conformation view
                }
                return RedirectToAction("Index");

                //changed from    return RedirectToAction("Courses");

            }//END OF BIG FUNCTION
            return View();
        }




        //public ActionResult YouTubeUrl()
        //{
        //    Lesson lv = new Lesson();

        //    var CompleteYouTubeURL = lv.VideoURL;

        //    var v = CompleteYouTubeURL.IndexOf("v=");
        //    var amp = CompleteYouTubeURL.IndexOf("&", v);
        //    string vid;
        //    // if the video id is the last value in the url
        //    if (amp == -1)
        //    {
        //        vid = CompleteYouTubeURL.Substring(v + 2);
        //        // if there are other parameters after the video id in the url
        //    }
        //    else
        //    {
        //        vid = CompleteYouTubeURL.Substring(v + 2, amp - (v + 2));
        //    }
        //    ViewBag.VideoID = vid;

        //    return View();
        //}


    }
}
