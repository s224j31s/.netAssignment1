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
using System.Net;

namespace BankConsoleApp
{
    public class Bank
    {
        private bool loggedIn = false;
        private List<Accounts> accounts;
        public Bank()
        {
            accounts = new List<Accounts>();
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

            if (File.Exists("login.txt"))//login.txt file check
            {
                string[] reader = System.IO.File.ReadAllLines("login.txt");
                try
                {
                    foreach (string s in reader)
                    {
                        char[] delimiter = { '|' };
                        string[] loginCheck = s.Split(delimiter);

                        if (inputUsername == loginCheck[0] && passwordInput == loginCheck[1])
                        {
                            return true;
                        }
                    }
                    if (loginDetails == false)
                    {
                        Console.Write("Invalid login details.");
                        Console.Write(" Please try again (Y/N)");
                        char retry = '0';

                        while ((retry = readInput().KeyChar) != 'y')
                            switch (retry)
                            {
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
            return loggedIn;
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
        //main menu method after logging in
        private void mainMenu()
        {
            char select = '0';
            bool invalidKey = false;
            bool menuBreak = true;
            Console.Clear();
            mainMenuView();
            //Switch case for user selection

            while (menuBreak == true)
            {
                if (invalidKey)
                {
                    Console.SetCursorPosition(0, 15);
                    Console.WriteLine("Invalid key. Please enter a number between 1 and 7");
                }
                menuBreak = true;
                invalidKey = false;
                select = readInput().KeyChar;
                switch (select)
                {
                    case '1':
                        createAccount();
                        menuBreak = false;
                        break;

                    case '2':
                        searchAccount();
                        menuBreak = false;
                        break;
                    case '3':
                        depositAcc();
                        break;
                    case '4':
                        withdraw();
                        break;
                    case '5':
                        statement();
                        break;
                    case '6':
                        deleteAccount();
                        break;
                    case '7':
                        menuBreak = false;
                        Environment.Exit(0);
                        break;
                    default:
                        invalidKey = true;
                        menuBreak = true;
                        break;
                }
            }
        }

        private ConsoleKeyInfo readInput()
        {
            return Console.ReadKey(true);
        }
        //Console view of the create account function
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
        //Method to create an account. Contains phone number and email validation checks. Will contain sent email check
        private void createAccount()
        {
            Console.Clear();
            createAccountView();
            int accNumber;
            double balance;

            Console.SetCursorPosition(16, 3); 
            string firstName = Console.ReadLine();

            Console.SetCursorPosition(15, 4); 
            string lastName = Console.ReadLine();

            Console.SetCursorPosition(13, 5); 
            string address = Console.ReadLine();

            Console.SetCursorPosition(11, 6); 
            string phone = Console.ReadLine();

            Console.SetCursorPosition(11, 7);
            string email = Console.ReadLine();

            //Email regex pattern source https://www.rhyous.com/2010/06/15/csharp-email-regular-expression/
            string pattern = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*" + "@" + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$";
            Regex rgx = new Regex(pattern);
            Match match = rgx.Match(email);
            //Email validation with regex
            bool emailMatch = false;
            bool phoneSuccess = false;
            if (match.Success)
                emailMatch = true;
            else
                emailMatch = false;

            if (phone.Length == 10 && Int32.TryParse(phone, out int phoneNumber))
                phoneSuccess = true;
            else
                phoneSuccess = false;
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Is this information correct? Y/N");

            char select = '0';
            bool detailsValid = true;
            do
            {
                detailsValid = true;
                select = readInput().KeyChar;
                switch (select)
                {
                    case 'y':
                        if (phoneSuccess == true && emailMatch == true)
                        {
                            accNumber = new Random().Next(100000, 99999999);//Generates account number randomly
                            balance = 0;
                            Accounts accounts = new Accounts(firstName, lastName, address, phone, email, accNumber, balance);

                            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                            client.Credentials = new NetworkCredential("dotnetassignmentbank32@gmail.com", "Bankconsoleapp1");
                            client.EnableSsl = true;

                            MailMessage message = new MailMessage(new MailAddress("dotnetassignmentbank32@gmail.com", "Simple Bank"),
                                                   new MailAddress("dotnetassignmentbank32@gmail.com", firstName));

                            message.Subject = "Welcome to Simple Bank";
                            message.Body = $"Dear {firstName}, " + " Welcome to Simple Bank. " + "Your details are " + accNumber + firstName
                                + lastName + address + phone + email + balance + ". Kind regards, the Simple Bank Team.";
                            client.Send(message);

                            Console.WriteLine("Account created! Details will be emailed to you.");
                            Console.WriteLine("Account number is: " + accNumber);

                            string path1 = accNumber.ToString();
                            string FileName = Path.Combine(path1) + ".txt";//text file joining with account number
                            using (StreamWriter accDetails = File.CreateText(FileName))//creating accountnumber.txt
                            {
                                accDetails.WriteLine("First Name|" + firstName);
                                accDetails.WriteLine("Last Name|" + lastName);
                                accDetails.WriteLine("address|" + address);
                                accDetails.WriteLine("Phone Number|" + phone);
                                accDetails.WriteLine("email|" + email);
                                accDetails.WriteLine("Account Number|" + accNumber);
                                accDetails.WriteLine("Balance|" + balance);
                            }
                            Console.WriteLine("Do you wish to make another account? Y/N");
                            createAccYesNo();
                        }
                        else if (phoneSuccess == false && emailMatch == false)
                        {
                            Console.WriteLine("Error. Details invalid. Would you like to try again (Y/N)?");
                            createAccYesNo();
                        }
                        else if (emailMatch == false && phoneSuccess == true)
                        {
                            Console.WriteLine("Email is invalid. Would you like to try again (Y/N)?");
                            createAccYesNo();
                        }
                        else if (phoneSuccess == false && emailMatch == true)
                        {
                            Console.WriteLine("Phone Number is invalid. Would you like to try again (Y/N)?");
                            createAccYesNo();
                        }
                        detailsValid = false;
                        break;
                    case 'n':
                        createAccount();
                        detailsValid = false;
                        break;
                    default:
                        detailsValid = true;
                        break;
                }
            } while (detailsValid == true);
        }

        private void createAccYesNo()
        {
            char yesNoSelect = '0';
            yesNoSelect = readInput().KeyChar;
            bool yesNo = true;
            while (yesNo == true)
            {
                yesNo = true;
                switch (yesNoSelect)
                {
                    case 'y':
                        createAccount();
                        yesNo = false;

                        break;
                    case 'n':
                        mainMenu();
                        yesNo = false;

                        break;
                    default:
                        yesNoSelect = readInput().KeyChar;
                        yesNo = true;
                        break;
                }

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

        private void searchAccount()
        {
            bool searching = true;
            do
            {
                searching = true;
                Console.Clear();
                searchAccountView();
                Console.SetCursorPosition(23, 3);
                string accNumber = Console.ReadLine();
                Console.SetCursorPosition(0, 8);

                if (findAccount(accNumber))
                {
                    try
                    {
                        Account(Convert.ToInt32(accNumber)).viewAccDetails();
                        Console.WriteLine("Would you like to search for another account Y/N?");
                        searchAccYesNo();

                    }
                    catch
                    {
                        Console.WriteLine("Error. Please enter a valid number. Try again Y/N");
                        searchAccYesNo();
                    }
                }
                else
                {
                    Console.WriteLine("Error. Account Number not found");
                    Console.WriteLine("Would you like to try search again? Y/N");
                    searchAccYesNo();

                }
            } while (searching == true);

        }

        //method to check if the number from user input matches a text file, if it matches the account file contents are added to an account 
        //object and the method returns true. If the text file cannot be found, return false.
        private bool findAccount(string accNumber)
        {
            string FileName = Path.Combine(accNumber) + ".txt";
            if (accNumber.Length >= 6 && accNumber.Length <= 10 && Int32.TryParse(accNumber, out int accountNumber) && File.Exists(FileName))
            {

                string[] accountFile = File.ReadAllLines(FileName);
                string[] accountInfo = accountFile.Take(7).ToArray();
                List<string> accInfoList = new List<string>();
                foreach (string line in accountInfo)
                {
                    char[] delimiter = { '|' };
                    string[] splitInfo = line.Split(delimiter);
                    string parsedInfo = splitInfo[1].ToString();
                    accInfoList.Add(parsedInfo);
                }

                try
                {
                    Accounts validAccount = new Accounts(
                    accInfoList[0], accInfoList[1], accInfoList[2], accInfoList[3], accInfoList[4], Convert.ToInt32(accInfoList[5]), Convert.ToDouble(accInfoList[6]));
                    accounts.Add(validAccount);
                    validAccount.AddOldStatements();

                }
                catch
                {
                    return false;
                }
                return true;
            }
            else
            {
                return false;
            }

        }

        private void searchAccYesNo()
        {
            char select = '0';
            bool searchYesNo = true;
            select = readInput().KeyChar;
            do
            {
                searchYesNo = true;
                switch (select)
                {
                    case 'y':
                        searchAccount();
                        searchYesNo = false;

                        break;
                    case 'n':
                        mainMenu();
                        searchYesNo = false;

                        break;
                    default:
                        select = readInput().KeyChar;
                        searchYesNo = true;
                        break;
                }
            } while (searchYesNo == true);
        }
        private void depositAcc()
        {
            Console.Clear();
            depositView();
            Console.SetCursorPosition(23, 5);
            string accNumber = Console.ReadLine();
            Console.SetCursorPosition(18, 6);
            string amount = Console.ReadLine();
            string FileName = Path.Combine(accNumber) + ".txt";//text file joining with account number

            if (findAccount(accNumber))
            {

                if (Double.TryParse(amount, out double dblAmount))
                {
                    Account(Convert.ToInt32(accNumber)).Deposit(dblAmount);
                    Console.WriteLine();
                    Console.WriteLine("Amount deposited. Would you like to make another deposit Y/N?");
                    depositYesNo();
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("Error: Please enter a valid number");
                    Console.WriteLine("Would you like to try again Y/N?");
                    depositYesNo();
                }
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Error. Account Number not found");
                Console.WriteLine("Would you like to try search again? Y/N");
                depositYesNo();
            }
        }

        private void depositYesNo()
        {
            char select = '0';
            bool deposit = true;
            select = readInput().KeyChar;
            do
            {
                switch (select)
                {
                    case 'y':
                        deposit = false;
                        depositAcc();
                        break;
                    case 'n':
                        deposit = false;
                        mainMenu();
                        break;
                    default:
                        deposit = true;
                        select = readInput().KeyChar;
                        break;
                }
            } while (deposit == true);
        }

        private Accounts Account(int accountNumber)
        {

            foreach (Accounts account in accounts)
            {

                if (account.matchNumbers(accountNumber))
                    return account;
            }
            return null;
        }

        private void depositView()
        {
            Console.WriteLine("╔═══════════════════════════════════════════════╗");
            Console.WriteLine("║\t\tDeposit:\t\t\t║");
            Console.WriteLine("║═══════════════════════════════════════════════║");
            Console.WriteLine("║ \t\tEnter Details:\t\t\t║");
            Console.WriteLine("║═══════════════════════════════════════════════║");
            Console.WriteLine("║ \tAccount Number:\t\t\t\t║");
            Console.WriteLine("║ \tAmount: $\t\t\t\t║");
            Console.WriteLine("╚═══════════════════════════════════════════════╝");
        }
        private void withdraw()
        {
            Console.Clear();
            withdrawView();
            Console.SetCursorPosition(23, 5);
            string accNumber = Console.ReadLine();
            Console.SetCursorPosition(17, 6);
            string amount = Console.ReadLine();

            if (findAccount(accNumber))
            {
                if (Double.TryParse(amount, out double dblAmount))
                {
                    if (Account(Convert.ToInt32(accNumber)).checkBalance(dblAmount))
                    {
                        Account(Convert.ToInt32(accNumber)).Withdraw(dblAmount);
                        Console.WriteLine();
                        Console.WriteLine("Amount withdrawn. Would you like to make another withdrawal Y/N?");
                        withdrawYesNo();
                    }
                    else
                    {
                        Console.WriteLine();
                        Console.WriteLine("Account does not have enough funds to withdraw.");
                        Console.WriteLine("Would you like to try again Y/N?");
                        withdrawYesNo();
                    }
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("Error: Please enter a valid number");
                    Console.WriteLine("Would you like to try again Y/N?");
                    withdrawYesNo();
                }
            }
            else
            {
                Console.WriteLine("Error. Account Number not found");
                Console.WriteLine("Would you like to try search again? Y/N");
                withdrawYesNo();
            }
        }

        private void withdrawView()
        {
            Console.WriteLine("╔═══════════════════════════════════════════════╗");
            Console.WriteLine("║\t\tWithdraw:\t\t\t║");
            Console.WriteLine("║═══════════════════════════════════════════════║");
            Console.WriteLine("║ \t\tEnter Details:\t\t\t║");
            Console.WriteLine("║═══════════════════════════════════════════════║");
            Console.WriteLine("║ \tAccount Number:\t\t\t\t║");
            Console.WriteLine("║ \tAmount: $\t\t\t\t║");
            Console.WriteLine("╚═══════════════════════════════════════════════╝");
        }


        private void withdrawYesNo()
        {
            char select = '0';
            bool withdrawing = true;
            select = readInput().KeyChar;
            do
            {
                switch (select)
                {
                    case 'y':
                        withdrawing = false;
                        withdraw();
                        break;
                    case 'n':
                        withdrawing = false;
                        mainMenu();
                        break;
                    default:
                        withdrawing = true;
                        select = readInput().KeyChar;
                        break;
                }
            } while (withdrawing == true);
        }

        private void statement()
        {
            Console.Clear();
            statementView();
            Console.SetCursorPosition(23, 5);
            string accNumber = Console.ReadLine();
            Console.WriteLine();
            if (findAccount(accNumber))
            {
                string FileName = Path.Combine(accNumber.ToString()) + ".txt";
                string[] accountFile = File.ReadAllLines(FileName);
                string[] accountInfo = accountFile.Take(7).ToArray();

                foreach (string line in accountInfo)
                {
                    char[] delimiter = { '|' };
                    string[] death = line.Split(delimiter);
                    Console.WriteLine(death[1].ToString());

                }
            }
            else
                Console.WriteLine("Error");
        }

        private void statementView()
        {
            {
                Console.WriteLine("╔═══════════════════════════════════════════════╗");
                Console.WriteLine("║\t\tStatement:\t\t\t║");
                Console.WriteLine("║═══════════════════════════════════════════════║");
                Console.WriteLine("║ \t\tEnter Details:\t\t\t║");
                Console.WriteLine("║═══════════════════════════════════════════════║");
                Console.WriteLine("║ \tAccount Number:\t\t\t\t║");
                Console.WriteLine("╚═══════════════════════════════════════════════╝");
            }
        }

        private void deleteAccount()
        {
            Console.Clear();
            deleteView();
            Console.SetCursorPosition(23, 5);
            string accNumber = Console.ReadLine();
            bool deleteConfirm = true;
            char deleteSelect = '0';
            Console.WriteLine();

            if (findAccount(accNumber))
            {
                string FileName = Path.Combine(accNumber) + ".txt";//text file joining with account number
                Account(Convert.ToInt32(accNumber)).viewAccDetails();
                Console.WriteLine("Are you sure you want to delete this file Y/N?");

                while (deleteConfirm == true)
                {
                    deleteSelect = readInput().KeyChar;
                    switch (deleteSelect)
                    {
                        case 'y':
                            File.Delete(FileName);
                            Console.WriteLine("Account deleted.");
                            Console.WriteLine("Would you like to try again Y/N?");
                            retryDelete();
                            deleteConfirm = false;
                            break;
                        case 'n':
                            mainMenu();
                            deleteConfirm = false;
                            break;
                        default:
                            deleteSelect = readInput().KeyChar;
                            deleteConfirm = true;
                            break;
                    }
                }

            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Error. Account Number not found");
                Console.WriteLine("Would you like to try search again? Y/N");
                retryDelete();
            }
        }

        private void retryDelete()
        {
            bool delete = true;
            char retryDelete = '0';
            retryDelete = readInput().KeyChar;
            do
            {
                switch (retryDelete)
                {
                    case 'y':
                        {
                            Console.Clear();
                            deleteAccount();
                            break;
                        }
                    case 'n':
                        {
                            mainMenu();
                            break;
                        }
                    default:
                        retryDelete = readInput().KeyChar;
                        break;
                }

            } while (delete == true);
        }

        private void deleteView()
        {
            Console.WriteLine("╔═══════════════════════════════════════════════╗");
            Console.WriteLine("║\t\tDelete Account \t\t\t║");
            Console.WriteLine("║═══════════════════════════════════════════════║");
            Console.WriteLine("║ \t\tEnter Details \t\t\t║");
            Console.WriteLine("║═══════════════════════════════════════════════║");
            Console.WriteLine("║ \tAccount Number:\t\t\t\t║");
            Console.WriteLine("╚═══════════════════════════════════════════════╝");
        }

    }
}
