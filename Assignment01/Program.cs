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
            ModuleAttendanceMenu moduleAttendanceMenu = new ModuleAttendanceMenu();
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
                        case 1:
                            moduleAttendanceMenu.ListModules(db);
                            break;
                        case 2:
                            moduleAttendanceMenu.InstructorTakingModule(db);
                            break;
                        case 3:
                            moduleAttendanceMenu.ModuleLearningEvents(db);
                            break;
                        case 4:
                            moduleAttendanceMenu.AttendanceRecord(db);
                            break;
                        case 5:
                            moduleAttendanceMenu.MissedClasses(db);
                            break;
                        case 6:
                            moduleAttendanceMenu.GenerateReport(db);
                            break;
                        case 7:
                            Console.WriteLine("Goodbye");
                            return;
                        default:
                            Console.WriteLine("Invalid Option!");
                            break;
                    }// end switch
                }//end while loop
            }//end using
        }//end main

        //(1)   List the module code and name of all Modules.
        public void ListModules(AttendanceModel dbC)
        {
            var query = from b in dbC.Modules
                        orderby b.ModuleName
                        select b;
            foreach (var item in query)
            {
                Console.WriteLine(String.Format("Module Name: {0} | Module Code: {1}", item.ModuleName, item.ModuleCode));
            }
            ReturnToMenu();

        }
        // end ListModules function

        //(2)   Given a module code, list the “Staff Number” and “Name” of the instructor taking the module.
        public void InstructorTakingModule(AttendanceModel dbC)
        {
            string moduleCode = getModuleCode();

            var module = dbC.Modules.Where(m => m.ModuleCode == moduleCode).FirstOrDefault();

            if (module != null)
            {
                Console.WriteLine("Module Code: " + module.ModuleCode);
                var instructors = module.Instructor;

                Console.WriteLine("Instructor Name: " + instructors.StaffName);
                Console.WriteLine("Instructor Number: " + instructors.StaffNum);
            }
            else
            {
                Console.WriteLine("There is no module with that module code");
            }
            ReturnToMenu();
        }
        //end InstructorTakingModule function

        //(3)   Given a module code, list the “Date”, “Time” and “Type” of all the learning events associated with that module.
        public void ModuleLearningEvents(AttendanceModel dbC)
        {
            string moduleCode = getModuleCode();

            var module = dbC.Modules.Where(l => l.ModuleCode == moduleCode).FirstOrDefault();

            if (module != null)
            {
                Console.WriteLine("Module Code: " + module.ModuleCode);
                var events = module.LearningEvents;
                foreach (var learningEvent in events)
                {
                    Console.WriteLine("Event Date/Time: " + learningEvent.eventDateTime);
                    Console.WriteLine("Event Type: " + learningEvent.eventType);
                }
            }
            else
            {
                Console.WriteLine("There is no module with that module code");
            }
            Console.WriteLine("---------------------------------");
        }
        //end ModuleLearningEvents function

        //(4)   Given a module code and student number, display the attendance record (ordered by date) for that student.
        public void AttendanceRecord(AttendanceModel dbC)
        {
            string moduleCode = getModuleCode();
            string studentNum = getStudentNum();

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

            string fullname = query.FirstOrDefault().studentForname + " " + query.FirstOrDefault().studentSurname;
            foreach (var attendances in query)
            {
                Console.WriteLine(String.Format("{0} {1} Attendance: {2} on {3}", fullname, attendances.eventType, attendances.attendanceStatus, attendances.eventDateTime));
            }
            ReturnToMenu();
        }
        //end AttendanceRecord function

        //(5)   List the names of all students who have missed two or more learning experiences given the module code.
        public void MissedClasses(AttendanceModel dbC)
        {
            string moduleCode = getModuleCode();

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
                    studentA.studentForname, studentA.studentSurname, studentA.studentNumber
                };

            foreach (var absentStudent in studentNames)
            {
                string fullname = absentStudent.studentForname + " " + absentStudent.studentSurname;
                Console.WriteLine(String.Format("{0} missed {1} events", fullname, absentStudent.absentCount));
            }
            ReturnToMenu();
        }
        //end MissedClasses function

        //(6)   Generate an attendance report for a selected module.
        public void GenerateReport(AttendanceModel dbC)
        {
            string moduleCode = getModuleCode();

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
            Console.WriteLine("Module Code | Event Type | Event Date/Time | Student Name | Attendance Status");
            foreach (var attendances in query)
            { 
                Console.WriteLine(String.Format("{0} | {1} | {2} | {3} {4} | {5}",
                    attendances.ModuleCode, attendances.eventType, 
                    attendances.eventDateTime, attendances.studentForname, 
                    attendances.studentSurname, attendances.attendanceStatus));
            }
            ReturnToMenu();
        }
        //end GenerateReport function

        /*A collection of additional functions to help increase code re-usability and reduce redundancy*/

        //get input from the user, takes a message input as a paramater
        string getInput(string msg)
        {
            Console.Write(msg);
            string input = Console.ReadLine();
            return input;
        }

        //get the module code from the user, uses getInput function, passes through message
        string getModuleCode()
        {
            return getInput("Please Enter a Module Code: ");
        }

        //get the student number from the user, uses getInput function, passes through message
        string getStudentNum()
        {
            return getInput("Please Enter a Student Number: ");
        }

        //asks the user to input any key before returning to the main menu
        ConsoleKeyInfo ReturnToMenu()
        {
            Console.WriteLine("Please press any key to return to the menu");
            Console.WriteLine("---------------------------------");
            return Console.ReadKey();
        }
    }
}

