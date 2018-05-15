using araniyor.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;


namespace araniyor.Controllers.Home
{
    public class HomeController : Controller
    {
        private Araniyor db = new Araniyor();

        // GET: Home
        [ValidateInput(false)]
        public ActionResult Index()
        {
            AboutUs aboutUs = db.AboutUs.FirstOrDefault();

            ViewBag.Users = db.Users.ToList();

            return View(aboutUs);
        }
        public ActionResult Hakkimizda()
        {
            AboutUs aboutUs = db.AboutUs.FirstOrDefault();
            string aboutUsHakkimizda = db.AboutUs.FirstOrDefault().Hakkimizda;
            ViewBag.Hakkimizda = MvcHtmlString.Create(aboutUsHakkimizda.Replace(Environment.NewLine, "<br/>"));
            ViewBag.Users = db.Users.ToList();

            return View(aboutUs);
        }
        public ActionResult Misyon()
        {
            AboutUs aboutUs = db.AboutUs.FirstOrDefault();
            string aboutUsMisyon = db.AboutUs.FirstOrDefault().Misyon;
            ViewBag.Misyon = MvcHtmlString.Create(aboutUsMisyon.Replace(Environment.NewLine, "<br/>"));

            ViewBag.Users = db.Users.ToList();

            return View(aboutUs);
        }
        public ActionResult Vizyon()
        {
            AboutUs aboutUs = db.AboutUs.FirstOrDefault();
            string aboutUsVizyon = db.AboutUs.FirstOrDefault().Vizyon;
            ViewBag.Vizyon = MvcHtmlString.Create(aboutUsVizyon.Replace(Environment.NewLine, "<br/>"));
            ViewBag.Users = db.Users.ToList();

            return View(aboutUs);
        }

        public ActionResult Kategoriler()
        {
            AboutUs aboutUs = db.AboutUs.FirstOrDefault();

            ViewBag.Kategori = db.businessCategory.ToList();
            ViewBag.AltKategori = db.businessSubategory.ToList();
            ViewBag.Users = db.Users.ToList();

            return View(aboutUs);
        }

