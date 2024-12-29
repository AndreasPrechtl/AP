using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AP.Website.Controllers
{
    public class HomeController : Controller
    {
        //protected override void HandleUnknownAction(string actionName)
        //{
        //    this.View(actionName).ExecuteResult(this.ControllerContext);
        //}
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Imprint()
        {
            return View();
        }
        public ActionResult Technologies()
        {
            return View();
        }
        //public ActionResult About()
        //{
        //    return View();
        //}

        public ActionResult References()
        {
            return View();
        }
        
        // ---- those will be added later on ----
        // ---- but for now a simple imprint will do ----


        //public ActionResult Contact()
        //{
        //    return View();
        //}

    }
}
