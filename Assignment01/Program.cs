using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using ConsoleTables;
using System.Text.RegularExpressions;

namespace Assignment01
{
    class ModuleAttendanceMenu
    {
        static void Main(string[] args)
        {
            ModuleAttendanceMenu moduleAttendanceMenu = new ModuleAttendanceMenu();
            using (var db = new AttendanceModel())
            {
                while (true)
                {
                    //Clear previous results from console
                    Console.Clear();
                    // Create a table to hold a list of menu options
                    var table = new ConsoleTable("Attendance Monitoring System");
                    table.AddRow("Please Select an option to continue");
                    table.AddRow("(1) List all module codes & names");
                    table.AddRow("(2) Enter a module code to list the staff name " +
                        "and and number of the instructor taking the module");
                    table.AddRow("(3) Enter a module code to list the date and time of all learning " +
                        "events associated with that module");
                    table.AddRow("(4) Enter a module code to list the student number and attendance" +
                            "record for said student");
                    table.AddRow("(5) Enter a module code and list the names of any students who have" +
                            "missed 2 or more classes for said module");
                    table.AddRow("(6) Enter a module code to generate an attendance report");
                    table.AddRow("(7) Exit the system");
                    table.Write();

                    // Handle options selection
                    int option = 0;
                    Console.Write("Enter Option Number: ");
                    while (!(int.TryParse(Console.ReadLine(), out option) && (option >= 1 && option <= 7)))
                    {
                        Console.WriteLine("Option must be between 1 and 7");
                        Console.WriteLine("Enter Option Number: ");
                    }

                    // Switch statement to call function based on which option was entered
                    switch (option)
                    {
                        case 1:
                            Console.Clear();
                            moduleAttendanceMenu.ListModules(db);
                            break;
                        case 2:
                            Console.Clear();
                            moduleAttendanceMenu.InstructorTakingModule(db);
                            break;
                        case 3:
                            Console.Clear();
                            moduleAttendanceMenu.ModuleLearningEvents(db);
                            break;
                        case 4:
                            Console.Clear();
                            moduleAttendanceMenu.AttendanceRecord(db);
                            break;
                        case 5:
                            Console.Clear();
                            moduleAttendanceMenu.MissedClasses(db);
                            break;
                        case 6:
                            Console.Clear();
                            moduleAttendanceMenu.GenerateReport(db);
                            break;
                        case 7:
                            Console.Clear();
                            Console.WriteLine("Goodbye");
                            return;
                        default:
                            Console.Clear();
                            Console.WriteLine("Invalid Option!");
                            break;
                    }// end switch
                }// end while loop
            }// end using
        }// end main

        //(1)   List the module code and name of all Modules.
        public void ListModules(AttendanceModel dbC)
        {
            Console.WriteLine("List of all modules \n");
            // Initiate query to obtain all modules from the table
            var query = from m in dbC.Modules
                        orderby m.ModuleName
                        select m;

            // Create a new table to house the results
            var table = new ConsoleTable("Module Code", "Module Title");

            // Ensure that the query doesn't return a blank result
            if (query.FirstOrDefault() == null)
            {
                Console.WriteLine("There are no modules in the system");
                ReturnToMenu();
            }

            //Iterate through query results to add the moduleCode and moduleName to the table
            foreach (var item in query)
            {
                table.AddRow(item.ModuleCode, item.ModuleName);
            }

            table.Write();
            ReturnToMenu();
        }
        // end ListModules function

        //(2)   Given a module code, list the “Staff Number” and “Name” of the instructor taking the module.
        public void InstructorTakingModule(AttendanceModel dbC)
        {
            Console.WriteLine("List of Staff taking module \n");

            /* Obtain the module code from the user, query it against the 
            database and store the value in the moduleCode variable */
            string moduleCode = getModuleCode();
            var module = dbC.Modules.Where(m => m.ModuleCode == moduleCode).FirstOrDefault();
            

            // Check that the module variable isn't empty and print the required information
            if (module != null)
            {
                var instructors = module.Instructor;
                Console.WriteLine(String.Format("Module Code: {0} | Instructor Name: {1} | Instructor Number: {2}",
                    module.ModuleCode, instructors.StaffName, instructors.StaffNum) + "\n");
            }
            else
            {
                Console.WriteLine("There is no module with that module code");
            }
            ReturnToMenu();
        }
        //end InstructorTakingModule function

