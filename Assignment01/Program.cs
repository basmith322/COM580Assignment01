using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Assignment01
{
    class ModuleAttendanceMenu
    {
        static void Main(string[] args)
        {
            AttendanceModel attendanceModel = new AttendanceModel();
            using (var db = new AttendanceModel())
            {
                Console.WriteLine("Attendance Monitoring System " +
                                "\n=============================");
                while (true)
                {
                    Console.WriteLine("Please select an option to continue");
                    Console.WriteLine("(1) List all module codes & names");
                    Console.WriteLine("(2) Enter a module code to list the staff name and and number of" +
                        "\nthe instructor taking the module");
                    Console.WriteLine("(3) Enter a module code to list the date and time of all learning" +
                        "\nevents associated with that module");
                    Console.WriteLine("(4) Enter a module code to list the student number and attendance" +
                        "\nrecord for said student");
                    Console.WriteLine("(5) Enter a module code and list the names of any students who have" +
                        "\nmissed 2 or more classes for said module");
                    Console.WriteLine("(6) Enter a module code to generate an attendance report");
                    Console.WriteLine("(7) Exit the system");

                    //Handle options selection
                    int option = 0;
                    Console.WriteLine("Enter Option Number: ");
                    while (!(int.TryParse(Console.ReadLine(), out option) && (option >= 1 && option <= 7)))
                    {
                        Console.WriteLine("Option must be between 1 and 7");
                        Console.WriteLine("Enter Option Number: ");
                    }
                    //Switch statement to call function based on which option was entered
                    switch (option)
                    {
                        case 7:
                            Console.WriteLine("Goodbye");
                            return;
                        default:
                            Console.WriteLine("Invalid Option!");
                            break;
                    }
                }//end while loop
            }
        }
    }
}

