using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LMSFinalProject.DATA.EF;
using System.Net.Mail;
using System.Net;
using LMSFinalProject.Models;
using Microsoft.AspNet.Identity; //Added these two
using Microsoft.AspNet.Identity.Owin;
using System.Data;
using System.Data.Entity;

namespace LMSFinalProject.UI.MVC.Controllers
{
    public class HomeController : Controller
    {

        LMSFinalEntities1 db = new LMSFinalEntities1();


        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        //[Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        [HttpGet]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        public ActionResult UntypedView()
        {
            //Linq     Order Information on LessonsViewed and CoursesCompleted

            //WORKS!!
            var orderLesson = db.LessonViews.Where(ol => ol.DateViewed != null);

            var orderCourse = db.CourseCompletions.Where(oc => oc.DateCompleted != null);

            //When employee has finished 6 courses, they are added to Annual Training Complete List
            var annualDone = db.CourseCompletions.Where(ad => ad.CourseCompletionId >= 6);

            ViewBag.OrderLesson = orderLesson;
            ViewBag.OrderCourse = orderCourse;
            ViewBag.AnnualDone = annualDone;

            return View();
        }


        //Employee Course Completed View
        public ActionResult UntypedViewEmployee(int? id)
        {

            LMSFinalEntities1 db = new LMSFinalEntities1();

            Lesson lesson = db.Lessons.Find(id);

            string userID = User.Identity.GetUserId();

            

            //Courses completed by employees (Check mark next to them)
            var empCourse = db.CourseCompletions.Where(oc => oc.UserId == userID);


            //variable to relate courses not complete (Experimenting)
            //var alldone = db.LessonViews.Where(all => all.UserDetail.UserId == userID);

            //All Active Courses 
            var empnoCourse = db.Courses.Where(en => en.CourseId != null && en.IsActive == true);


            //var empnoCourse = db.CourseCompletions.Where(en => en.UserId != en.UserDetail.UserId);

            ViewBag.EmpCourse = empCourse;
            ViewBag.EmpNoCourse = empnoCourse;


            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]  //Error message creator
        public ActionResult Contact(ContactViewModel usersContactRequest)
        {
            if (!ModelState.IsValid)
            {
                return View(usersContactRequest);
            }

            //The MailMessage Object takes several parameters has 3 overloads 
            //This instance in particular takes 4 parameters:
            //from: admin@yourdomain.com
            //to: lzvogel@outlook.com
            //subject: usersContactRequest.Subject (ContactViewModel passed into the POST Contact route (above))
            //message: usersContactRequest.Message (ContactViewModel passed into the POST Contact route (above))
            MailMessage msg = new MailMessage(
                "admin@lancevogel.com",
                "lzvogel@outlook.com",
                usersContactRequest.Subject,
                usersContactRequest.Message);

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

                return View(usersContactRequest);
            }

            return View("EmailConfirmation", usersContactRequest); // send the user to a email conformation view
        }


        //public ActionResult UnpublishedCourses()
        //{


        //    return View();
        //}


        }
}
