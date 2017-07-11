using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace Server.Models
{
    public class DatabaseModel : DataContext
    {
        public DatabaseModel() : base(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database.mdf;Integrated Security=True") { }

        public Table<Assignments> Assignments;
        public Table<Tasks> Tasks;
        public Table<Users> Users;
    }

    [Table]
    public class Assignments
    {
        [Column(IsPrimaryKey = true)]
        public int TaskID;
        [Column(IsPrimaryKey = true)]
        public int UserID;
    }

    [Table]
    public class Tasks
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

    [Table]
    public class Users
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int UserID;
        [Column]
        public string FirstName;
        [Column]
        public string LastName;
    }
}