using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using araniyor.Models;

namespace araniyor.Controllers.Admin
{
    public class UsersController : Controller
    {
        private Araniyor db = new Araniyor();

        // GET: Users
        public ActionResult Index()
        {
            var users = db.Users.Include(u => u.businessCategory).Include(u => u.businessSubategory);
            return View(users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            ViewBag.businessCategoryID = new SelectList(db.businessCategory, "businessCategoryID", "businessCategory1");
            ViewBag.businessSubategoryID = new SelectList(db.businessSubategory, "businessSubategoryID", "businessSubategory1");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "userID,businessCategoryID,businessSubategoryID,username,password,eMail,name,surname,steward,picture,country,city,district,birthday,phone,description,votingNumber,totalScore,score,numberOfComments,numberOfViews,active,gender,experience")] Users users)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(users);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.businessCategoryID = new SelectList(db.businessCategory, "businessCategoryID", "businessCategory1", users.businessCategoryID);
            ViewBag.businessSubategoryID = new SelectList(db.businessSubategory, "businessSubategoryID", "businessSubategory1", users.businessSubategoryID);
            return View(users);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            ViewBag.businessCategoryID = new SelectList(db.businessCategory, "businessCategoryID", "businessCategory1", users.businessCategoryID);
            ViewBag.businessSubategoryID = new SelectList(db.businessSubategory, "businessSubategoryID", "businessSubategory1", users.businessSubategoryID);
            return View(users);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "userID,businessCategoryID,businessSubategoryID,username,password,eMail,name,surname,steward,picture,country,city,district,birthday,phone,description,votingNumber,totalScore,score,numberOfComments,numberOfViews,active,gender,experience")] Users users)
        {
            if (ModelState.IsValid)
            {
                db.Entry(users).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.businessCategoryID = new SelectList(db.businessCategory, "businessCategoryID", "businessCategory1", users.businessCategoryID);
            ViewBag.businessSubategoryID = new SelectList(db.businessSubategory, "businessSubategoryID", "businessSubategory1", users.businessSubategoryID);
            return View(users);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Users users = db.Users.Find(id);
            db.Users.Remove(users);
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
