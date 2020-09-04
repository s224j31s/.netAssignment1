using BankConsoleApp.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Runtime.InteropServices;

namespace BankConsoleApp
{
    class MainMenu
    {
        static void Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine("╔═══════════════════════════════════════════════╗");
            Console.WriteLine("║\tWelcome to Simple Banking System\t║");
            Console.WriteLine("║═══════════════════════════════════════════════║");
            Console.WriteLine("║\t\t LOGIN TO START\t\t\t║");
            Console.WriteLine("║\t\t\t\t\t\t║");
            Console.WriteLine("║ User Name:                                    ║");
            Console.WriteLine("║ Password:                                     ║");
            Console.WriteLine("╚═══════════════════════════════════════════════╝");
            Console.SetCursorPosition(12, 5); //set username cursor location
            string inputUsername = Console.ReadLine();
            Console.SetCursorPosition(12, 6); //set password location
            char passwordChar = '*';
            string passwordInput = "";
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Backspace && passwordInput.Length > 0) //Allows backspace on password entry
                {
                    passwordInput = passwordInput.Remove(passwordInput.Length - 1);
                    Console.Write("\b \b");
                }
                else if (key.Key != ConsoleKey.Spacebar && key.Key != ConsoleKey.Enter &&
                  key.Key != ConsoleKey.Escape && key.Key != ConsoleKey.Tab &&
                  key.Key != ConsoleKey.Backspace) //check to prevent specific keys from being entered
                {
                    passwordInput += key.KeyChar;
                    Console.Write(passwordChar);
                }
            } while (key.Key != ConsoleKey.Enter); //loop to faciliate password masking

            Console.SetCursorPosition(0, 9);
            Console.WriteLine(inputUsername + " " + passwordInput);




            // Console.WriteLine(resourceName);
            string combine = inputUsername + "|";
            combine += passwordInput;
            Console.WriteLine(combine);

            if (File.Exists("login.txt"))//login.txt file check
            {
                bool loginDetails = false;
                string[] reader = System.IO.File.ReadAllLines("login.txt");
                try
                {
                    foreach (string s in reader)
                    {
                        if (combine.Equals(s))
                        {
                            Console.WriteLine("Success");
                            loginDetails = true;
                            break;
                        }
                        else
                        {
                            Console.Write("Invalid login details. Please try again");
                            break;
                        }

                    }
                }
                catch (IndexOutOfRangeException)
                {
                }
            }
            else
            {
                Console.WriteLine("Error");
            }

            Console.ReadKey();
        }
        //guest|1234user1|password123user2|321password

        private void loginMenuView()
        {
            Console.Clear();
            Console.WriteLine("╔═══════════════════════════════════════════════╗");
            Console.WriteLine("║\tWelcome to Simple Banking System\t║");
            Console.WriteLine("║═══════════════════════════════════════════════║");
            Console.WriteLine("║\t\t LOGIN TO START\t\t\t║");
            Console.WriteLine("║\t\t\t\t\t\t║");
            Console.WriteLine("║ User Name:                                    ║");
            Console.WriteLine("║ Password:                                     ║");
            Console.WriteLine("╚═══════════════════════════════════════════════╝");
            Console.SetCursorPosition(12, 5);
            Console.ReadLine();
            Console.SetCursorPosition(12, 6);
            Console.ReadLine();

        }

        private void mainMenuView()
        {

            Console.WriteLine("╔═══════════════════════════════════════════════╗");
            Console.WriteLine("║\tWelcome to Simple Banking System\t║");
            Console.WriteLine("║═══════════════════════════════════════════════║");
            Console.WriteLine("║ 1. Create a new account\t\t\t║");
            Console.WriteLine("║ 2. Search for a new Account\t\t\t║");
            Console.WriteLine("║ 3. Deposit:                                   ║");
            Console.WriteLine("║ 4. Withdraw:                                  ║");
            Console.WriteLine("║ 5. A/C Statement:                             ║");
            Console.WriteLine("║ 6. Delete Account:                            ║");
            Console.WriteLine("║ 7. Exit:                                      ║");
            Console.WriteLine("╚═══════════════════════════════════════════════╝");
            Console.WriteLine("║ \t\t\t\t\t\t║");
            Console.WriteLine("║ Enter your Choice (1-7):\t\t\t║");
            Console.WriteLine("╚═══════════════════════════════════════════════╝");

        }
    }
}
