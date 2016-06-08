using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserAuth.Data;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press 1 to create user, 2 to test login");
            string choice = Console.ReadLine();
            if (choice == "1")
            {
                Console.WriteLine("Enter username");
                string username = Console.ReadLine();
                Console.WriteLine("Enter password");
                string password = Console.ReadLine();
                var manager = new UserManager(Properties.Settings.Default.ConStr);
                manager.AddUser(username, password);
                Console.WriteLine("User created!");
            }
            else
            {
                Console.WriteLine("Enter username");
                string username = Console.ReadLine();
                Console.WriteLine("Enter password");
                string password = Console.ReadLine();
                var manager = new UserManager(Properties.Settings.Default.ConStr);
                var user = manager.Login(username, password);
                if (user == null)
                {
                    Console.WriteLine("INVALID LOGIN!!!");
                }
                else
                {
                    Console.WriteLine("WOOHOO!! You're in!");
                }
            }

            Console.ReadKey(true);
        }
    }
}
