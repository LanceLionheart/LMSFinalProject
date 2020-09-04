using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LMSFinalProject.DATA.EF;

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

            var orderLesson = db.CourseCompletions.OrderBy(ol => ol.UserId);

            var orderCourse = db.LessonViews.OrderBy(oc => oc.UserId);

            ViewBag.OrderLesson = orderLesson;
            ViewBag.OrderCourse = orderCourse;

            return View();
        }





    }
}
