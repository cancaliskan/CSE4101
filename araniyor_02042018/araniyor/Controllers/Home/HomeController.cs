using araniyor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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


    }
}