        int pageSize = 9;
        public ActionResult ListUser(int? altKategoriID, int? page, string siralamaSekli
                        , string userList, string deneyimListesi, bool? ilkListeleme, string puanlama, string cinsiyetListesi, bool? kelimeArama)
        {
            Thread.Sleep(500);
            ViewBag.altKategoriID = altKategoriID;
            ViewBag.userList1 = null;
            ViewBag.siralamaSekli = null;
            ViewBag.Users = db.Users.ToList();
            //içe - puan aralığı - cinsiyete göre filtreleme yapılacak 
            ViewBag.ilce = db.Users.Where(x => x.businessSubategoryID == altKategoriID && x.steward == true && x.active == true).ToList().Select(y => y.city).Distinct();
            ViewBag.ilceListe = db.City.ToList();
            ViewBag.Deneyim = db.Users.OrderByDescending(x => x.experience).Where(x => x.businessSubategoryID == altKategoriID && x.steward == true && x.active == true).ToList().Select(y => y.experience).Distinct();
            ViewBag.Puan = db.Users.OrderByDescending(x => x.score).Where(x => x.businessSubategoryID == altKategoriID && x.steward == true && x.active == true).ToList().Select(y => y.score).Distinct();
            ViewBag.Cinsiyet = db.Users.Where(x => x.businessSubategoryID == altKategoriID && x.steward == true && x.active == true).ToList().Select(y => y.gender).Distinct();


            ViewBag.ilkListeleme = ilkListeleme;

            if (ilkListeleme == true)
            {
                if (siralamaSekli == null)
                {
                    ViewBag.siralamaSekli = null;
                    if (page == null)
                    {
                        ViewBag.userList1 = db.Users.OrderBy(x => x.userID).Where(x => x.businessSubategoryID == altKategoriID && x.admin == false && x.active == true).Take(pageSize).ToList();
                    }
                    else
                    {
                        int pageIndex = pageSize * page.Value;
                        ViewBag.userList1 = db.Users.OrderBy(x => x.userID).Where(x => x.businessSubategoryID == altKategoriID && x.admin == false && x.active == true).Skip(pageIndex).Take(pageSize).ToList();
                    }
                    if (Request.IsAjaxRequest() && kelimeArama != true)
                    {
                        return PartialView("_UserList", ViewBag.userList1);
                    }
                }
                else
                {
                    // puana göre sıralanma işlemlerinde parametre olarak liste alacak ve ona göre işlem yapacak
                    if (siralamaSekli.Equals("puanaGoreArtan"))
                    {
                        ViewBag.siralamaSekli = "puanaGoreArtan";

                        if (page == null)
                        {
                            ViewBag.userList1 = db.Users.OrderBy(x => x.score).Where(x => x.businessSubategoryID == altKategoriID && x.admin == false && x.active == true).Take(pageSize).ToList();
                        }
                        else
                        {
                            int pageIndex = pageSize * page.Value;
                            ViewBag.userList1 = db.Users.OrderBy(x => x.score).Where(x => x.businessSubategoryID == altKategoriID && x.admin == false && x.active == true).Skip(pageIndex).Take(pageSize).ToList();
                        }
                        if (Request.IsAjaxRequest())
                        {
                            return PartialView("_UserList", ViewBag.userList1);
                        }
                    }
                    else if (siralamaSekli.Equals("puanaGoreAzalan"))
                    {
                        ViewBag.siralamaSekli = "puanaGoreAzalan";

                        if (page == null)
                        {
                            ViewBag.userList1 = db.Users.OrderByDescending(x => x.score).Where(x => x.businessSubategoryID == altKategoriID && x.admin == false && x.active == true).Take(pageSize).ToList();
                        }
                        else
                        {
                            int pageIndex = pageSize * page.Value;
                            ViewBag.userList1 = db.Users.OrderByDescending(x => x.score).Where(x => x.businessSubategoryID == altKategoriID && x.admin == false && x.active == true).Skip(pageIndex).Take(pageSize).ToList();
                        }
                        if (Request.IsAjaxRequest())
                        {
                            return PartialView("_UserList", ViewBag.userList1);
                        }
                    }
                    else if (siralamaSekli.Equals("deneyimeGoreArtan"))
                    {
                        ViewBag.siralamaSekli = "deneyimeGoreArtan";

                        if (page == null)
                        {
                            ViewBag.userList1 = db.Users.OrderBy(x => x.experience).Where(x => x.businessSubategoryID == altKategoriID && x.admin == false && x.active == true).Take(pageSize).ToList();
                        }
                        else
                        {
                            int pageIndex = pageSize * page.Value;
                            ViewBag.userList1 = db.Users.OrderBy(x => x.experience).Where(x => x.businessSubategoryID == altKategoriID && x.admin == false && x.active == true).Skip(pageIndex).Take(pageSize).ToList();
                        }
                        if (Request.IsAjaxRequest())
                        {
                            return PartialView("_UserList", ViewBag.userList1);
                        }
                    }
                    else if (siralamaSekli.Equals("deneyimeGoreAzalan"))
                    {
                        ViewBag.siralamaSekli = "deneyimeGoreAzalan";

                        if (page == null)
                        {
                            ViewBag.userList1 = db.Users.OrderByDescending(x => x.experience).Where(x => x.businessSubategoryID == altKategoriID && x.admin == false && x.active == true).Take(pageSize).ToList();
                        }
                        else
                        {
                            int pageIndex = pageSize * page.Value;
                            ViewBag.userList1 = db.Users.OrderByDescending(x => x.experience).Where(x => x.businessSubategoryID == altKategoriID && x.admin == false && x.active == true).Skip(pageIndex).Take(pageSize).ToList();
                        }
                        if (Request.IsAjaxRequest())
                        {
                            return PartialView("_UserList", ViewBag.userList1);
                        }
                    }
                }
            }
            else
            {
                bool ilceCheck = false, deneyimCheck = false, puanCheck = false, cinsiyetCheck = false;
                List<Users> tempUserList = new List<Users>();
                string[] userIDList = null, userDeneyimList = null, userPuanlamaList = null, userCinsiyetList = null;
                if (userList != null)
                {
                    ilceCheck = true;
                    userIDList = userList.Split(',');
                    foreach (var item in db.Users.ToList())
                    {
                        for (int i = 0; i < userIDList.Length; i++)
                        {
                            if (item.userID == Convert.ToInt32(userIDList[i].ToString()))
                            {
                                tempUserList.Add(item);
                            }
                        }
                    }
                }
                if (deneyimListesi != null)
                {
                    deneyimCheck = true;
                    List<Users> tempListe = new List<Users>();
                    if (tempUserList.Count != 0)
                        tempListe = tempUserList.ToList();
                    else if (ilceCheck == false)
                        tempListe = db.Users.ToList();

                    tempUserList.Clear();
                    userDeneyimList = deneyimListesi.Split(',');
                    foreach (var item in tempListe)
                    {
                        for (int i = 0; i < userDeneyimList.Length; i++)
                        {
                            if (item.userID == Convert.ToInt32(userDeneyimList[i].ToString()))
                            {
                                tempUserList.Add(item);
                            }
                        }
                    }
                }

                if (puanlama != null)
                {
                    puanCheck = true;
                    List<Users> tempListe = new List<Users>();
                    if (tempUserList.Count != 0)
                        tempListe = tempUserList.ToList();
                    else if (ilceCheck == false && deneyimCheck == false)
                        tempListe = db.Users.ToList();

                    tempUserList.Clear();
                    userPuanlamaList = puanlama.Split(',');
                    foreach (var item in tempListe)
                    {
                        for (int i = 0; i < userPuanlamaList.Length; i++)
                        {
                            if (item.userID == Convert.ToInt32(userPuanlamaList[i].ToString()))
                            {
                                tempUserList.Add(item);
                            }
                        }
                    }
                }

                if (cinsiyetListesi != null)
                {
                    cinsiyetCheck = true;
                    List<Users> tempListe = new List<Users>();
                    if (tempUserList.Count != 0)
                        tempListe = tempUserList.ToList();
                    else if (ilceCheck == false && deneyimCheck == false && puanCheck == false)
                        tempListe = db.Users.ToList();

                    tempUserList.Clear();
                    userCinsiyetList = cinsiyetListesi.Split(',');
                    foreach (var item in tempListe)
                    {
                        for (int i = 0; i < userCinsiyetList.Length; i++)
                        {
                            if (item.userID == Convert.ToInt32(userCinsiyetList[i].ToString()))
                            {
                                tempUserList.Add(item);
                            }
                        }
                    }
                }

                //ViewBag.userIDList = userList;
                ViewBag.userList1 = tempUserList;
            }
            return View("ListUser", db.AboutUs.FirstOrDefault());
        }

