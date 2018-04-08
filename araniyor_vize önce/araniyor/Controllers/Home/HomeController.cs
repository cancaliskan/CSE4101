using araniyor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

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

            return View(aboutUs);
        }
        public ActionResult Hakkimizda()
        {
            AboutUs aboutUs = db.AboutUs.FirstOrDefault();
            string aboutUsHakkimizda = db.AboutUs.FirstOrDefault().Hakkimizda;
            ViewBag.Hakkimizda = MvcHtmlString.Create(aboutUsHakkimizda.Replace(Environment.NewLine, "<br/>"));
            return View(aboutUs);
        }
        public ActionResult Misyon()
        {
            AboutUs aboutUs = db.AboutUs.FirstOrDefault();
            string aboutUsMisyon = db.AboutUs.FirstOrDefault().Misyon;
            ViewBag.Misyon = MvcHtmlString.Create(aboutUsMisyon.Replace(Environment.NewLine, "<br/>"));
            return View(aboutUs);
        }
        public ActionResult Vizyon()
        {
            AboutUs aboutUs = db.AboutUs.FirstOrDefault();
            string aboutUsVizyon = db.AboutUs.FirstOrDefault().Vizyon;
            ViewBag.Vizyon = MvcHtmlString.Create(aboutUsVizyon.Replace(Environment.NewLine, "<br/>"));
            return View(aboutUs);
        }

        public ActionResult Kategoriler()
        {
            AboutUs aboutUs = db.AboutUs.FirstOrDefault();

            ViewBag.Kategori = db.businessCategory.ToList();
            ViewBag.AltKategori = db.businessSubategory.ToList();
            return View(aboutUs);
        }



        int pageSize = 9;
        public ActionResult ListUser(int? altKategoriID, int? page, List<Users> userList)
        {
            Thread.Sleep(500);
            ViewBag.altKategoriID = altKategoriID;
            AboutUs aboutUs = db.AboutUs.FirstOrDefault();
            ViewBag.userList1 = null;

            if (userList == null)
            {
                if (page == null)
                {
                    ViewBag.userList1 = db.Users.OrderBy(x => x.userID).Where(x => x.businessSubategoryID == altKategoriID && x.admin == false && x.active == true).Take(pageSize).ToList();
                }
                else
                {
                    int pageIndex = pageSize * page.Value;
                    ViewBag.userList1 = db.Users.OrderBy(x => x.userID).Where(x => x.businessSubategoryID == altKategoriID && x.admin == false && x.active == true).Skip(pageIndex).Take(pageSize).ToList();
                }
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_UserList", ViewBag.userList1);
                }
                return View(aboutUs);
            }
            else
            {
                // puana göre sıralanma işlemlerinde parametre olarak liste alacak ve ona göre işlem yapacak
                return View(aboutUs);
            }
        }



    }
}
