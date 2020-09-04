using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LMSFinalProject.DATA.EF;

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
        public ActionResult LessonComplete(int? id)  //called line 80 in LessonIndex
        {         
            if (Request.IsAuthenticated)
            {
                LMSFinalEntities1 db = new LMSFinalEntities1();

                string userID = User.Identity.GetUserId();

                if (User.IsInRole("Employee")) //If user is logged in as "Employee", Lesson/Course clicks saved
                {//START OF BIG FUNCTION

                    var viewedlessons = db.LessonViews.Where(l => l.UserId == userID);
                    bool hasviewed = true;

                    foreach (var Lessonitem in viewedlessons)
                    {
                          if (id == Lessonitem.LessonId)
                             hasviewed = true;
                    }
                    if (hasviewed==false)
                    {
                        LessonView lv = new LessonView();
                        lv.DateViewed = DateTime.Now;
                        lv.UserId = userID;
                        //lv.LessonViewId
                        //lv.Lessonid

                       db.LessonViews.Add(lv);
                        db.SaveChanges(); 
                    }

                    //**************CourseCompletion*******************
                    //Courses viewed is not saved, only completed courses

                    var coursecompleted = db.Courses.Where(cou => cou.Lessons == viewedlessons);//grabbed from ^

                    bool lessonsalldone = true; //is course completed of all lessons?

                    foreach (var courseitem in coursecompleted)
                    {
                        if (id == courseitem.CourseId)
                            lessonsalldone = false; //course is not yet completed
                    }

                    if (lessonsalldone == true) //if course is completed, add new CourseCompletion
                    {
                        CourseCompletion cc= new CourseCompletion();
                        cc.DateCompleted = DateTime.Now;
                        cc.UserId = userID;
                        //lv.LessonViewId
                        //lv.Lessonid

                        db.CourseCompletions.Add(cc);
                        db.SaveChanges();
                    }


                    //Make sure this entire function is saving under one person)
                    //Courses Done, Now Email Manager
                    var trainingDone = db.CourseCompletions.Where(oo => oo.CourseId == 6);

                    //bool alldone = true;
                    //if (trainingDone <= 6)
                    {
                        //Email
                //        MailMessage msg = new MailMessage(
                //        "admin@lancevogel.com",
                //"lzvogel@outlook.com",
                //usersContactRequest.Subject,
                //usersContactRequest.Message);

                //        msg.IsBodyHtml = true;
                //        msg.Priority = MailPriority.High;

                //        //msg.ReplyToList.Add(cvm.Email);
                //        //msg.CC.Add("metalsquidlance@gmail.com");

                //        SmtpClient client = new SmtpClient("mail.lancevogel.com");
                //        client.Credentials = new NetworkCredential("admin@lancevogel.com", "T3s!");

                //        client.Port = 8889;

                //        try
                //        {
                //            client.Send(msg);
                //        }
                //        catch (System.Exception ex)
                //        {
                //            ViewBag.ErrorMessage = "There was a problem . . . </br>" + ex.StackTrace;

                //            return View(usersContactRequest);
                //        }

                //        return View("EmailConfirmation", usersContactRequest); // send the user to a email conformation view
                    }

                    return RedirectToAction("Course");

                }//END OF BIG FUNCTION





                //UserDetail currentUser = db.UserDetails.Where(ud => ud.UserId == userID).FirstOrDefault();

                //string userFN = "Guest";

                //if (currentUser != null)
                //{
                //    userFN = currentUser.FirstName;
                //}

            }

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