        [HttpPost]
        public ActionResult filtrele(int[] ilce, int? altKategoriID, int[] deneyim, int[] puanlama, int[] cinsiyet)
        {
            string userListesi = null;
            string deneyimListesi = null;
            string puanlamaListesi = null;
            string cinsiyetListesi = null;

            //ViewBag.userListesi= userListesi;
            //Users tempUser;
            if (ilce != null)
            {
                foreach (var item in ilce)
                {
                    if (userListesi == null)
                    {
                        foreach (var user in db.Users.ToList())
                        {
                            if (user.city == item.ToString())
                            {
                                if (userListesi == null)
                                    userListesi = user.userID.ToString();
                                else
                                    userListesi = userListesi + "," + user.userID.ToString();
                            }
                        }
                    }
                    else
                    {
                        foreach (var user in db.Users.ToList())
                        {
                            if (user.city == item.ToString())
                            {
                                userListesi = userListesi + "," + user.userID.ToString();
                            }
                        }
                    }
                }
            }
            if (deneyim != null)
            {
                foreach (var item in deneyim)
                {
                    if (deneyimListesi == null)
                    {
                        foreach (var user in db.Users.ToList())
                        {
                            if (user.experience == item)
                            {
                                if (deneyimListesi == null)
                                    deneyimListesi = user.userID.ToString();
                                else
                                    deneyimListesi = deneyimListesi + "," + user.userID.ToString();
                            }
                        }
                    }
                    else
                    {
                        foreach (var user in db.Users.ToList())
                        {
                            if (user.experience == item)
                            {
                                deneyimListesi = deneyimListesi + "," + user.userID.ToString();
                            }
                        }
                    }
                }
            }

            if (puanlama != null)
            {
                List<Users> tempUserIDlist = new List<Users>();
                int taban, tavan;
                foreach (var item in puanlama)
                {
                    if (item == 11)
                    {
                        taban = 0;
                        tavan = 1;
                    }
                    else
                    {
                        taban = Convert.ToInt32(Convert.ToString(item).Substring(0, 1));
                        tavan = Convert.ToInt32(Convert.ToString(item).Substring(1, 1));
                    }
                    if (tavan != 5)
                        tempUserIDlist = tempUserIDlist.Concat(db.Users.Where(x => x.score >= taban && x.score < tavan && x.steward == true && x.active == true).ToList()).ToList();
                    else
                        tempUserIDlist = tempUserIDlist.Concat(db.Users.Where(x => x.score >= taban && x.score <= tavan && x.steward == true && x.active == true).ToList()).ToList();

                }
                foreach (var item in tempUserIDlist)
                {
                    if (puanlamaListesi == null)
                        puanlamaListesi = item.userID.ToString();
                    else
                        puanlamaListesi = puanlamaListesi + "," + item.userID.ToString();
                }
            }

            if (cinsiyet != null)
            {
                List<Users> tempUserIDlist = new List<Users>();

                foreach (var item in cinsiyet)
                {
                    if (item == -1) // kadın demek
                        tempUserIDlist = tempUserIDlist.Concat(db.Users.Where(x => x.gender == "Kadın" && x.steward == true && x.active == true).ToList()).ToList();
                    else if (item == -2) // erkek demek
                        tempUserIDlist = tempUserIDlist.Concat(db.Users.Where(x => x.gender == "Erkek" && x.steward == true && x.active == true).ToList()).ToList();
                }

                foreach (var item in tempUserIDlist)
                {
                    if (cinsiyetListesi == null)
                        cinsiyetListesi = item.userID.ToString();
                    else
                        cinsiyetListesi = cinsiyetListesi + "," + item.userID.ToString();
                }
            }
            return RedirectToAction("ListUser", new { altKategoriID = altKategoriID, userList = userListesi, deneyimListesi = deneyimListesi, puanlama = puanlamaListesi, cinsiyetListesi = cinsiyetListesi });
        }

