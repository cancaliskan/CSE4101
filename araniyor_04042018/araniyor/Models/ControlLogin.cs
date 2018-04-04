using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace araniyor.Models
{
    public class ControlLogin : ActionFilterAttribute, IActionFilter
    {
        private Araniyor db = new Araniyor();
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                if (!string.IsNullOrEmpty(HttpContext.Current.Session["ID"].ToString()))
                {
                    var username = HttpContext.Current.Session["ID"].ToString();
                    var user = db.Users.Where(a => a.username.Equals(username)).FirstOrDefault();
                    base.OnActionExecuting(filterContext);
                }
                else
                {
                    HttpContext.Current.Response.Redirect("/Admin/Index");
                }
            }
            catch (Exception)
            {
                HttpContext.Current.Response.Redirect("/Admin/Index");
            }

        }
    }
}