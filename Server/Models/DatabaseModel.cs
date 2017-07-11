using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace Server.Models
{
    public class DatabaseModel : DataContext
    {
        public DatabaseModel() : base(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database.mdf;Integrated Security=True") { }

        public Table<Assignment> Assignments;
        public Table<Task> Tasks;
        public Table<User> Users;
    }

    [Table(Name = "Assignments")]
    public class Assignment
    {
        [Column(IsPrimaryKey = true)]
        public int TaskID;
        [Column(IsPrimaryKey = true)]
        public int UserID;
    }

    [Table(Name = "Tasks")]
    public class Task
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int TaskID;
        [Column]
        public DateTime BeginDateTime;
        [Column]
        public DateTime DeadlineDateTime;
        [Column]
        public string Title;
        [Column]
        public string Requirements;
    }

    [Table(Name = "Users")]
    public class User
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int UserID;
        [Column]
        public string FirstName;
        [Column]
        public string LastName;
    }
}