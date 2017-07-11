using System.Linq;
using System.Web.Mvc;

namespace Server.Models
{
    public class ApiModel
    {
        public JsonResult GetTask(int id)
        {
            var db = new DatabaseModel();
            var json = new JsonResult();
            var task = db.Tasks
                .Where(x => x.TaskID == id)
                .Select(x => x);

            if (task.Count() == 1)
            {
                json.Data = task;
            }
            else
            {
                json.Data = new { Error = "Couldn't find the specified task" };
            }

            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return json;
        }
    }
}