        public ActionResult kullaniciEngelle(int engelleyen, int engellenen)
        {
            if (ModelState.IsValid)
            {
                var temp = db.blocked.Where(a => a.blockerID == engelleyen && a.blockedID == engellenen).FirstOrDefault();
                if (temp == null)
                {
                    blocked block = new blocked();
                    block.blockerID = engelleyen;
                    block.blockedID = engellenen;
                    block.date = DateTime.Now.ToUniversalTime();
                    db.blocked.Add(block);
                    db.SaveChanges();
                    return RedirectToAction("KullaniciDetayi", "Home", new { id = engellenen });
                }
                else
                {
                    db.blocked.Remove(temp);
                    db.SaveChanges();
                    return RedirectToAction("KullaniciDetayi", "Home", new { id = engellenen });
                }
            }
            else
                return RedirectToAction("Index", "Home");
            return View(db.AboutUs.FirstOrDefault());
        }


        public ActionResult KullaniciDetayi(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users tempuser = db.Users.Find(id);
            ViewBag.User = tempuser;

            if (ViewBag.User == null)
            {
                return HttpNotFound();
            }
            if (tempuser.steward == true)
            {
                var tempID = Convert.ToInt32(tempuser.city);
                ViewBag.sehir = db.City.Where(x => x.ID == tempID).FirstOrDefault().City1;
                tempID = Convert.ToInt32(tempuser.district);
                ViewBag.ilce = db.District.Where(x => x.ID == tempID).FirstOrDefault().District1;

                ViewBag.KullaniciYorumlari = db.Yorumlar.Where(x => x.yorumYapilan == id).ToList();
                ViewBag.Kullanicilar = db.Users.ToList();
            }
            ViewBag.blockList = db.blocked.ToList();
            ViewBag.Users = db.Users.ToList();

            return View(db.AboutUs.FirstOrDefault());
        }