        /* (3)   Given a module code, list the “Date”, “Time” and “Type” of 
        all the learning events associated with that module. */
        public void ModuleLearningEvents(AttendanceModel dbC)
        {
            Console.WriteLine("List of learning events for module \n");

            /* Obtain the module code from the user, query it against the 
            database and store the value in the moduleCode variable */
            string moduleCode = getModuleCode();
            var module = dbC.Modules.Where(l => l.ModuleCode == moduleCode).FirstOrDefault();

            // Check that the module variable isn't empty and print the required information
            if (module != null)
            {
                var events = module.LearningEvents;

                //Create table
                var table = new ConsoleTable("Event Type", "Event Date/Time");

                // Iterate through the results adding a row for each set of results. Print the table
                foreach (var learningEvent in events)
                {
                    table.AddRow(learningEvent.eventType, learningEvent.eventDateTime);
                }
                table.Write();
            }
            else
            {
                Console.WriteLine("There is no module with that module code");
            }
            ReturnToMenu();
        }
        //end ModuleLearningEvents function

        /* (4)   Given a module code and student number, display the attendance record 
        (ordered by date) for that student. */
        public void AttendanceRecord(AttendanceModel dbC)
        {
            Console.WriteLine("Student attendance for a module \n");

            // Obtain the module code & student number from the user
            string moduleCode = getModuleCode();
            string studentNum = getStudentNum();

            // Joining tables together to ensure the relevant information can be queried
            var query =
            from attendanceA in dbC.Attendances
            join learningEventA in dbC.LearningEvents on attendanceA.eventID equals learningEventA.Id
            join moduleA in dbC.Modules on learningEventA.moduleID equals moduleA.Id
            join studentA in dbC.Students on attendanceA.studentID equals studentA.Id
            where moduleA.ModuleCode == moduleCode && studentA.studentNumber == studentNum
            orderby learningEventA.eventDateTime ascending
            select new
            {
                moduleA.ModuleCode,
                learningEventA.eventType,
                attendanceA.attendanceStatus,
                learningEventA.eventDateTime,
                studentA.studentForname,
                studentA.studentSurname
            };

            var table = new ConsoleTable("Student Name", "Learning Event", "Attendance Status", "Event Date/Time");

            // Check the query results are not empty
            if (query.FirstOrDefault() == null)
            {
                Console.WriteLine("Student Number or Module code not found");
                ReturnToMenu();
            }
            else
            {
                // String to concatenate student forename and surname
                string fullname = query.FirstOrDefault().studentForname + " " + query.FirstOrDefault().studentSurname;

                // Iterate through the results and write a line for each result
                foreach (var attendances in query)
                {
                    table.AddRow(fullname, attendances.eventType, attendances.attendanceStatus, attendances.eventDateTime);
                }
                table.Write();
                Console.WriteLine("");
                ReturnToMenu();
            }
        }
        //end AttendanceRecord function

