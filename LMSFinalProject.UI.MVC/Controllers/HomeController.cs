using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LMSFinalProject.DATA.EF;
using System.Net.Mail;
using System.Net;
using LMSFinalProject.Models;
using Microsoft.AspNet.Identity;
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

        //Employee Status Page (UntypedView)
        public ActionResult UntypedView()
        {
            //Order LessonViews (Completed Lessons) by User Last Name
            var orderLesson = db.LessonViews.Where(ol => ol.DateViewed != null).OrderBy(ol => ol.UserDetail.LastName);

            //Order CourseCompletions (Completed Courses) by User Last Name
            var orderCourse = db.CourseCompletions.Where(oc => oc.DateCompleted != null).OrderBy(oc => oc.UserDetail.LastName);

            ViewBag.OrderLesson = orderLesson;
            ViewBag.OrderCourse = orderCourse;


            return View();
        }

        //My Progress Page (UntypedViewEmployee)
        public ActionResult UntypedViewEmployee(int? id)
        {

            LMSFinalEntities1 db = new LMSFinalEntities1();

            Lesson lesson = db.Lessons.Find(id);

            string userID = User.Identity.GetUserId();

            //Courses completed by currently logged in user
            var empCourse = db.CourseCompletions.Where(oc => oc.UserId == userID);

            //Lessons completed by currently logged in user
            var empLess = db.LessonViews.Where(oc => oc.UserId == userID);


            //All Active Courses so Employees can keep track of what still needs to be completed
            var empnoCourse = db.Courses.Where(en => en.CourseId != null && en.IsActive == true);


            ViewBag.EmpLess = empLess;
            ViewBag.EmpCourse = empCourse;
            ViewBag.EmpNoCourse = empnoCourse;


            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact(ContactViewModel usersContactRequest)
        {
            if (!ModelState.IsValid)
            {
                return View(usersContactRequest);
            }

            MailMessage msg = new MailMessage(
                "admin@lancevogel.com",
                "lzvogel@outlook.com",
                usersContactRequest.Subject,
                usersContactRequest.Message);

            msg.IsBodyHtml = true;
            msg.Priority = MailPriority.High;


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

            return View("EmailConfirmation", usersContactRequest);
        }

    }
}
