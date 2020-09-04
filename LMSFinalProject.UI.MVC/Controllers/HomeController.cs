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

            //WORKS!!
            var orderLesson = db.LessonViews.Where(ol => ol.DateViewed != null);

            var orderCourse = db.CourseCompletions.Where(oc => oc.DateCompleted == null);

            //When employee has finished 6 courses, they are added to Annual Training Complete List
            var annualDone = db.CourseCompletions.Where(ad => ad.CourseCompletionId >= 6);

            ViewBag.OrderLesson = orderLesson;
            ViewBag.OrderCourse = orderCourse;
            ViewBag.AnnualDone = annualDone;

            return View();
        }





    }
}
