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
        public JsonResult Index()
        {
            return JsonError("No default action");
        }

        /*
         * Task actions
         */

        [HttpGet]
        [ActionName("Task")]
        public JsonResult GetTask(int? taskID)
        {
            var api = new ApiModel();

            if(taskID.HasValue)
            {
                if (api.GetTask(taskID.Value) != null)
                {
                    return Json(api.GetTask(taskID.Value), JsonRequestBehavior.AllowGet);
                }
            }
            else if(api.GetTasks() != null)
            {
                return Json(api.GetTasks(), JsonRequestBehavior.AllowGet);
            }

            return JsonError("Invalid request");
        }

        /*
         * Assignment actions
         */

        [HttpGet]
        [ActionName("Assignment")]
        public JsonResult GetAssignment(int? taskID, int? userID)
        {
            var api = new ApiModel();
            if(taskID.HasValue && userID.HasValue)
            {
                if (api.GetAssignment(taskID.Value, userID.Value) != null)
                {
                    return Json(api.GetAssignment(taskID.Value, userID.Value), JsonRequestBehavior.AllowGet);
                }
            }
            else if(taskID.HasValue)
            {
                if (api.GetAssignments(taskID.Value) != null)
                {
                    return Json(api.GetAssignments(taskID.Value), JsonRequestBehavior.AllowGet);
                }
            }

            return JsonError("Invalid request");
        }

        [HttpPost]
        [ActionName("Assignment")]
        public JsonResult CreateAssignment(int? taskID, int? userID)
        {
            if (taskID != null && userID != null && new ApiModel().CreateAssignment(taskID.Value, userID.Value))
            {
                return JsonSuccess("Assignment created successfully");
            }

            return JsonError("Invalid request");
        }

        [HttpDelete]
        [ActionName("Assignment")]
        public JsonResult DeleteAssignment(int? taskID, int? userID)
        {
            if(taskID != null && userID != null)
            {
                return Json(new ApiModel().DeleteAssignment(taskID.Value, userID.Value));
            }

            return JsonError("Invalid request");
        }

        /*
         * Helper methods for JSON responses
         */

        private JsonResult JsonError(string error)
        {
            return new JsonResult {
                Data = new { Error = error },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        private JsonResult JsonSuccess(string success)
        {
            return new JsonResult
            {
                Data = new { Success = success },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }
}