        //(5)   List the names of all students who have missed two or more learning experiences given the module code.
        public void MissedClasses(AttendanceModel dbC)
        {
            Console.WriteLine("List of students who have missed more than 2 learning experiences \n");

            // Obtain the module code from the user
            string moduleCode = getModuleCode();

            // Joining tables together to ensure the relevant information can be queried
            var studentIds =
                from attendanceA in dbC.Attendances
                join learningEventA in dbC.LearningEvents on attendanceA.eventID equals learningEventA.Id
                join moduleA in dbC.Modules on learningEventA.moduleID equals moduleA.Id
                join studentA in dbC.Students on attendanceA.studentID equals studentA.Id
                where moduleA.ModuleCode == moduleCode && attendanceA.attendanceStatus == "Absent"
                group attendanceA by studentA.Id
                into attendanceGroup
                where attendanceGroup.Count() >= 2
                select new
                {
                    absentCount = attendanceGroup.Count(),
                    studentid = attendanceGroup.Key
                };

            var attendanceQuery =
                from attendanceA in dbC.Attendances
                join attendanceStatus in studentIds on attendanceA.Id equals attendanceStatus.studentid

                select new
                {
                    attendanceA.attendanceStatus
                };

            var studentNames =
                from studentA in dbC.Students
                join absentStudent in studentIds on studentA.Id equals absentStudent.studentid
                select new
                {
                    absentStudent.absentCount,
                    studentA.studentForname,
                    studentA.studentSurname,
                    studentA.studentNumber
                };

            // Check the query results are not empty
            if (studentIds.FirstOrDefault() == null)
            {
                Console.WriteLine("Module code not found");
                ReturnToMenu();
            }

            // Iterate through query results and print them
            foreach (var absentStudent in studentNames)
            {
                string fullname = absentStudent.studentForname + " " + absentStudent.studentSurname;
                Console.WriteLine(String.Format("{0} missed {1} learning events", fullname, absentStudent.absentCount));
            }
            ReturnToMenu();
        }
        //end MissedClasses function

        //(6)   Generate an attendance report for a selected module.
        public void GenerateReport(AttendanceModel dbC)
        {
            Console.WriteLine("Attendance Report for module \n");

            //Obtain moduleCode from the user
            string moduleCode = getModuleCode();

            // Joining tables together to ensure the relevant information can be queried 
            var query =
            from attendanceA in dbC.Attendances
            join learningEventA in dbC.LearningEvents on attendanceA.eventID equals learningEventA.Id
            join moduleA in dbC.Modules on learningEventA.moduleID equals moduleA.Id
            join studentA in dbC.Students on attendanceA.studentID equals studentA.Id
            where moduleA.ModuleCode == moduleCode
            orderby moduleA.ModuleCode ascending
            select new
            {
                moduleA.ModuleCode,
                learningEventA.eventType,
                attendanceA.attendanceStatus,
                learningEventA.eventDateTime,
                studentA.studentForname,
                studentA.studentSurname
            };

            // Check the query results are not empty 
            if (query.FirstOrDefault() == null)
            {
                Console.WriteLine("Module code not found");
                ReturnToMenu();
            }

            // Creating the table
            var table = new ConsoleTable("Module Code", "Event Type", "Event Date/Time",
                "Student Name", "Attendance Status");

            // Iterate through the results and add them to rows, print the table
            foreach (var attendances in query)
            {
                string fullname = attendances.studentForname + " " + attendances.studentSurname;
                table.AddRow(attendances.ModuleCode, attendances.eventType,
                    attendances.eventDateTime, fullname, attendances.attendanceStatus);
            }
            table.Write();
            ReturnToMenu();
        }
        //end GenerateReport function

        /*A collection of additional functions to help increase code re-usability and reduce redundancy*/

        //get input from the user, takes a message input as a paramater
        string getInput(string msg, string regex)
        {
            Console.Write(msg);
            string input = Console.ReadLine();
            Console.WriteLine("");
            while (!Regex.IsMatch(input, regex))
            {
                Console.WriteLine("Invald Input");
                Console.Write(msg);
                input = Console.ReadLine();
            }
            return input;
        }

        //get the module code from the user, uses getInput function, passes through message
        string getModuleCode()
        {
            return getInput("Please Enter a Module Code: ", @"[A-Za-z][0-9]");
        }

        //get the student number from the user, uses getInput function, passes through message
        string getStudentNum()
        {
            return getInput("Please Enter a Student Number: ", @"[A-Za-z][0-9]");
        }

        //asks the user to input any key before returning to the main menu
        ConsoleKeyInfo ReturnToMenu()
        {
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine("Please press any key to return to the menu");
            Console.WriteLine("-------------------------------------------");
            return Console.ReadKey();
        }
    }
}

