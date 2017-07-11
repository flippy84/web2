using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Server.Controllers
{
    public class ApiController : Controller
    {
        // GET: Api
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [ActionName("Task")]
        public JsonResult GetTask(int ?id)
        {
            if(id != null)
            {
                return new ApiModel().GetTask(id.Value);
            }

            return Json(new { Error = "No id specified" }, JsonRequestBehavior.AllowGet);
        }

        [HttpDelete]
        [ActionName("Task")]
        public JsonResult DeleteTask(int? id)
        {
            return Json(1);
        }

        [HttpGet]
        [ActionName("Test")]
        public JsonResult TestGet()
        {
            return Json("hej");
        }

        public ActionResult Test2()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Task")]
        public JsonResult PostTask(int? id)
        {
            return Json(1);
        }
    }
}