        public ActionResult yorumYap(int yorumYapilan, int yorumYapan, string yorumMetni, int puan)
        {
            if (yorumYapan == yorumYapilan)
            {
                Response.Write("<script LANGUAGE='JavaScript' >alert('Request Submitted Successfully!');</script>");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    Puanlar puanlar = new Puanlar();
                    Yorumlar yorumlar = new Yorumlar();
                    yorumlar.yorumYapan = yorumYapan;
                    yorumlar.yorumYapilan = yorumYapilan;
                    yorumlar.yorumMetni = yorumMetni;
                    yorumlar.yorumTarihi = DateTime.Now;
                    puanlar.puanVeren = yorumYapan;
                    puanlar.puanVerilen = yorumYapilan;
                    puanlar.Tarih = DateTime.Now;
                    puanlar.Puan = puan;
                    db.Puanlar.Add(puanlar);
                    db.Yorumlar.Add(yorumlar);
                    db.SaveChanges();

                    var user = db.Users.Find(yorumlar.yorumYapilan);
                    if (user.numberOfComments == 0 || user.numberOfComments == null)
                        user.numberOfComments = 1;
                    else
                        user.numberOfComments += 1;
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();

                    var user_ = db.Users.Find(puanlar.puanVerilen);
                    var toplamPuanSayisiList = db.Puanlar.Where(a => a.puanVerilen == puanlar.puanVerilen).ToList();
                    int totalScore = 0;
                    for (int i = 0; i < toplamPuanSayisiList.Count; i++)
                    {
                        totalScore += toplamPuanSayisiList[i].Puan;
                    }
                    user_.score = (Convert.ToDouble(totalScore) / Convert.ToDouble(toplamPuanSayisiList.Count));
                    db.Entry(user_).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("KullaniciDetayi", "Home", new { id = yorumYapilan });
        }


        public ActionResult girisYap(string eposta, string parola, int? hizmetVerenID)
        {
            if (eposta != null || parola != null)
            {
                var user = db.Users.Where(x => x.eMail == eposta).FirstOrDefault();

                if (user != null)
                    Session["userID"] = user.userID;
                else
                    return new HttpStatusCodeResult(410, "E-Posta veya Parola Hatali.!");
            }
            else
                return new HttpStatusCodeResult(410, "E-Posta vey Parola boş geçilemez.!");
            if (hizmetVerenID != null)
                return RedirectToAction("KullaniciDetayi", "Home", new { id = hizmetVerenID });

            return RedirectToAction("Index", "Home");
        }

        public ActionResult mesajAt(int mesajAtan, int mesajAtilan, string mesaj)
        {
            if (mesaj != null)
            {
                messages mesajKaydet = new messages();
                mesajKaydet.message = mesaj;
                mesajKaydet.senderID = mesajAtan;
                mesajKaydet.receiverID = mesajAtilan;
                mesajKaydet.date = DateTime.Now;
                try
                {
                    db.messages.Add(mesajKaydet);

                    db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {

                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Response.Write(string.Format("Entity türü \"{0}\" şu hatalara sahip \"{1}\" Geçerlilik hataları:", eve.Entry.Entity.GetType().Name, eve.Entry.State));
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Response.Write(string.Format("- Özellik: \"{0}\", Hata: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                        }
                        Response.End();
                        // return new HttpStatusCodeResult(410, "Islem Basarisiz. Lutfen tekrar deneyin.!");
                    }
                }
            }
            return RedirectToAction("KullaniciDetayi", "Home", new { id = mesajAtilan });
        }


        public ActionResult uyeOl(string eposta, string parola, string parolaTekrar, string username, string name, string surname)
        {
            if (eposta != null || parola != null || parolaTekrar != null || username != null || name != null || surname != null)
            {
                Users usernameVarmi = db.Users.Where(a => a.username == username).FirstOrDefault();
                Users epostaVarmi = db.Users.Where(a => a.eMail == eposta).FirstOrDefault();

                if (usernameVarmi != null || epostaVarmi != null)
                {
                    return new HttpStatusCodeResult(410, "E-Posta veya Kullanici Adi Sistemde Mevcut.!");
                }

                if (parola != parolaTekrar)
                {
                    return new HttpStatusCodeResult(410, "Girdiginiz parolalar birbiriyle uyusmuyor.!");
                }
                //,totalScore,score,numberOfComments,numberOfViews,gender,experience,admin
                Users user = new Users();
                user.businessCategoryID = null;
                user.businessSubategoryID = null;
                user.country = null;
                user.city = null;
                user.district = null;
                user.birthday = null;
                user.phone = null;
                user.description = null;
                user.votingNumber = null;
                user.totalScore = null;
                user.score = null;
                user.numberOfComments = null;
                user.numberOfViews = null;
                user.gender = null;
                user.experience = null;
                user.admin = false;
                user.active = true;
                user.username = username;
                user.eMail = eposta;
                user.password = parola;
                user.name = name;
                user.surname = surname;
                user.steward = false;
                byte[] userImage = System.IO.File.ReadAllBytes(Server.MapPath(@"~\images\search-people.png"));
                user.picture = userImage;

                db.Users.Add(user);
                try
                {
                    db.SaveChanges();
                    return new HttpStatusCodeResult(200, "Islem Basarili..");
                }
                catch (DbEntityValidationException e)
                {

                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Response.Write(string.Format("Entity türü \"{0}\" şu hatalara sahip \"{1}\" Geçerlilik hataları:", eve.Entry.Entity.GetType().Name, eve.Entry.State));
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Response.Write(string.Format("- Özellik: \"{0}\", Hata: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                        }
                        Response.End();
                        return new HttpStatusCodeResult(410, "Islem Basarisiz. Lutfen tekrar deneyin.!");
                    }
                }
            }
            else
                return new HttpStatusCodeResult(410, "Lutfen tum alanlari doldurunuz.!");
            return View();
        }

        [ControlUserLogin]
        public ActionResult hizmetVerenOl(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            else if (Session["userID"].ToString() != Convert.ToString(id))
                return RedirectToAction("Index", "Home");

            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            if (users.steward == true)
                return RedirectToAction("Index", "Home");

            ViewBag.Kategori = db.businessCategory.ToList();
            ViewBag.Sehir = db.City.ToList();
            ViewBag.user = users;
            ViewBag.Users = db.Users.ToList();

            //ViewBag.businessCategoryID = new SelectList(db.businessCategory, "businessCategoryID", "businessCategory1", users.businessCategoryID);
            //ViewBag.businessSubategoryID = new SelectList(db.businessSubategory, "businessSubategoryID", "businessSubategory1", users.businessSubategoryID);
            return View(db.AboutUs.FirstOrDefault());
        }

        public ActionResult hizmetVerenOl2(int userID, int businessCategoryID, int businessSubategoryID,
                                      string city, string district, DateTime? birthday, string phone, string description,
                                      string gender, int? experience)
        {
            var user = db.Users.Find(userID);
            if (businessCategoryID == 0 || businessSubategoryID == 0)
                return new HttpStatusCodeResult(200, "Kategori veya alt kategori boş geçilemez..");
            else
            {
                //if (fileup != null)
                //{
                //    user.picture = new byte[fileup.ContentLength];
                //    fileup.InputStream.Read(user.picture, 0, fileup.ContentLength);
                //}


                if ((city != null && district != null
                && birthday != null && description != null && gender != null && experience != null))
                {
                    if (ModelState.IsValid)
                    {
                        user.businessCategoryID = businessCategoryID;
                        user.businessSubategoryID = businessSubategoryID;
                        user.city = city;
                        user.district = district;
                        user.birthday = birthday;
                        user.description = description;
                        user.phone = phone;
                        user.gender = gender;
                        user.experience = experience;
                        user.steward = true;
                        user.admin = false;
                        user.votingNumber = 0;
                        user.totalScore = 0;
                        user.score = 0;
                        user.numberOfComments = 0;
                        user.numberOfViews = 0;

                        db.Entry(user).State = EntityState.Modified;
                        try
                        {
                            db.SaveChanges();
                            return new HttpStatusCodeResult(200, "Islem Basarili..");
                        }
                        catch (DbEntityValidationException e)
                        {
                            foreach (var eve in e.EntityValidationErrors)
                            {
                                Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                    eve.Entry.Entity.GetType().Name, eve.Entry.State);
                                foreach (var ve in eve.ValidationErrors)
                                {
                                    Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                        ve.PropertyName, ve.ErrorMessage);
                                }
                            }
                        }
                        return View();
                    }
                }
                else
                    return new HttpStatusCodeResult(410, "Lütfen tüm alanlaı doldurunuz..");
            }
            ViewBag.Kategori = db.businessCategory.ToList();
            ViewBag.Sehir = db.City.ToList();
            return View(user);

        }


        [ControlUserLogin]
        [HttpPost]
        public ActionResult hizmetVerenResimEkle(int id, HttpPostedFileBase fileup)
        {
            var user = db.Users.Find(id);

            if (fileup != null)
            {
                user.picture = new byte[fileup.ContentLength];
                fileup.InputStream.Read(user.picture, 0, fileup.ContentLength);
            }
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public JsonResult GetirKategori(string kategoriId)
        {
            Araniyor entities = new Araniyor();

            int kategoriID = Convert.ToInt32(kategoriId);

            var altKategoriler = entities.businessSubategory.Where(x => x.businessCategoryID == kategoriID).Select(a => new { Id = a.businessSubategoryID, Ad = a.businessSubategory1 }).ToList();
            return Json(altKategoriler);
        }

        [HttpPost]
        public JsonResult GetirIlceler(string ilId)
        {
            Araniyor entities = new Araniyor();
            int ilID = 0;
            if (!string.IsNullOrEmpty(ilId))
                ilID = Convert.ToInt32(ilId);

            var ilceler = entities.District.Where(ilce => ilce.CityID == ilID).Select(ilce => new { Id = ilce.ID, Ad = ilce.District1 }).ToList();
            return Json(ilceler);
        }


        [ControlUserLogin]
        public ActionResult profilDüzenle(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            else if (Session["userID"].ToString() != Convert.ToString(id))
                return RedirectToAction("Index", "Home");

            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            ViewBag.Kategori = db.businessCategory.ToList();
            ViewBag.Sehir = db.City.ToList();
            ViewBag.user = users;
            ViewBag.Users = db.Users.ToList();

            return View(db.AboutUs.FirstOrDefault());
        }

        public ActionResult profilDüzenle2(int userID, int businessCategoryID, int businessSubategoryID,
                                     string city, string district, DateTime? birthday, string phone, string description,
                                     string gender, int? experience, string eposta, string parola, string parolaTekrar, string username, string name, string surname)
        {
            var tempUser = db.Users.Where(x => x.username == username).FirstOrDefault();
            var tempUser2 = db.Users.Where(x => x.eMail == eposta).FirstOrDefault();

            if (tempUser != null || tempUser2 != null)
            {
                var user = db.Users.Find(userID);

                user.businessCategoryID = businessCategoryID;
                user.businessSubategoryID = businessSubategoryID;
                user.city = city;
                user.district = district;
                user.birthday = birthday;
                user.phone = phone;
                user.description = description;
                user.gender = gender;
                user.experience = experience;
                user.eMail = eposta;
                if (parola != null && parolaTekrar != null)
                {
                    if (parola == parolaTekrar)
                        user.password = parola;
                    else
                        return new HttpStatusCodeResult(410, "Parola Eşleşmiyor..");
                }
                else
                    user.password = user.password;
                user.username = username;
                user.name = name;
                user.surname = surname;

                db.Entry(user).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                    return new HttpStatusCodeResult(200, "Islem Basarili..");
                }
                catch (DbEntityValidationException e)
                {

                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Response.Write(string.Format("Entity türü \"{0}\" şu hatalara sahip \"{1}\" Geçerlilik hataları:", eve.Entry.Entity.GetType().Name, eve.Entry.State));
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Response.Write(string.Format("- Özellik: \"{0}\", Hata: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                        }
                        Response.End();
                        return new HttpStatusCodeResult(410, "Islem Basarisiz. Lutfen tekrar deneyin.!");
                    }
                }
            }
            else
                return new HttpStatusCodeResult(410, "E-Posta veya Kullanıcı adı sistemde bulunmakta.!");

            return View();
        }


        public ActionResult profilDüzenleNormalKullanici(int userID, string eposta, string parola, string parolaTekrar, string username, string name, string surname)
        {
            var tempUser = db.Users.Where(x => x.username == username).FirstOrDefault();
            var tempUser2 = db.Users.Where(x => x.eMail == eposta).FirstOrDefault();

            if (tempUser != null || tempUser2 != null)
            {
                var user = db.Users.Find(userID);

                user.eMail = eposta;
                if (parola != null && parolaTekrar != null)
                {
                    if (parola == parolaTekrar)
                        user.password = parola;
                    else
                        return new HttpStatusCodeResult(410, "Parola Eşleşmiyor..");
                }
                else
                    user.password = user.password;
                user.username = username;
                user.name = name;
                user.surname = surname;

                db.Entry(user).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                    return RedirectToAction("Index", "Home");

                }
                catch (DbEntityValidationException e)
                {

                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Response.Write(string.Format("Entity türü \"{0}\" şu hatalara sahip \"{1}\" Geçerlilik hataları:", eve.Entry.Entity.GetType().Name, eve.Entry.State));
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Response.Write(string.Format("- Özellik: \"{0}\", Hata: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                        }
                        Response.End();
                        return new HttpStatusCodeResult(410, "Islem Basarisiz. Lutfen tekrar deneyin.!");
                    }
                }
            }
            else
                return new HttpStatusCodeResult(410, "E-Posta veya Kullanıcı adı sistemde bulunmakta.!");

            return View();
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
            return RedirectToAction("profilDüzenle", "Home", new { id = id });
        }

        [ControlUserLogin]
        public ActionResult Mesajlasma(int? receiverID)
        {
            ViewBag.blockList = db.blocked.ToList();
            if (receiverID != null)
                ViewBag.ilkYüklenme = false;
            else
                ViewBag.ilkYüklenme = true;

            // ViewBag.Date = DateTime.Now;
            int senderID = 0;
            try
            {
                senderID = Convert.ToInt32(Session["userID"].ToString());
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", "Home");
            }

            #region receiverID boş geldiğinde 
            var receiverIDList = db.messages.Where(x => x.senderID == senderID).ToList().Select(y => y.receiverID).Distinct();
            var senderIDList = db.messages.Where(x => x.receiverID == senderID).ToList().Select(y => y.senderID).Distinct();
            var kisiler = receiverIDList.Union(senderIDList).ToList();

            List<Users> KisilerList = new List<Users>();

            foreach (var kisi in kisiler)
            {
                foreach (var user in db.Users.ToList())
                {
                    if (user.userID == kisi)
                    {
                        KisilerList.Add(user);
                    }
                }
            }

            ViewBag.Kisiler = KisilerList;
            #endregion

            #region receiver ID dolu geldiğinde
            ViewBag.mesajlar = db.messages.OrderBy(x => x.date).Where(x => (x.senderID == senderID && x.receiverID == receiverID) || (x.senderID == receiverID && x.receiverID == senderID)).ToList();
            ViewBag.receiverUser = db.Users.Find(receiverID);
            ViewBag.senderUser = db.Users.Find(senderID);

            ViewBag.base64ResimSender = Convert.ToBase64String(ViewBag.senderUser.picture);
            if (ViewBag.receiverUser != null)
                ViewBag.base64ResimReceiver = Convert.ToBase64String(ViewBag.receiverUser.picture);
            #endregion

            ViewBag.Users = db.Users.ToList();
            return View(db.AboutUs.FirstOrDefault());
        }
        [HttpPost]
        public ActionResult GetSearchValue(string term, string altKategori)
        {
            var subCategory = db.businessSubategory.Where(x => x.businessSubategory1.Contains(term))
                .Select(y => new altKategoriDTO
                {
                    businessSubategory1 = y.businessSubategory1,
                    businessSubategoryID = y.businessSubategoryID
                }).ToList();
            return Json(subCategory, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult altKategoriAra(string altKategori)
        {
            var altKategoriID = db.businessSubategory.Where(x => x.businessSubategory1 == altKategori).FirstOrDefault().businessSubategoryID;
            return Json(Url.Action("ListUser", "Home", new {
                altKategoriID= altKategoriID,
                ilkListeleme=true,
                kelimeArama=true
            }));
        }

        public void mesajGonder(string mesaj, int receiverID, int senderID)
        {
            ViewBag.blockList = db.blocked.ToList();
            if (mesaj != null)
            {
                messages mesajKaydet = new messages();
                mesajKaydet.message = mesaj;
                mesajKaydet.senderID = senderID;
                mesajKaydet.receiverID = receiverID;
                mesajKaydet.date = DateTime.Now;
                try
                {
                    db.messages.Add(mesajKaydet);

                    db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {

                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Response.Write(string.Format("Entity türü \"{0}\" şu hatalara sahip \"{1}\" Geçerlilik hataları:", eve.Entry.Entity.GetType().Name, eve.Entry.State));
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Response.Write(string.Format("- Özellik: \"{0}\", Hata: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                        }
                        Response.End();
                       // return new HttpStatusCodeResult(410, "Islem Basarisiz. Lutfen tekrar deneyin.!");
                    }
                }
            }
        }

        public ActionResult cikisYap()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
    public class altKategoriDTO
    {
        public int businessSubategoryID { get; set; }
        public string businessSubategory1 { get; set; }
    }
}
