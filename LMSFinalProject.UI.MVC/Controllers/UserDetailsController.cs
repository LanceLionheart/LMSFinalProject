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
    public class UserDetailsController : Controller
    {
        private LMSFinalEntities1 db = new LMSFinalEntities1();

        //**************************************************
        //public ActionResult UntypedView()
        //{
        //    //var userNames = db.UserDetails.ToString((u => u.FirstName + "" + u.LastName);

        //    var userNames = db.UserDetails.Where(u => u.FirstName != null);
        //    ViewBag.UserNames = userNames;

        //    return View();
        //}
        //**************************************************


        #region AJAX Delete
        [HttpPost]
        public JsonResult AjaxDelete(int id)
        {
            UserDetail usede= db.UserDetails.Find(id);

            db.UserDetails.Remove(usede);

            db.SaveChanges();

            var message = $"Deleted Course {usede.FirstName} from the database!";
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
        public PartialViewResult UserDetailsDetails(int id)
        {
            UserDetail usede = db.UserDetails.Find(id);

            return PartialView(usede);

        }
        #endregion

        #region AJAX Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult UserDetailsCreate(UserDetail userdetail)
        {
            db.UserDetails.Add(userdetail);
            db.SaveChanges();
            return Json(userdetail);
        }
        #endregion

        #region AJAX Edit
        public PartialViewResult UserDetailsEdit(int id)
        {
            UserDetail usede = db.UserDetails.Find(id);
            return PartialView(usede);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AjaxEdit(UserDetail userdetail)
        {
            db.Entry(userdetail).State = EntityState.Modified;
            db.SaveChanges();
            return Json(userdetail);
        }

        #endregion

        // GET: UserDetails
        public ActionResult Index()
        {
            return View(db.UserDetails.ToList());
        }

        // GET: UserDetails/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserDetail userDetail = db.UserDetails.Find(id);
            if (userDetail == null)
            {
                return HttpNotFound();
            }
            return View(userDetail);
        }

        // GET: UserDetails/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserId,FirstName,LastName")] UserDetail userDetail)
        {
            if (ModelState.IsValid)
            {
                db.UserDetails.Add(userDetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(userDetail);
        }

        // GET: UserDetails/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserDetail userDetail = db.UserDetails.Find(id);
            if (userDetail == null)
            {
                return HttpNotFound();
            }
            return View(userDetail);
        }

        // POST: UserDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,FirstName,LastName")] UserDetail userDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userDetail);
        }

        // GET: UserDetails/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserDetail userDetail = db.UserDetails.Find(id);
            if (userDetail == null)
            {
                return HttpNotFound();
            }
            return View(userDetail);
        }

        // POST: UserDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            UserDetail userDetail = db.UserDetails.Find(id);
            db.UserDetails.Remove(userDetail);
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
