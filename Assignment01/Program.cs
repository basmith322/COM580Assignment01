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
            using (var db = new AttendanceModel())
            {
                //Console.Write("Enter a new module name: ");
                //var moduleName = Console.ReadLine();

                //var module = new Module { ModuleName = moduleName };
                //db.Modules.Add(module);
                //db.SaveChanges();

                var query = from b in db.Modules
                            orderby b.ModuleName
                            select b;

                Console.WriteLine("All modules in db:");
                foreach (var item in query)
                {
                    Console.WriteLine(item.ModuleName);
                }

                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
        }
    }
}

