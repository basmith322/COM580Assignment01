using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Assignment01
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new ModulesContext())
            {
                //Create a new Module
                Console.Write("Enter a Module Title: ");
                var name = Console.ReadLine();

                var module = new Modules { moduleName = name };
                db.Modules.Add(module);
                db.SaveChanges();

                var query = from b in db.Modules
                            orderby b.moduleName
                            select b;
                Console.WriteLine("All modules in the db");
                foreach (var item in query)
                {
                    Console.WriteLine(item.moduleName);
                }
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
        }
    }

    public class Modules
    {
        public int moduleID { get; set; }
        public int moduleCode { get; set; }
        public string moduleName { get; set; }
        public string moduleLevel { get; set; }
        public virtual Instructor Instructor { get; set; }
    }

    public class Instructor
    {
        public int instructorID { get; set; }
        public int instructorNumber { get; set; }
        public string instructorName { get; set; }
    }

    public class Student
    {
        public int studentID { get; set; }
        public string studentForname { get; set; }
        public string studentSurname { get; set; }
        public string studentNumber { get; set; }
        public string studentEmail { get; set; }
        public int studentTelNum { get; set; }
    }

    public class LearningEvent
    {
        public int eventID { get; set; }
        public string eventType { get; set; }
        public DateTime eventDateTime { get; set; }
        public int eventDuration { get; set; }
    }

    public class Attendance
    {
        public int attendanceID { get; set; }
        public string attendanceStatus { get; set; }
        public virtual LearningEvent LearningEventID { get; set; }
        public virtual Student StudentID { get; set; }
    }

    public class Register
    {
        public int registerID { get; set; }
        public DateTime regDate { get; set; }
        public virtual Student StudentID { get; set; }
        public virtual Modules ModuleID { get; set; }
    }

    public class ModulesContext: DbContext
    {
        public DbSet<Modules> Modules { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
    }
}
