using araniyor.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
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
                    var user = db.Users.Where(i => i.username.Equals(username) && i.password.Equals(password)).SingleOrDefault();
                    if (user != null && user.admin == true)
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

        [HttpPost]
        public ActionResult ForgetPassword(string eMail)
        {
            var user = db.Users.Where(x => x.eMail == eMail).FirstOrDefault();
            if (user == null)
            {
                TempData["mesaj"] = "E-Posta adresine kayıtlı kullanıcı bulunmuyor.";
            }
            else
            {

                string server = ConfigurationManager.AppSettings["server"];
                int port = int.Parse(ConfigurationManager.AppSettings["port"]);
                bool ssl = ConfigurationManager.AppSettings["ssl"].ToString() == "1" ? true : false;
                string from = ConfigurationManager.AppSettings["from"];
                string password = ConfigurationManager.AppSettings["password"];
                string fromname = ConfigurationManager.AppSettings["fromname"];

                var client = new SmtpClient();
                client.Host = server;
                client.Port = port;
                client.EnableSsl = ssl;
                client.UseDefaultCredentials = true;
                client.Credentials = new System.Net.NetworkCredential(from, password);

                var gonderilecekMail = new MailMessage();
                gonderilecekMail.From = new MailAddress(from, fromname);
                gonderilecekMail.To.Add(eMail);
                gonderilecekMail.Subject = "Şifremi Unuttum";
                gonderilecekMail.IsBodyHtml = false;
                gonderilecekMail.Body = "Kullanıcı Adınız : " + user.username + "\n" + "Parolanız : " + user.password;
                client.Send(gonderilecekMail);

                //catch (Exception)
                //{
                //    TempData["mesaj"] = "E-Posta gönderilirken bir sorun oluştu. Lütfen daha sonra tekrar deneyin.";
                //}

            }
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
        [ControlLogin]
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
        [ControlLogin]
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
        [ControlLogin]
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
        [ControlLogin]
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
        [ControlLogin]
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
        [ControlLogin]
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

        [ControlLogin]
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
        [ControlLogin]
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

        [HttpPost]
        [ControlLogin]
        public JsonResult GetirKategori(string kategoriId)
        {
            Araniyor entities = new Araniyor();

            int kategoriID = Convert.ToInt32(kategoriId);

            var altKategoriler = entities.businessSubategory.Where(x => x.businessCategoryID == kategoriID).Select(a => new { Id = a.businessSubategoryID, Ad = a.businessSubategory1 }).ToList();
            return Json(altKategoriler);
        }

        [HttpPost]
        [ControlLogin]
        public JsonResult GetirIlceler(string ilId)
        {
            Araniyor entities = new Araniyor();
            int ilID = 0;
            if (!string.IsNullOrEmpty(ilId))
                ilID = Convert.ToInt32(ilId);

            var ilceler = entities.District.Where(ilce => ilce.CityID == ilID).Select(ilce => new { Id = ilce.ID, Ad = ilce.District1 }).ToList();
            return Json(ilceler);
        }

        [ControlLogin]
        public ActionResult CreateUser()
        {
            Users user = new Users();
            user.active = true;
            user.admin = false;
            ViewBag.Kategori = db.businessCategory.ToList();
            ViewBag.Sehir = db.City.ToList();
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ControlLogin]
        public ActionResult CreateUser([Bind(Include = "userID,businessCategoryID,businessSubategoryID,username,password,eMail,name,surname,steward,country,city,district,birthday,phone,description,votingNumber,totalScore,score,numberOfComments,numberOfViews,active,gender,experience,admin")] Users users, HttpPostedFileBase fileup)
        {
            var usernameVarmi = db.Users.Where(a => a.username == users.username).FirstOrDefault();
            if (usernameVarmi != null)
                TempData["mesaj"] = "Kullanıcı adı sisemde mevcut.!";
            else
            {
                if (users.businessCategoryID == 0 || users.businessSubategoryID == 0)
                    TempData["mesaj"] = "Kategori veya alt kategori boş geçilemez.!";
                else
                {
                    if (fileup != null)
                    {
                        users.picture = new byte[fileup.ContentLength];
                        fileup.InputStream.Read(users.picture, 0, fileup.ContentLength);

                    }
                    else
                    {
                        byte[] userImage = System.IO.File.ReadAllBytes(Server.MapPath(@"~\images\search-people.png"));
                        users.picture = userImage;
                    }
                    if (users.steward == true)
                    {
                        if ((users.city != null && users.district != null
                        && users.birthday != null && users.description != null && users.gender != null && users.experience != null))
                        {
                            if (ModelState.IsValid)
                            {
                                db.Users.Add(users);
                                db.SaveChanges();
                                return RedirectToAction("ListUser");
                            }
                        }
                        else
                            TempData["mesaj"] = "Lütfen tüm alanları doldurunuz.!";
                    }
                    else
                    {
                        if (fileup != null)
                        {
                            users.picture = new byte[fileup.ContentLength];
                            fileup.InputStream.Read(users.picture, 0, fileup.ContentLength);

                        }
                        else
                        {
                            byte[] userImage = System.IO.File.ReadAllBytes(Server.MapPath(@"~\images\search-people.png"));
                            users.picture = userImage;
                        }
                        if (ModelState.IsValid)
                        {
                            db.Users.Add(users);
                            db.SaveChanges();
                            return RedirectToAction("ListUser");
                        }
                    }
                }
            }
            Users user = new Users();
            user.active = true;
            ViewBag.Kategori = db.businessCategory.ToList();
            ViewBag.Sehir = db.City.ToList();
            ViewBag.businessCategoryID = new SelectList(db.businessCategory, "businessCategoryID", "businessCategory1", users.businessCategoryID);
            ViewBag.businessSubategoryID = new SelectList(db.businessSubategory, "businessSubategoryID", "businessSubategory1", users.businessSubategoryID);
            return View(users);
        }

        [ControlLogin]
        public ActionResult DetailsUser(int? id)
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

        [ControlLogin]
        public ActionResult EditUser(int? id)
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
            ViewBag.Kategori = db.businessCategory.ToList();
            ViewBag.Sehir = db.City.ToList();

            //ViewBag.businessCategoryID = new SelectList(db.businessCategory, "businessCategoryID", "businessCategory1", users.businessCategoryID);
            //ViewBag.businessSubategoryID = new SelectList(db.businessSubategory, "businessSubategoryID", "businessSubategory1", users.businessSubategoryID);
            return View(users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ControlLogin]
        public ActionResult EditUser([Bind(Include = "userID,businessCategoryID,businessSubategoryID,username,password,eMail,name,surname,steward,picture,country,city,district,birthday,phone,description,votingNumber,totalScore,score,numberOfComments,numberOfViews,active,gender,experience,admin")] Users users, HttpPostedFileBase fileup)
        {
            //var tempUser = db.Users.Where(x => x.username == users.username).FirstOrDefault();
            //if ((tempUser != null && tempUser.userID != users.userID))
            //    TempData["mesaj"] = "Kullanıcı adı sistemde mevcut.!";
            //else
            //{
            if (users.businessCategoryID == 0 || users.businessSubategoryID == 0)
                TempData["mesaj"] = "Kategori veya alt kategori boş geçilemez.!";
            else
            {
                db.Entry(users).State = EntityState.Modified;

                if (fileup != null)
                {
                    users.picture = new byte[fileup.ContentLength];
                    fileup.InputStream.Read(users.picture, 0, fileup.ContentLength);

                }
                else
                {
                    db.Users.Attach(users);
                    ////var entry = db.Entry(aboutUs);
                    db.Entry(users).Property(x => x.picture).IsModified = false;
                }
                if (users.steward == true)
                {
                    if ((users.city != null && users.district != null
                    && users.birthday != null && users.description != null && users.gender != null && users.experience != null))
                    {

                        if (ModelState.IsValid)
                        {
                            db.SaveChanges();
                            return RedirectToAction("ListUser");
                        }
                    }
                    else
                        TempData["mesaj"] = "Lütfen tüm alanları doldurunuz.!";
                }
                else
                {
                    //if (fileup != null)
                    //{
                    //    users.picture = new byte[fileup.ContentLength];
                    //    fileup.InputStream.Read(users.picture, 0, fileup.ContentLength);

                    //}
                    //else
                    //{
                    //    byte[] userImage = System.IO.File.ReadAllBytes(Server.MapPath(@"~\images\search-people.png"));
                    //    users.picture = userImage;
                    //}
                    if (ModelState.IsValid)
                    {
                        //db.Entry(users).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("ListUser");
                    }
                }
            }
            //}
            ViewBag.Kategori = db.businessCategory.ToList();
            ViewBag.Sehir = db.City.ToList();

            //ViewBag.businessCategoryID = new SelectList(db.businessCategory, "businessCategoryID", "businessCategory1", users.businessCategoryID);
            //ViewBag.businessSubategoryID = new SelectList(db.businessSubategory, "businessSubategoryID", "businessSubategory1", users.businessSubategoryID);
            return View(users);
        }

        public ActionResult EditUserProfilResmiKaldir(int? id)
        {
            var user = db.Users.Find(id);
            if (ModelState.IsValid)
            {
                byte[] userImage = System.IO.File.ReadAllBytes(Server.MapPath(@"~\images\search-people.png"));
                user.picture = userImage;
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("EditUser", "Admin", new { id = id });
        }

        [ControlLogin]
        public ActionResult DeleteUser(int? id)
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
        [ControlLogin]
        [HttpPost, ActionName("DeleteUser")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmedUser(int id)
        {
            Users users = db.Users.Find(id);
            db.Users.Remove(users);
            db.SaveChanges();
            return RedirectToAction("ListUser");
        }

        [ControlLogin]
        public ActionResult ListYorum()
        {
            return View(db.Yorumlar.ToList());
        }

        [ControlLogin]
        public ActionResult CreateYorum()
        {
            ViewBag.Users = new SelectList(db.Users, "userID", "username");
            return View();
        }

        [ControlLogin]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateYorum([Bind(Include = "ID,yorumYapan,yorumYapilan,yorumMetni,yorumTarihi")] Yorumlar yorumlar)
        {
            if (yorumlar.yorumYapan == yorumlar.yorumYapilan)
            {
                TempData["mesaj"] = "Kişi kendine yorum yapamaz.!";
            }
            else
            {
                if (ModelState.IsValid)
                {
                    db.Yorumlar.Add(yorumlar);
                    db.SaveChanges();
                    var user = db.Users.Find(yorumlar.yorumYapilan);
                    if (user.numberOfComments == 0 || user.numberOfComments == null)
                        user.numberOfComments = 1;
                    else
                        user.numberOfComments += 1;
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("ListYorum");
                }
            }
            ViewBag.Users = new SelectList(db.Users, "userID", "username");
            return View(yorumlar);
        }

        [ControlLogin]
        public ActionResult DetailsYorum(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Yorumlar yorumlar = db.Yorumlar.Find(id);
            if (yorumlar == null)
            {
                return HttpNotFound();
            }
            return View(yorumlar);
        }

        [ControlLogin]
        public ActionResult EditYorum(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Yorumlar yorumlar = db.Yorumlar.Find(id);
            if (yorumlar == null)
            {
                return HttpNotFound();
            }
            ViewBag.Users = new SelectList(db.Users, "userID", "username");
            return View(yorumlar);
        }

        [ControlLogin]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditYorum([Bind(Include = "ID,yorumYapan,yorumYapilan,yorumMetni,yorumTarihi")] Yorumlar yorumlar)
        {
            if (yorumlar.yorumYapan == yorumlar.yorumYapilan)
            {
                TempData["mesaj"] = "Kişi kendine yorum yapamaz.!";
            }
            else
            {
                if (ModelState.IsValid)
                {
                    db.Entry(yorumlar).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("ListYorum");
                }
            }
            ViewBag.Users = new SelectList(db.Users, "userID", "username");
            return View(yorumlar);
        }

        [ControlLogin]
        public ActionResult DeleteYorum(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Yorumlar yorumlar = db.Yorumlar.Find(id);
            if (yorumlar == null)
            {
                return HttpNotFound();
            }
            return View(yorumlar);
        }

        [ControlLogin]
        [HttpPost, ActionName("DeleteYorum")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmedYorum(int id)
        {
            Yorumlar yorumlar = db.Yorumlar.Find(id);
            var user = db.Users.Find(yorumlar.yorumYapilan);

            db.Yorumlar.Remove(yorumlar);
            db.SaveChanges();
            user.numberOfComments -= 1;
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("ListYorum");
        }

        [ControlLogin]
        public ActionResult ListMesaj()
        {
            return View(db.messages.ToList());
        }

        [ControlLogin]
        public ActionResult DetailsMesaj(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            messages messages = db.messages.Find(id);
            if (messages == null)
            {
                return HttpNotFound();
            }
            return View(messages);
        }

        [ControlLogin]
        public ActionResult CreateMesaj()
        {
            ViewBag.Users = new SelectList(db.Users, "userID", "username");
            return View();
        }

        [ControlLogin]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateMesaj([Bind(Include = "messageID,senderID,receiverID,message,date,conversationID")] messages messages)
        {
            if (messages.senderID == messages.receiverID)
            {
                TempData["mesaj"] = "Kişi kendine mesaj atamaz.!";
            }
            else
            {
                messages.conversationID = Convert.ToString(messages.senderID) + Convert.ToString(messages.receiverID);
                if (ModelState.IsValid)
                {
                    db.messages.Add(messages);
                    db.SaveChanges();
                    return RedirectToAction("ListMesaj");
                }
            }
            ViewBag.Users = new SelectList(db.Users, "userID", "username");
            return View(messages);
        }

        [ControlLogin]
        public ActionResult EditMesaj(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            messages messages = db.messages.Find(id);
            if (messages == null)
            {
                return HttpNotFound();
            }
            ViewBag.Users = new SelectList(db.Users, "userID", "username");
            return View(messages);
        }

        [ControlLogin]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditMesaj([Bind(Include = "messageID,senderID,receiverID,message,date,conversationID")] messages messages)
        {
            if (messages.senderID == messages.receiverID)
            {
                TempData["mesaj"] = "Kişi kendine mesaj atamaz.!";
            }
            else
            {
                if (ModelState.IsValid)
                {
                    db.Entry(messages).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("ListMesaj");
                }
            }
            ViewBag.Users = new SelectList(db.Users, "userID", "username");
            return View(messages);
        }

        [ControlLogin]
        public ActionResult DeleteMesaj(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            messages messages = db.messages.Find(id);
            if (messages == null)
            {
                return HttpNotFound();
            }
            return View(messages);
        }

        // POST: messages/Delete/5
        [ControlLogin]
        [HttpPost, ActionName("DeleteMesaj")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmedMesaj(int id)
        {
            messages messages = db.messages.Find(id);
            db.messages.Remove(messages);
            db.SaveChanges();
            return RedirectToAction("ListMesaj");
        }

        [ControlLogin]
        public ActionResult ListPuan()
        {
            return View(db.Puanlar.ToList());
        }

        [ControlLogin]
        public ActionResult DetailsPuan(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Puanlar puanlar = db.Puanlar.Find(id);
            if (puanlar == null)
            {
                return HttpNotFound();
            }
            return View(puanlar);
        }


        [ControlLogin]
        public ActionResult CreatePuan()
        {
            ViewBag.Users = new SelectList(db.Users, "userID", "username");
            return View();
        }

        [ControlLogin]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePuan([Bind(Include = "ID,puanVeren,puanVerilen,Tarih,Puan")] Puanlar puanlar)
        {
            if (puanlar.puanVeren == puanlar.puanVerilen)
            {
                TempData["mesaj"] = "Kişi kendine puan veremez.!";
            }
            else
            {
                if (ModelState.IsValid)
                {
                    db.Puanlar.Add(puanlar);
                    db.SaveChanges();
                    var user = db.Users.Find(puanlar.puanVerilen);
                    var toplamPuanSayisiList = db.Puanlar.Where(a => a.puanVerilen == puanlar.puanVerilen).ToList();
                    int totalScore = 0;
                    for (int i = 0; i < toplamPuanSayisiList.Count; i++)
                    {
                        totalScore += toplamPuanSayisiList[i].Puan;
                    }
                    user.score = (Convert.ToDouble(totalScore) / Convert.ToDouble(toplamPuanSayisiList.Count));
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("ListPuan");
                }
            }
            ViewBag.Users = new SelectList(db.Users, "userID", "username");
            return View(puanlar);
        }

        [ControlLogin]
        public ActionResult EditPuan(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Puanlar puanlar = db.Puanlar.Find(id);
            if (puanlar == null)
            {
                return HttpNotFound();
            }
            ViewBag.Users = new SelectList(db.Users, "userID", "username");
            return View(puanlar);
        }

        [ControlLogin]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPuan([Bind(Include = "ID,puanVeren,puanVerilen,Tarih,Puan")] Puanlar puanlar)
        {
            if (ModelState.IsValid)
            {
                db.Entry(puanlar).State = EntityState.Modified;
                db.SaveChanges();
                var user = db.Users.Find(puanlar.puanVerilen);
                var toplamPuanSayisiList = db.Puanlar.Where(a => a.puanVerilen == puanlar.puanVerilen).ToList();
                int totalScore = 0;
                for (int i = 0; i < toplamPuanSayisiList.Count; i++)
                {
                    totalScore += toplamPuanSayisiList[i].Puan;
                }
                user.score = (Convert.ToDouble(totalScore) / Convert.ToDouble(toplamPuanSayisiList.Count));
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ListPuan");
            }
            ViewBag.Users = new SelectList(db.Users, "userID", "username");
            return View(puanlar);
        }

        [ControlLogin]
        public ActionResult DeletePuan(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Puanlar puanlar = db.Puanlar.Find(id);
            if (puanlar == null)
            {
                return HttpNotFound();
            }
            return View(puanlar);
        }

        // POST: Puanlars/Delete/5
        [ControlLogin]
        [HttpPost, ActionName("DeletePuan")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmedPuan(int id)
        {
            Puanlar puanlar = db.Puanlar.Find(id);
            db.Puanlar.Remove(puanlar);
            db.SaveChanges();
            var user = db.Users.Find(puanlar.puanVerilen);
            var toplamPuanSayisiList = db.Puanlar.Where(a => a.puanVerilen == puanlar.puanVerilen).ToList();
            int totalScore = 0;
            if (toplamPuanSayisiList.Count != 0)
            {
                for (int i = 0; i < toplamPuanSayisiList.Count; i++)
                {
                    totalScore += toplamPuanSayisiList[i].Puan;
                }
                user.score = (Convert.ToDouble(totalScore) / Convert.ToDouble(toplamPuanSayisiList.Count));
            }
            else
            {
                user.score = 0;
            }
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("ListPuan");
        }

        [ControlLogin]
        public ActionResult ListProfilGoruntuleme()
        {
            return View(db.ProfilGoruntuleme.ToList());
        }

        [ControlLogin]
        public ActionResult DetailsProfilGoruntuleme(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProfilGoruntuleme profilGoruntuleme = db.ProfilGoruntuleme.Find(id);
            if (profilGoruntuleme == null)
            {
                return HttpNotFound();
            }
            return View(profilGoruntuleme);
        }

        [ControlLogin]
        public ActionResult CreateProfilGoruntuleme()
        {
            ViewBag.Users = new SelectList(db.Users, "userID", "username");
            return View();
        }

        [ControlLogin]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateProfilGoruntuleme([Bind(Include = "ID,goruntuleyen,goruntulenen,Tarih")] ProfilGoruntuleme profilGoruntuleme)
        {
            if (profilGoruntuleme.goruntulenen == profilGoruntuleme.goruntuleyen)
            {
                TempData["mesaj"] = "Kendi profil resmini görüntülemesi geçersizdir.!";
            }
            else
            {
                if (ModelState.IsValid)
                {
                    profilGoruntuleme.Tarih = DateTime.Now;
                    db.ProfilGoruntuleme.Add(profilGoruntuleme);
                    db.SaveChanges();
                    var user = db.Users.Find(profilGoruntuleme.goruntulenen);

                    if (user.numberOfViews == 0 || user.numberOfViews == null)
                        user.numberOfViews = 1;
                    else
                        user.numberOfViews += 1;

                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("ListProfilGoruntuleme");
                }
            }
            ViewBag.Users = new SelectList(db.Users, "userID", "username");
            return View(profilGoruntuleme);
        }

        [ControlLogin]
        public ActionResult DeleteProfilGoruntuleme(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProfilGoruntuleme profilGoruntuleme = db.ProfilGoruntuleme.Find(id);
            if (profilGoruntuleme == null)
            {
                return HttpNotFound();
            }
            return View(profilGoruntuleme);
        }

        // POST: ProfilGoruntulemes/Delete/5
        [ControlLogin]
        [HttpPost, ActionName("DeleteProfilGoruntuleme")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmedProfilGoruntuleme(int id)
        {
            ProfilGoruntuleme profilGoruntuleme = db.ProfilGoruntuleme.Find(id);
            db.ProfilGoruntuleme.Remove(profilGoruntuleme);
            db.SaveChanges();
            return RedirectToAction("ListProfilGoruntuleme");
        }

        [ControlLogin]
        public ActionResult EditHakkimizda()
        {
            AboutUs aboutUs = db.AboutUs.FirstOrDefault();

            if (aboutUs == null)
            {
                return HttpNotFound();
            }
            return View(aboutUs);
        }

        [ControlLogin]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        //public ActionResult EditHakkimizda([Bind(Include = "ID,Hakkimizda,Misyon,Vizyon,Adres,Telefon,EPosta,Facebook,Twitter,LinkedIn,HakkimizdaResim,KullaniciSozlesmesi")] AboutUs aboutUs
        //    , HttpPostedFileBase HakkimizdaResim, HttpPostedFileBase VizyonResim, HttpPostedFileBase MisyonResim, HttpPostedFileBase SliderResim1, HttpPostedFileBase SliderResim2
        //    , HttpPostedFileBase SliderResim3, HttpPostedFileBase SliderResim4)
        //{
        public ActionResult EditHakkimizda(string Hakkimizda, string Misyon, string Vizyon, string Adres, string Telefon, string EPosta, string Facebook
            , string Twitter, string LinkedIn, string KullaniciSozlesmesi, HttpPostedFileBase HakkimizdaResim, HttpPostedFileBase VizyonResim, HttpPostedFileBase MisyonResim, HttpPostedFileBase SliderResim1, HttpPostedFileBase SliderResim2
            , HttpPostedFileBase SliderResim3, HttpPostedFileBase SliderResim4)
        {

            //var tempKayit = db.AboutUs.Where(x => x.ID == aboutUs.ID).SingleOrDefault();
            //db.Entry(tempKayit).State = EntityState.Modified;
            //db.Entry(aboutUs).State = EntityState.Modified;
            AboutUs aboutUs = db.AboutUs.FirstOrDefault();
            if (ModelState.IsValid)
            {
                aboutUs.Hakkimizda = Hakkimizda;
                aboutUs.Misyon = Misyon;
                aboutUs.Vizyon = Vizyon;
                aboutUs.Adres = Adres;
                aboutUs.Telefon = Telefon;
                aboutUs.EPosta = EPosta;
                aboutUs.Facebook = Facebook;
                aboutUs.Twitter = Twitter;
                aboutUs.LinkedIn = LinkedIn;
                aboutUs.KullaniciSozlesmesi = KullaniciSozlesmesi;

                if (HakkimizdaResim != null)
                {
                    byte[] thePictureAsBytes = new byte[HakkimizdaResim.ContentLength];
                    using (BinaryReader theReader = new BinaryReader(HakkimizdaResim.InputStream))
                    {
                        thePictureAsBytes = theReader.ReadBytes(HakkimizdaResim.ContentLength);
                    }
                    aboutUs.HakkimizdaResim = thePictureAsBytes;
                    //HakkimizdaResim.InputStream.Read(aboutUs.HakkimizdaResim, 0, HakkimizdaResim.ContentLength);
                }
                //else
                //{
                //    //db.AboutUs.Attach(aboutUs);
                //    ////var entry = db.Entry(aboutUs);
                //    //var s = Convert.FromBase64String();
                //    //db.Entry(aboutUs).Property(x => x.HakkimizdaResim).IsModified = false;
                //    //aboutUs.HakkimizdaResim = Convert.FromBase64String(HakkimizdaResimTemp);//Encoding.UTF8.GetBytes(HakkimizdaResimTemp);
                //}

                if (VizyonResim != null)
                {
                    aboutUs.VizyonResim = new byte[VizyonResim.ContentLength];
                    VizyonResim.InputStream.Read(aboutUs.VizyonResim, 0, VizyonResim.ContentLength);
                }

                if (MisyonResim != null)
                {
                    aboutUs.MisyonResim = new byte[MisyonResim.ContentLength];
                    MisyonResim.InputStream.Read(aboutUs.MisyonResim, 0, MisyonResim.ContentLength);
                }

                if (SliderResim1 != null)
                {
                    aboutUs.SliderResim1 = new byte[SliderResim1.ContentLength];
                    SliderResim1.InputStream.Read(aboutUs.SliderResim1, 0, SliderResim1.ContentLength);
                }

                if (SliderResim2 != null)
                {

                    aboutUs.SliderResim2 = new byte[SliderResim2.ContentLength];
                    SliderResim2.InputStream.Read(aboutUs.SliderResim2, 0, SliderResim2.ContentLength);
                }

                if (SliderResim3 != null)
                {
                    aboutUs.SliderResim3 = new byte[SliderResim3.ContentLength];
                    SliderResim3.InputStream.Read(aboutUs.SliderResim3, 0, SliderResim3.ContentLength);
                }

                if (SliderResim4 != null)
                {
                    aboutUs.SliderResim4 = new byte[SliderResim4.ContentLength];
                    SliderResim4.InputStream.Read(aboutUs.SliderResim4, 0, SliderResim4.ContentLength);
                }
                db.Entry(aboutUs).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("AdminPaneli", "Admin");
            }
            return View(aboutUs);
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
