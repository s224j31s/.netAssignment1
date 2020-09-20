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
            } while (!bank.loggedIn); //boolean loop to show login menu if logged in

        }
        //Initiate the login process. Checks the login process by username and password. Requires login.txt file or the appliation will prompt
        //the user that it is missing and thus unable to login.
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
            try
            {
                if (File.Exists("login.txt"))//login.txt file check
                {
                    string[] reader = System.IO.File.ReadAllLines("login.txt");
                    try
                    {
                        foreach (string s in reader)
                        {
                            char[] delimiter = { '|' };
                            string[] loginCheck = s.Split(delimiter);
                            //username and password check
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
                    catch
                    {
                        Console.WriteLine("Error. Press any key to exit");
                        Console.ReadKey();
                        Environment.Exit(0);
                    }
                }
                else
                {   //if no login text file was found
                    Console.WriteLine("Error: login.txt was not able to be found");
                    Console.WriteLine("Press any key to exit the application");
                    Console.ReadKey();
                    Environment.Exit(0);
                }
            }
            catch
            {
                Console.WriteLine("There was an error. Please reopen the application.");
                Console.ReadKey();
                Environment.Exit(0);
            }

            return loggedIn;
        }
        //console view of login
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
        //console view of main menu
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
        //main menu method after logging in. Cases 1-7 are options for the user to input. any other case results in user being informed of invalid choices.

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
                if (invalidKey) //check for invalid inputs
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

        //reads console input
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


        //checks if create account fields are empty
        private bool emptyCheck(string check)
        {
            if (String.IsNullOrEmpty(check))
            {
                return true;
            }
            return false;
        }
        //Method to create an account. Contains phone number, empty field and email validation checks. Contains email functionality to email the user their account details upon success
        private void createAccount()
        {
            Console.Clear();
            createAccountView();
            int accNumber;
            double balance;
            bool fnameCheck = false;
            bool lnameCheck = false;
            bool addrCheck = false;

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

            if (phone.Length == 10 && Int32.TryParse(phone, out int phoneNumber))//checks if phone number i svalid
                phoneSuccess = true;
            else
                phoneSuccess = false;
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Is this information correct? Y/N");

            char select = '0';
            bool detailsValid = true;
            bool noEmpty = false;
            emptyCheck(firstName);
            emptyCheck(address);
            emptyCheck(lastName);
            do
            {
                detailsValid = true;

                if (emptyCheck(firstName) == false)
                {
                    fnameCheck = true;
                }
                if (emptyCheck(lastName) == false)
                {
                    lnameCheck = true;
                }
                if (emptyCheck(address) == false)
                {
                    addrCheck = true;
                }

                if (fnameCheck == true && lnameCheck == true && addrCheck == true)
                {
                    noEmpty = true;
                }
                select = readInput().KeyChar;
                switch (select)
                {
                    case 'y':
                        if (phoneSuccess == true && emailMatch == true && noEmpty == true)//If all fields are valid
                        {
                            accNumber = new Random().Next(100000, 99999999);//Generates account number randomly
                            balance = 0;
                            Accounts accounts = new Accounts(firstName, lastName, address, phone, email, accNumber, balance);//creates an object
                            //Code to allow for email to be sent
                            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                            client.Credentials = new NetworkCredential("dotnetassignmentbank32@gmail.com", "Bankconsoleapp1");
                            client.EnableSsl = true;
                            bool emailSent = true;
                            try
                            {
                                //Code for email including the sender, receiver and message contents. ALlows for HTML to be in the body of the email
                                MailMessage message = new MailMessage(new MailAddress("dotnetassignmentbank32@gmail.com", "Simple Bank"),
                                                 new MailAddress("dotnetassignmentbank32@gmail.com", firstName));
                                message.IsBodyHtml = true;
                                message.Subject = "Welcome to Simple Bank";

                                message.Body = string.Format($"Dear {firstName},<br><br>" +
                              "Welcome to Simple Bank !<br><br>" +
                              "Your account details are as follows:<br>" +

                              $"Account number: {accNumber}<br>" +
                              $"First name: {firstName}<br>" +
                              $"Last name: {lastName}<br>" +
                              $"Address: {address}<br>" +
                              $"Phone number: {phone}<br>" +
                              $"Email: {email}<br><br>" +

                              "Kind regards,<br>" +
                              "The Simple Bank Team");
                                client.Send(message); //sends the email message
                                emailSent = true;
                            }
                            catch
                            {
                                emailSent = false;
                            }

                            if (emailSent == true)
                            {
                                Console.WriteLine("Account created! Details will be emailed to you.");
                            }
                            else
                            {
                                Console.WriteLine("Account details could not be sent at this time");
                            }

                            Console.WriteLine("Account number is: " + accNumber);
                            //Process of creating a file and saving the account details to file. Utilises streamwriter
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
                        }//below are the potential scenarios for invalid input
                        else if (noEmpty == true && phoneSuccess == false && emailMatch == false || noEmpty == false && phoneSuccess == true && emailMatch == true ||
                             noEmpty == false && phoneSuccess == false && emailMatch == true ||
                              noEmpty == false && phoneSuccess == false && emailMatch == false)
                        {
                            Console.WriteLine("Cannot have empty fields. Would you like to try again (Y/N)?");
                            createAccYesNo();
                        }
                        else if (phoneSuccess == false && emailMatch == false && noEmpty == false)
                        {
                            Console.WriteLine("Error. Details invalid. Would you like to try again (Y/N)?");
                            createAccYesNo();
                        }
                        else if (emailMatch == false && phoneSuccess == true && noEmpty == true)
                        {
                            Console.WriteLine("Email is invalid. Would you like to try again (Y/N)?");
                            createAccYesNo();
                        }
                        else if (phoneSuccess == false && emailMatch == true && noEmpty == true)
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

        //Reusable options for yes no choices in create account
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
        //console view for search account
        private void searchAccountView()
        {
            Console.WriteLine("╔═══════════════════════════════════════════════╗");
            Console.WriteLine("║\tSearch for Account:\t\t\t║");
            Console.WriteLine("║═══════════════════════════════════════════════║");
            Console.WriteLine("║ \tAccount Number:\t\t\t\t║");
            Console.WriteLine("╚═══════════════════════════════════════════════╝");
        }
        //search method function. Runs checks to see if an account exists then displays it. Will print console messages if not found or invalid numbers are inputted.
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
                Console.SetCursorPosition(0, 6);
                try
                {
                    if (findAccount(accNumber))
                    {
                        try
                        {
                            Account(Convert.ToInt32(accNumber)).viewAccDetails();//displays the account details
                            Console.WriteLine("Would you like to search for another account Y/N?");
                            searchAccYesNo();

                        }
                        catch (NullReferenceException)//if an account file could not be added to the list of accounts
                        {
                            Console.WriteLine();
                            Console.WriteLine("Error. Account was not created through the application. Try again Y/N");
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
                }
                catch (ArgumentException)//catch for invalid inputs
                {
                    Console.WriteLine();
                    Console.WriteLine("Error. Please return to MainMenu by pressing any key");
                    Console.ReadKey();
                    mainMenu();
                }

            } while (searching == true);

        }

        //method to check if the number from user input matches a text file, if it matches the account file contents are added to an account 
        //list and the method returns true. If the text file cannot be found, return false.
        private bool findAccount(string accNumber)
        {
            string FileName = Path.Combine(accNumber) + ".txt";
            if (accNumber.Length >= 6 && accNumber.Length <= 10 && Int32.TryParse(accNumber, out int accountNumber) && File.Exists(FileName))
            {

                string[] accountFile = File.ReadAllLines(FileName);
                string[] accountInfo = accountFile.Take(7).ToArray(); //takes the first 7 lines of a text file and writes to an array
                List<string> accInfoList = new List<string>();
                foreach (string line in accountInfo)
                {
                    char[] delimiter = { '|' };
                    string[] splitInfo = line.Split(delimiter);
                    string parsedInfo = splitInfo[1].ToString();
                    accInfoList.Add(parsedInfo);
                }

                try //tries to add the file to accounts. Returns true if successful, returns false if an error occurs
                {
                    Accounts validAccount = new Accounts(
                    accInfoList[0], accInfoList[1], accInfoList[2], accInfoList[3], accInfoList[4], Convert.ToInt32(accInfoList[5]), Convert.ToDouble(accInfoList[6]));
                    accounts.Add(validAccount);
                    validAccount.AddOldStatements(); //adds the old statements in the text file to the oldstatements list
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
        //yes no choices for search
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
        //deposit account function. Utilises search and checks for valid inputs. Informs user via console if accounts are not found or invalid inputs
        private void depositAcc()
        {
            Console.Clear();
            depositView();
            Console.SetCursorPosition(23, 5);
            string accNumber = Console.ReadLine();
            Console.SetCursorPosition(18, 6);
            string amount = Console.ReadLine();
            try
            {
                if (findAccount(accNumber))
                {
                    if (Double.TryParse(amount, out double dblAmount))//checks if its a valid number entered
                    {
                        try
                        {
                            Account(Convert.ToInt32(accNumber)).Deposit(dblAmount);// deposit method invoked
                            Console.WriteLine();
                            Console.WriteLine("Amount deposited. Would you like to make another deposit Y/N?");
                            depositYesNo();
                        }
                        catch (NullReferenceException)
                        {
                            Console.WriteLine();
                            Console.WriteLine("Error. Account was not created through the application. Try again with a valid account? Y/N");
                            depositYesNo();
                        }
                    }
                    else
                    {
                        Console.WriteLine();
                        Console.WriteLine("Error: Please enter a valid amount");
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
            catch (ArgumentException)
            {
                Console.WriteLine();
                Console.WriteLine("Error. Please return to MainMenu by pressing any key");
                Console.ReadKey();
                mainMenu();
            }
        }
        //yes no options for deposit
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
        //searches the list of all accounts by looking by account numbers
        private Accounts Account(int accountNumber)
        {
            foreach (Accounts account in accounts)
            {
                if (account.matchNumbers(accountNumber))
                    return account;
            }
            return null;
        }
        //console view of deposit
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
        //withdraw function. Contains checks for valid inputs, enough money being withdrawn. notifies user upon withdraw function failure
        private void withdraw()
        {
            Console.Clear();
            withdrawView();
            Console.SetCursorPosition(23, 5);
            string accNumber = Console.ReadLine();
            Console.SetCursorPosition(17, 6);
            string amount = Console.ReadLine();
            try
            {
                if (findAccount(accNumber))
                {
                    if (Double.TryParse(amount, out double dblAmount))//checks to see if its a valid number being entered
                    {
                        try
                        {
                            if (Account(Convert.ToInt32(accNumber)).checkBalance(dblAmount))//checks to see if an account has enough balance
                            {
                                Account(Convert.ToInt32(accNumber)).Withdraw(dblAmount);//invokes withdraw proess
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
                        catch (NullReferenceException)
                        {
                            Console.WriteLine();
                            Console.WriteLine("Error. Account was not created through the application. Try again with a valid account? Y/N");
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
                    Console.WriteLine();
                    Console.WriteLine("Error. Account Number not found");
                    Console.WriteLine("Would you like to try search again? Y/N");
                    withdrawYesNo();
                }
            }
            catch
            {
                Console.WriteLine();
                Console.WriteLine("Invalid characters entered.");
                Console.WriteLine("Returning to Main menu. Press any key to continue");
                Console.ReadKey();
                mainMenu();
            }

        }
        //console viewfor withdraw
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

        //yes no options for withdraw
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
        //Prints the last 5 statements of an account. Also offers the ability for a user to send the last 5 accounts to email. 
        private void statement()
        {
            Console.Clear();
            statementView();
            Console.SetCursorPosition(23, 5);
            string accNumber = Console.ReadLine();
            Console.WriteLine();
            try
            {
                if (findAccount(accNumber))
                {
                    Account(Convert.ToInt32(accNumber)).PrintStatement();//prints the account details and up to the last 5 statements
                    Console.WriteLine("Email statement? Y/N");
                    char select = '0';
                    bool retryStatement = true;
                    select = readInput().KeyChar;
                    do
                    {
                        switch (select)
                        {
                            case 'y':
                                retryStatement = false;
                                Account(Convert.ToInt32(accNumber)).emailStatement(); //emails the statement
                                Console.WriteLine("Statement Emailed. Would you like to email another statement? Y/N");
                                yesNoStatement();
                                break;
                            case 'n':
                                retryStatement = false;
                                Console.WriteLine("Statement not emailed. Press any key to return to main menu");
                                Console.ReadKey();
                                mainMenu();
                                break;
                            default:
                                retryStatement = true;
                                select = readInput().KeyChar;
                                break;
                        }
                    } while (retryStatement == true);
                }
                else
                    Console.WriteLine("Error. Please enter a valid account number. Try again? Y/N");
                yesNoStatement();
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Invalid characters entered.");
                Console.WriteLine("Returning to Main menu. Press any key to continue");
                Console.ReadKey();
                mainMenu();
            }

        }
        //yes no options for account statements
        private void yesNoStatement()
        {
            char select = '0';
            bool retryStatement = true;
            select = readInput().KeyChar;
            do
            {
                switch (select)
                {
                    case 'y':
                        retryStatement = false;
                        statement();
                        break;
                    case 'n':
                        retryStatement = false;
                        mainMenu();
                        break;
                    default:
                        retryStatement = true;
                        select = readInput().KeyChar;
                        break;
                }
            } while (retryStatement == true);
        }
        //account statement search view
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
        //Deletes an account. Requires user input to find the account number then deletes the file.
        private void deleteAccount()
        {
            Console.Clear();
            deleteView();
            Console.SetCursorPosition(23, 5);
            string accNumber = Console.ReadLine();
            bool deleteConfirm = true;
            char deleteSelect = '0';
            Console.WriteLine();
            try
            {
                if (findAccount(accNumber))
                {
                    try
                    {
                        string FileName = Path.Combine(accNumber) + ".txt";//text file joining with account number
                        Account(Convert.ToInt32(accNumber)).viewAccDetails();//shows the account details
                        Console.WriteLine("Are you sure you want to delete this file Y/N?");

                        while (deleteConfirm == true)
                        {
                            deleteSelect = readInput().KeyChar;
                            switch (deleteSelect)
                            {
                                case 'y':
                                    File.Delete(FileName); //deletes the file
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
                    catch (NullReferenceException)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Error. Account was not created through the application. Try again with a valid account? Y/N");
                        retryDelete();
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
            catch (ArgumentException)
            {
                Console.WriteLine("Invalid characters entered.");
                Console.WriteLine("Returning to Main menu. Press any key to continue");
                Console.ReadKey();
                mainMenu();
            }

        }
        //yes no optons for delete
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
        //view for delete 
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
