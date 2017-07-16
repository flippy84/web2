using System;
using System.Linq;
using System.Web.Mvc;

namespace Server.Models
{
    public class ApiModel
    {
        public Task GetTask(int taskID)
        {
            var db = new DatabaseModel();
            var task = db.Tasks
                .Where(x => x.TaskID == taskID)
                .Select(x => x);

            if (task.Count() == 1)
            {
                return task.Single();
            }

            return null;
        }

        public Task[] GetTasks()
        {
            var db = new DatabaseModel();
            var tasks = db.Tasks
                .Select(x => x);

            if(tasks.Count() > 0)
                return tasks.ToArray();

            return null;
        }

        public Assignment GetAssignment(int taskID, int userID)
        {
            var db = new DatabaseModel();
            var assignment = db.Assignments
                .Where(x => x.TaskID == taskID && x.UserID == userID)
                .Select(x => x)
                .SingleOrDefault();

            return assignment;
        }

        public Assignment[] GetAssignments(int taskID)
        {
            var db = new DatabaseModel();
            var assignments = db.Assignments
                .Where(x => x.TaskID == taskID)
                .Select(x => x);

            if(assignments.Count() > 0)
                return assignments.ToArray();

            return null;
        }

        public bool DeleteAssignment(int taskID, int userID)
        {
            var db = new DatabaseModel();
            var assignment = db.Assignments
                .Where(x => x.TaskID == taskID && x.UserID == userID)
                .Select(x => x);

            if (assignment.Count() == 1)
            {
                db.Assignments.DeleteOnSubmit(assignment.Single());
                db.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool CreateAssignment(int taskID, int userID)
        {
            var db = new DatabaseModel();
            var assignment = new Assignment
            {
                TaskID = taskID,
                UserID = userID
            };

            try
            {
                db.Assignments.InsertOnSubmit(assignment);
                db.SubmitChanges();
            }
            catch
            {
                return false;
            }

            return true;
        }

        public User[] GetUsers()
        {
            var db = new DatabaseModel();
            var users = db.Users
                .Select(x => x);

            if (users.Count() > 0)
                return users.ToArray();

            return null;
        }

        public User GetUser(int userID)
        {
            var db = new DatabaseModel();
            var user = db.Users
                .Where(x => x.UserID == userID)
                .Select(x => x)
                .SingleOrDefault();

            return user;
        }
    }
}