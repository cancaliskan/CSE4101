using araniyor.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace araniyor.Controllers.Admin
{

    public class AdminController : Controller
    {
        private Araniyor db = new Araniyor();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(string username, string password)
        {
            if (ModelState.IsValid)
            {
                using (Araniyor db = new Araniyor())
                {
                    var user = db.Users.Where(i => i.username.Equals(username) && i.password.Equals(password)).FirstOrDefault();
                    if (user != null)
                    {
                        Session["ID"] = user.userID.ToString();
                        return RedirectToAction("AdminPaneli", "Admin");
                    }
                    else
                    {
                        Response.Write("<script> alert('Bilgilerinizi kontrol edip tekrar deneyiniz.! ') </script>");
                    }
                }
            }
            return View();
        }

        public ActionResult LogOut()
        {
            Session.Clear();
            return RedirectToAction("Index", "Admin");
        }

        public ActionResult ForgetPassword()
        {
            return View();
        }

        [ControlLogin]
        public ActionResult AdminPaneli()
        {
            return View();
        }

        [ControlLogin]
        public ActionResult ListBusinessCategory()
        {
            return View(db.businessCategory.ToList());
        }

        [ControlLogin]
        public ActionResult CreateBusinessCategory()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateBusinessCategory([Bind(Include = "businessCategory1")] businessCategory businessCategory)
        {
            if (ModelState.IsValid)
            {
                businessCategory.businessCategory1 = businessCategory.businessCategory1.ToUpper();
                var temp = db.businessCategory.Where(x => x.businessCategory1 == businessCategory.businessCategory1).FirstOrDefault();
                if (temp == null)
                {
                    db.businessCategory.Add(businessCategory);
                    db.SaveChanges();
                    return RedirectToAction("ListBusinessCategory");
                }
                else
                {
                    TempData["mesaj"] = "Kategori ismi zaten mevcut.!";
                    //Response.Write("<script> alert('Kategori ismi zaten mevcut.! ') </script>");
                }
            }
            return View(businessCategory);
        }

        [ControlLogin]
        public ActionResult EditBusinessCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            businessCategory businessCategory = db.businessCategory.Find(id);
            if (businessCategory == null)
            {
                return HttpNotFound();
            }
            return View(businessCategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditBusinessCategory([Bind(Include = "businessCategoryID,businessCategory1")] businessCategory businessCategory)
        {
            if (ModelState.IsValid)
            {
                businessCategory.businessCategory1 = businessCategory.businessCategory1.ToUpper();
                var temp = db.businessCategory.Where(x => x.businessCategory1 == businessCategory.businessCategory1).FirstOrDefault();
                if (temp == null)
                {
                    db.Entry(businessCategory).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("ListBusinessCategory");
                }
                else
                {
                    TempData["mesaj"] = "Kategori ismi zaten mevcut.!";
                    //Response.Write("<script> alert('Kategori ismi zaten mevcut.! ') </script>");
                }
            }
            return View(businessCategory);
        }

        [ControlLogin]
        public ActionResult DetailsBusinessCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            businessCategory businessCategory = db.businessCategory.Find(id);
            if (businessCategory == null)
            {
                return HttpNotFound();
            }
            ViewBag.AltKategoriler = db.businessSubategory.Where(a => a.businessCategoryID == id).ToList();
            return View(businessCategory);
        }

        [ControlLogin]
        public ActionResult DeleteBusinessCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            businessCategory businessCategory = db.businessCategory.Find(id);
            if (businessCategory == null)
            {
                return HttpNotFound();
            }
            return View(businessCategory);
        }

        // POST: deneme/Delete/5
        [HttpPost, ActionName("DeleteBusinessCategory")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            businessCategory businessCategory = db.businessCategory.Find(id);
            db.businessCategory.Remove(businessCategory);
            db.SaveChanges();
            return RedirectToAction("ListBusinessCategory");
        }

        [ControlLogin]
        public ActionResult ListSubBusinessCategory()
        {
            return View(db.businessSubategory.ToList());
        }

        [ControlLogin]
        public ActionResult CreateSubBusinessCategory()
        {
            ViewBag.businessCategoryID = new SelectList(db.businessCategory, "businessCategoryID", "businessCategory1");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateSubBusinessCategory([Bind(Include = "businessSubategoryID,businessCategoryID,businessSubategory1")] businessSubategory businessSubategory)
        {
            if (ModelState.IsValid)
            {
                businessSubategory.businessSubategory1 = businessSubategory.businessSubategory1.ToUpper();
                var kategoriTemp = db.businessCategory.Where(x => x.businessCategoryID == businessSubategory.businessCategoryID).FirstOrDefault().businessCategory1.ToString();
                if (businessSubategory.businessSubategory1 == kategoriTemp)
                    TempData["mesaj"] = "Kategori ismi ile alt kategori ismi aynı olamaz.!";
                //Response.Write("<script> alert('Kategori ismi ile alt kategori ismi aynı olamaz.! ') </script>");
                else
                {
                    var temp = db.businessSubategory.Where(a => a.businessSubategory1 == businessSubategory.businessSubategory1).FirstOrDefault();
                    if (temp == null)
                    {
                        db.businessSubategory.Add(businessSubategory);
                        db.SaveChanges();
                        return RedirectToAction("ListSubBusinessCategory");
                    }
                    else
                    {
                        TempData["mesaj"] = "Alt Kategori ismi zaten mevcut.!";
                        //Response.Write("<script> alert('Alt Kategori ismi zaten mevcut.! ') </script>");
                    }
                }
            }

            ViewBag.businessCategoryID = new SelectList(db.businessCategory, "businessCategoryID", "businessCategory1", businessSubategory.businessCategoryID);
            return View(businessSubategory);
        }

        [ControlLogin]
        public ActionResult DetailsSubBusinessCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            businessSubategory businessSubategory = db.businessSubategory.Find(id);
            if (businessSubategory == null)
            {
                return HttpNotFound();
            }
            return View(businessSubategory);
        }

        [ControlLogin]
        public ActionResult EditSubBusinessCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            businessSubategory businessSubategory = db.businessSubategory.Find(id);
            if (businessSubategory == null)
            {
                return HttpNotFound();
            }
            ViewBag.businessCategoryID = new SelectList(db.businessCategory, "businessCategoryID", "businessCategory1", businessSubategory.businessCategoryID);
            return View(businessSubategory);
        }

        [ControlLogin]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSubBusinessCategory([Bind(Include = "businessSubategoryID,businessCategoryID,businessSubategory1")] businessSubategory businessSubategory)
        {
            if (ModelState.IsValid)
            {
                businessSubategory.businessSubategory1 = businessSubategory.businessSubategory1.ToUpper();
                var kategoriTemp = db.businessCategory.Where(x => x.businessCategoryID == businessSubategory.businessCategoryID).FirstOrDefault().businessCategory1.ToString();
                if (businessSubategory.businessSubategory1 == kategoriTemp)
                    TempData["mesaj"] = "Kategori ismi ile alt kategori ismi aynı olamaz.!";
                //Response.Write("<script> alert('Kategori ismi ile alt kategori ismi aynı olamaz.! ') </script>");
                else
                {
                    var temp = db.businessSubategory.Where(a => a.businessSubategory1 == businessSubategory.businessSubategory1).FirstOrDefault();
                    if (temp == null)
                    {
                        db.Entry(businessSubategory).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("ListSubBusinessCategory");
                    }
                    else
                    {
                        TempData["mesaj"] = "Alt Kategori ismi zaten mevcut.!";
                        //Response.Write("<script> alert('Alt Kategori ismi zaten mevcut.! ') </script>");
                    }
                }
            }
            ViewBag.businessCategoryID = new SelectList(db.businessCategory, "businessCategoryID", "businessCategory1", businessSubategory.businessCategoryID);
            return View(businessSubategory);
        }

        [ControlLogin]
        public ActionResult DeleteSubBusinessCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            businessSubategory businessSubategory = db.businessSubategory.Find(id);
            if (businessSubategory == null)
            {
                return HttpNotFound();
            }
            return View(businessSubategory);
        }

        [ControlLogin]
        [HttpPost, ActionName("DeleteSubBusinessCategory")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmedSubCategory(int id)
        {
            businessSubategory businessSubategory = db.businessSubategory.Find(id);
            db.businessSubategory.Remove(businessSubategory);
            db.SaveChanges();
            return RedirectToAction("ListSubBusinessCategory");
        }

        [ControlLogin]
        public ActionResult ListBlocked()
        {
            return View(db.blocked.ToList());
        }

        [ControlLogin]
        public ActionResult CreateBlocked()
        {
            ViewBag.blockerID = new SelectList(db.Users, "userID", "username");
            ViewBag.blockedID = new SelectList(db.Users, "userID", "username");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateBlocked([Bind(Include = "blockID,blockerID,blockedID")] blocked blocked)
        {
            if (ModelState.IsValid)
            {
                var temp = db.blocked.Where(a => a.blockerID == blocked.blockerID && a.blockerID == blocked.blockerID).FirstOrDefault();
                if (temp == null)
                {
                    if (blocked.blockerID == blocked.blockedID)
                        TempData["mesaj"] = "Engelleyen kullanıcı ile engellenen kullanıcı aynı olamaz.!";
                    //Response.Write("<script> alert('Engelleyen kullanıcı ile engellenen kullanıcı aynı olamaz.! ') </script>");
                    else
                    {
                        blocked.date = DateTime.Now.ToUniversalTime();
                        db.blocked.Add(blocked);
                        db.SaveChanges();
                        return RedirectToAction("ListBlocked");
                    }
                }
                else
                {
                    Response.Write("<script> alert('Aynı kayıt zaten mevcut.! ') </script>");
                }
            }

            ViewBag.blockerID = new SelectList(db.Users, "userID", "username", blocked.blockerID);
            ViewBag.blockedID = new SelectList(db.Users, "userID", "username", blocked.blockedID);
            return View(blocked);
        }

        [ControlLogin]
        public ActionResult EditBlocked(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            blocked blocked = db.blocked.Find(id);
            if (blocked == null)
            {
                return HttpNotFound();
            }
            ViewBag.blockerID = new SelectList(db.Users, "userID", "username", blocked.blockerID);
            ViewBag.blockedID = new SelectList(db.Users, "userID", "username", blocked.blockedID);
            return View(blocked);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditBlocked([Bind(Include = "blockID,blockerID,blockedID,date")] blocked blocked)
        {
            if (ModelState.IsValid)
            {
                if (blocked.blockerID == blocked.blockedID)
                    TempData["mesaj"] = "Engelleyen kullanıcı ile engellenen kullanıcı aynı olamaz.!";
                //Response.Write("<script> alert('Engelleyen kullanıcı ile engellenen kullanıcı aynı olamaz.! ') </script>");
                else
                {
                    db.Entry(blocked).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("ListBlocked");
                }
            }
            ViewBag.blockerID = new SelectList(db.Users, "userID", "username", blocked.blockerID);
            ViewBag.blockedID = new SelectList(db.Users, "userID", "username", blocked.blockedID);
            return View(blocked);
        }

        [ControlLogin]
        public ActionResult DetailsBlocked(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            blocked blocked = db.blocked.Find(id);
            if (blocked == null)
            {
                return HttpNotFound();
            }
            return View(blocked);
        }

        public ActionResult DeleteBlocked(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            blocked blocked = db.blocked.Find(id);
            if (blocked == null)
            {
                return HttpNotFound();
            }
            return View(blocked);
        }

        // POST: blockeds/Delete/5
        [HttpPost, ActionName("DeleteBlocked")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmedBlocked(int id)
        {
            blocked blocked = db.blocked.Find(id);
            db.blocked.Remove(blocked);
            db.SaveChanges();
            return RedirectToAction("ListBlocked");
        }


        [ControlLogin]
        public ActionResult ListUser()
        {
            return View(db.Users.ToList());
        }

        [ControlLogin]
        public ActionResult CreateUser()
        {
            ViewBag.businessCategoryID = new SelectList(db.businessCategory, "businessCategoryID", "businessCategory1");
            ViewBag.businessSubategoryID = new SelectList(db.businessSubategory, "businessSubategoryID", "businessSubategory1");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUser([Bind(Include = "userID,businessCategoryID,businessSubategoryID,username,password,eMail,name,surname,steward,picture,country,city,district,birthday,phone,description,votingNumber,totalScore,score,numberOfComments,numberOfViews,active,gender,experience")] Users users)
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
