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
            //Create a new Module
            Console.Write("Enter a Module Title: ");
            var title = Console.ReadLine();

            var module = new Modules { moduleTitle = title };

        }
    }

    public class Modules
    {
        public int moduleID { get; set; }
        public int moduleCode { get; set; }
        public string moduleTitle { get; set; }
        public virtual Instructor Instructor { get; set; }
    }

    public class Instructor
    {
        public int instructorID { get; set; }
        public string instructorForname { get; set; }
        public string instructorSurname { get; set; }
        public virtual Modules Modules { get; set; }
    }

    public class Student
    {
        public int studentID { get; set; }
        public string studentForname { get; set; }
        public string studentSurname { get; set; }
    }

    public class LearningEvent
    {
        public int eventID { get; set; }
        public string eventTitle { get; set; }
        public string eventType { get; set; }
        public DateTime eventDate { get; set; }
        public DateTime eventTime { get; set; }
    }
}
