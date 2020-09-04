using BankConsoleApp.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace BankConsoleApp
{
    public class Bank
    {
        private bool loggedIn = false;
        public Bank()
        {

        }

        public static void Main(string[] args)
        {
            Bank bank = new Bank();
            do
            {
                if (bank.Login())
                    bank.mainMenu();
            } while (!bank.loggedIn);

        }

        private Boolean Login()
        {
            bool loginDetails = false;
            Console.Clear();
            loginMenuView();
            Console.SetCursorPosition(12, 5); //set username cursor location
            string inputUsername = Console.ReadLine();
            Console.SetCursorPosition(12, 6); //set password location
            char passwordChar = '*';
            string passwordInput = "";
            ConsoleKeyInfo key;
            do
            {
                key = readInput();
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


            string combine = inputUsername + "|";
            combine += passwordInput;
            Console.WriteLine(combine);

            if (File.Exists("login.txt"))//login.txt file check
            {
                string[] reader = System.IO.File.ReadAllLines("login.txt");
                try
                {
                    foreach (string s in reader)
                    {
                        if (combine == s)
                        {
                            return true;
                        }

                    }
                    if (loginDetails == false)
                    {
                        Console.Write("Invalid login details.");
                        Console.Write("Please try again (Y/N)");
                        char retry = '0';

                        while ((retry = readInput().KeyChar) != 'y')
                            switch (retry)
                            {
                                case 'y':
                                    break;
                                case 'n':
                                    Environment.Exit(0);
                                    break;
                                default:
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
                Console.WriteLine("Error: login.txt was not able to be found");
                Console.WriteLine("Press any key to exit the application");
                Console.ReadKey();
                Environment.Exit(0);

            }
            return false;
        }
        private void loginMenuView()
        {

            Console.WriteLine("╔═══════════════════════════════════════════════╗");
            Console.WriteLine("║\tWelcome to Simple Banking System\t║");
            Console.WriteLine("║═══════════════════════════════════════════════║");
            Console.WriteLine("║\t\t LOGIN TO START\t\t\t║");
            Console.WriteLine("║\t\t\t\t\t\t║");
            Console.WriteLine("║ User Name:                                    ║");
            Console.WriteLine("║ Password:                                     ║");
            Console.WriteLine("╚═══════════════════════════════════════════════╝");

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
        private void mainMenu()
        {
            Console.Clear();
            mainMenuView();
            //Switch case for user selection
            char select = '0';
            while ((select = readInput().KeyChar) != '7')
            {
                switch (select)
                {
                    case '1':
                        createAccount();
                        break;

                    case '2':
                        searchAccount();
                        break;
                    case '3':
                        //deposit();
                        break;
                    case '4':
                        //withdraw();
                        break;
                    case '5':
                        //statement();
                        break;
                    case '6':
                        //deleteAccount();
                        break;
                    case '7':
                        this.loggedIn = false;

                        break;
                    default:
                        break;
                }

            }
        }

        private ConsoleKeyInfo readInput()
        {
            return Console.ReadKey(true);
        }
        private void createAccountView()
        {
            Console.WriteLine("╔═══════════════════════════════════════════════╗");
            Console.WriteLine("║\tCreate a new account\t\t\t║");
            Console.WriteLine("║═══════════════════════════════════════════════║");
            Console.WriteLine("║ 1. First Name: \t\t\t\t║");
            Console.WriteLine("║ 2. Last Name: \t\t\t\t║");
            Console.WriteLine("║ 3. Address:                                   ║");
            Console.WriteLine("║ 4. Phone:                                     ║");
            Console.WriteLine("║ 5. Email:                                     ║");
            Console.WriteLine("╚═══════════════════════════════════════════════╝");
        }
        private void createAccount()
        {

            Console.Clear();
            createAccountView();

            Console.SetCursorPosition(16, 3); //set FirstName cursor location
            string firstName = Console.ReadLine();

            Console.SetCursorPosition(15, 4); //set FirstName cursor location
            string lastName = Console.ReadLine();

            Console.SetCursorPosition(13, 5); //set FirstName cursor location
            string address = Console.ReadLine();

            Console.SetCursorPosition(11, 6); //set FirstName cursor location
            string phone = Console.ReadLine();

            Console.SetCursorPosition(11, 7); //set FirstName cursor location
            string email = Console.ReadLine();

            //Email regex pattern source https://www.rhyous.com/2010/06/15/csharp-email-regular-expression/
            string pattern = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*" + "@" + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$";
            Regex rgx = new Regex(pattern);
            Match match = rgx.Match(email);
            //Email validation with regex
            if (match.Success)
                Console.WriteLine(email + " is correct");
            else
                Console.WriteLine(email + " is incorrect");

            if (phone.Length == 10 && Int32.TryParse(phone, out int phoneNumber))
            {
                int accNumber;
                accNumber = new Random().Next(100000, 99999999);

            }

            Console.Clear();
            Console.WriteLine();

            Console.WriteLine("╔═══════════════════════════════════════════════╗");
            Console.WriteLine("║\tCreate a new account\t\t\t║");
            Console.WriteLine("║═══════════════════════════════════════════════║");
            Console.WriteLine("║ 1. First Name:" + firstName + "\t\t\t\t║");
            Console.WriteLine("║ 2. Last Name: " + lastName + "\t\t\t\t║");
            Console.WriteLine("║ 3. Address:" + address + " \t\t\t\t\t║");
            Console.WriteLine("║ 4. Phone:" + phone + "\t\t\t\t║");
            Console.WriteLine("║ 5. Email:" + email + " \t\t\t║");
            Console.WriteLine("╚═══════════════════════════════════════════════╝");


            Console.WriteLine();
            Console.WriteLine("Is this information correct? Y/N");
            ConsoleKeyInfo confirm;
            confirm = Console.ReadKey(true);


            do
            {
                if (confirm.Key == ConsoleKey.Y)
                {
                    int accNumber;
                    accNumber = new Random().Next(100000, 99999999);//Generates account number randomly
                    Console.WriteLine("Account created! Details will be emailed to you.");
                    Console.WriteLine("Account number is: " + accNumber);

                    Accounts accounts = new Accounts(accNumber, firstName, lastName, address, phone, 0, email);
                    string path1 = accNumber.ToString();
                    string FileName = Path.Combine(path1) + ".txt";//text file joining with account number
                    using (StreamWriter accDetails = File.CreateText(FileName))//creating accountnumber.txt
                    {
                        accDetails.WriteLine("First Name|" + firstName);
                        accDetails.WriteLine("Last Name|" + lastName);
                        accDetails.WriteLine("Address|" + address);
                        accDetails.WriteLine("Phone|" + phone);
                        accDetails.WriteLine("email|" + email);
                        accDetails.WriteLine("Account Number|" + accNumber);
                        accDetails.WriteLine("Balance|" + '0');
                    }


                    using (StreamReader sr = File.OpenText(FileName))
                    {
                        string s = "";
                        while ((s = sr.ReadLine()) != null)
                        {
                            Console.WriteLine(s);
                        }
                    }

                    Console.WriteLine("Do you wish to make another account? Y/N");
                    ConsoleKeyInfo repeat = Console.ReadKey(true);
                    if (repeat.Key == ConsoleKey.Y)
                    {
                        Console.Clear();
                        createAccount();
                    }
                    else if (repeat.Key == ConsoleKey.N)
                    {
                        mainMenu();
                    }


                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("Exiting Menu");
                    createAccount();
                    break;

                }
            } while (confirm.Key != ConsoleKey.Y | confirm.Key != ConsoleKey.N); //do while for confirmation 

        }
        private void searchAccount()
        {
            bool searching = false;
            char retry = '0';
            do
            {
                Console.Clear();
                searchAccountView();

                Console.SetCursorPosition(23, 3);

                string accNumber = Console.ReadLine();
                string FileName = Path.Combine(accNumber) + ".txt";//text file joining with account number

                Console.SetCursorPosition(0, 8);


                if (File.Exists(FileName))
                {
                    try
                    {
                        using (StreamReader sr = File.OpenText(FileName))
                        {
                            string line;
                            // Read and display lines from the file until the end of
                            // the file is reached.
                            while ((line = sr.ReadLine()) != null)
                            {
                                Console.WriteLine(line);
                            }
                        }
                        Console.WriteLine("Would you like to search for another account Y/N?");
                        retrySearch();
                    }
                    catch
                    {
                    }
                }
                else
                {
                    Console.WriteLine("Error. Account Number not found");
                    Console.WriteLine("Would you like to try search again? Y/N");
                    retrySearch();

                }
            } while (searching == false);

        }
        private void retrySearch()
        {
            bool searching = false;
            char retry = '0';
            while ((retry = readInput().KeyChar) != 'y')
                switch (retry)
                {
                    case 'y':
                        searching = false;
                        break;
                    case 'n':
                        searching = true;
                        mainMenu();
                        break;
                    default:
                        break;
                }

        }

        private void searchAccountView()
        {
            Console.WriteLine("╔═══════════════════════════════════════════════╗");
            Console.WriteLine("║\tSearch for Account:\t\t\t║");
            Console.WriteLine("║═══════════════════════════════════════════════║");
            Console.WriteLine("║ \tAccount Number:\t\t\t\t║");
            Console.WriteLine("╚═══════════════════════════════════════════════╝");
        }
    }
}
