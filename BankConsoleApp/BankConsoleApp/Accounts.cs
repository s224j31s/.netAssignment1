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
    public class Accounts
    {
        private int accNumber;
        private string firstName;
        private string lastName;
        private string address;
        private string phoneNo;
        private double balance;
        private string email;
        private List<Transactions> bankStatement;
        private List<string> oldStatements;
        private List<string> infoList = new List<string>();
        private List<string> fiveStatements = new List<string>();
        private List<string> emailList = new List<string>();
        private List<string> emailStatementfive = new List<string>();

        public Accounts(string firstName, string lastName, string address, string phoneNo, string email, int accNumber, double balance)
        {
            this.AccNumber = accNumber;
            this.firstName = firstName;
            this.lastName = lastName;
            this.address = address;
            this.phoneNo = phoneNo;
            this.balance = balance;
            this.email = email;
            bankStatement = new List<Transactions>();
            oldStatements = new List<string>();

        }


        //method for depositing money into an account. Updates File afterwards
        public void Deposit(double amount)
        {
            balance += amount;
            depositStatement(amount);
            string FileName = Path.Combine(accNumber.ToString()) + ".txt";//text file joining with account number
            string statementLists = "";
            string existingStatements = "";

            foreach (Transactions transaction in bankStatement)
            {
                statementLists += transaction.fileString();
            }

            foreach (string line in oldStatements)
            {
                existingStatements += line + "\n";
            }
            existingStatements += statementLists;
            using (StreamWriter accDetails = new StreamWriter(FileName))//update account balance
            {
                accDetails.WriteLine("First Name|" + firstName);
                accDetails.WriteLine("Last Name|" + lastName);
                accDetails.WriteLine("address|" + address);
                accDetails.WriteLine("Phone Number|" + phoneNo);
                accDetails.WriteLine("email|" + email);
                accDetails.WriteLine("Account Number|" + accNumber);
                accDetails.WriteLine("Balance|" + balance);
                accDetails.WriteLine(existingStatements.Trim());
            }

        }

        private void depositStatement(double amount)
        {
            bankStatement.Add(new Transactions(DateTime.Now, "Deposit", amount, balance));
        }

        //withdraw money from user account and then updates the file as required
        public void Withdraw(double amount)
        {
            string FileName = Path.Combine(accNumber.ToString()) + ".txt";//text file joining with account number

            if (balance >= amount)
            {
                balance -= amount;
                withdrawStatement(amount);
                string statementLists = "";
                string existingStatements = "";
                foreach (Transactions transaction in bankStatement)
                {
                    statementLists += transaction.fileString();
                }

                foreach (string line in oldStatements)
                {
                    existingStatements += line + "\n";
                }
                existingStatements += statementLists;

                using (StreamWriter accDetails = new StreamWriter(FileName))//update account balance
                {
                    accDetails.WriteLine("First Name|" + firstName);
                    accDetails.WriteLine("Last Name|" + lastName);
                    accDetails.WriteLine("address|" + address);
                    accDetails.WriteLine("Phone Number|" + phoneNo);
                    accDetails.WriteLine("email|" + email);
                    accDetails.WriteLine("Account Number|" + accNumber);
                    accDetails.WriteLine("Balance|" + balance);
                    accDetails.WriteLine((existingStatements).Trim());
                }
            }

        }
        //adds a withdraw statement
        private void withdrawStatement(double amount)
        {
            bankStatement.Add(new Transactions(DateTime.Now, "Withdraw", amount, balance));
        }
        //adds statements that already exist in the text file
        public void AddOldStatements()
        {
            string FileName = Path.Combine(accNumber.ToString()) + ".txt";
            string[] accountFile = File.ReadAllLines(FileName);
            string[] existingStatements = accountFile.Skip(7).ToArray();

            foreach (string transaction in existingStatements)
            {
                oldStatements.Add(transaction);
            }
        }

        
        public bool matchNumbers(int accountNumber)
        {
            return this.AccNumber == accountNumber;
        }
        //check if there is enough balance in an account
        public bool checkBalance(double enoughBalance)
        {
            return balance >= enoughBalance;
        }
        //Contains the code to show an accounts details after a search
        public void viewAccDetails()
        {
            Console.WriteLine();
            Console.WriteLine(" ╔════════════════════════════════════════════════╗");
            Console.WriteLine(" ║                                                ║");
            Console.WriteLine(" ║                 ACCOUNT DETAILS                ║");
            Console.WriteLine(" ║════════════════════════════════════════════════║");
            Console.WriteLine(" ║                                                ║");
            Console.WriteLine($" ║    Account Number: {accNumber}".PadRight(50, ' ') + "║");
            Console.WriteLine($" ║    Account Balance: ${balance:0.00}".PadRight(50, ' ') + "║");
            Console.WriteLine($" ║    First Name: {firstName}".PadRight(50, ' ') + "║");
            Console.WriteLine($" ║    Last Name: {lastName}".PadRight(50, ' ') + "║");
            Console.WriteLine($" ║    Address: {address}".PadRight(50, ' ') + "║");
            Console.WriteLine($" ║    Phone: {phoneNo}".PadRight(50, ' ') + "║");
            Console.WriteLine($" ║    Email: {email}".PadRight(50, ' ') + "║");
            Console.WriteLine(" ╚════════════════════════════════════════════════╝");
            Console.WriteLine();
        }


        //method to email the user the last 5 statements
        public void emailStatement()
        {
            string FileName = Path.Combine(accNumber.ToString()) + ".txt";
            string[] statementFile = File.ReadAllLines(FileName);
            string[] existingStatements = statementFile.Skip(7).ToArray();
            //adds all statements to the email list
            emailList.Clear();
            foreach (string line in existingStatements)
            {
                emailList.Add(line);
            }
            
            emailList.Reverse();//reverses the list
            string[] lastStatements = emailList.Take(5).ToArray();// takes the first 5 elements of the list and puts into an array

            emailStatementfive.Clear();
            emailStatementfive.AddRange(lastStatements);//adds the 5 statements to be sent
            string fiveEmail = "";
            foreach (string statement in emailStatementfive)
            {
                char[] delimiter = { '|' };
                string[] txtInfo = statement.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < 1; i++) {
                    fiveEmail += string.Format($"<tr><td>{txtInfo[0]}</td>" +
                                    $"<td>{txtInfo[1]}</td><td>{txtInfo[2]}</td>" +
                                    $"<td>{txtInfo[3]}</td></tr>");
                }
            }
         
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.Credentials = new NetworkCredential("dotnetassignmentbank32@gmail.com", "Bankconsoleapp1");
            client.EnableSsl = true;
            try
            {
                MailMessage message = new MailMessage(new MailAddress("dotnetassignmentbank32@gmail.com", "Simple Bank"),
                                  new MailAddress("dotnetassignmentbank32@gmail.com", firstName));
                message.IsBodyHtml = true;

                message.Subject = "Welcome to Simple Bank";
                message.Body = 

                string.Format($"Dear {firstName},<br><br>") +
                "Please find your account statements below:<br><br>" +

                "<table>" +
                    "<tr><th>Date</th><th>Description</th><th>Amount</th>" +
                    "<th>Balance</th></tr>" +
                 fiveEmail +
                "</table><br>" +

                "Kind regards,<br>" +
                "The Simple Banking Team";
                client.Send(message);
            }
            catch
            {
                
            }
           

        }
        //Code to take the last 5 statements from the account file and then display it to the user
        public void PrintStatement()
        {
            string FileName = Path.Combine(accNumber.ToString()) + ".txt";
            string[] statementFile = File.ReadAllLines(FileName);
            string[] existingStatements = statementFile.Skip(7).ToArray();

            infoList.Clear();
            foreach (string line in existingStatements)
            {
                infoList.Add(line);
            }
            foreach (Transactions transaction in bankStatement)
            {
               // infoList.Add(transaction.statementString());
            }

            infoList.Reverse();
            string[] lastStatements = new string[5];
            if (infoList.Count() <5)
            {
              lastStatements = infoList.Take(infoList.Count()).ToArray();
            }
            else if(infoList.Count()>= 5)
            {
                lastStatements = infoList.Take(5).ToArray();
            }
            
         
            fiveStatements.Clear();
            fiveStatements.AddRange(lastStatements);
            Console.WriteLine();
            Console.WriteLine(" ╔═══════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine(" ║                                                                       ║");
            Console.WriteLine(" ║                 ACCOUNT DETAILS                                       ║");
            Console.WriteLine(" ║═══════════════════════════════════════════════════════════════════════║");
            Console.WriteLine(" ║                                                                       ║");
            Console.WriteLine($" ║    Account Number: {accNumber}".PadRight(73, ' ') + "║");
            Console.WriteLine($" ║    Account Balance: ${balance:0.00}".PadRight(73, ' ') + "║");
            Console.WriteLine($" ║    First Name: {firstName}".PadRight(73, ' ') + "║");
            Console.WriteLine($" ║    Last Name: {lastName}".PadRight(73, ' ') + "║");
            Console.WriteLine($" ║    Address: {address}".PadRight(73, ' ') + "║");
            Console.WriteLine($" ║    Phone: {phoneNo}".PadRight(73, ' ') + "║");
            Console.WriteLine($" ║    Email: {email}".PadRight(73, ' ') + "║");
            Console.WriteLine(" ╚═══════════════════════════════════════════════════════════════════════╝");
            Console.WriteLine(" ║                                                                       ║");
            Console.WriteLine(" ║               Last Five Statements                                    ║");
            Console.WriteLine(" ║═══════════════════════════════════════════════════════════════════════║");
            Console.WriteLine(" ║                                                                       ║");

            try
            {
                foreach (string statement in fiveStatements)
                {
                    char[] delimiter = { '|' };
                    string[] txtInfo = statement.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
                    string statementString = String.Format("Date: " + txtInfo[0] + " " + txtInfo[1] + " Amount: " + txtInfo[2] + " Current Balance: " + txtInfo[3]);

                    Console.WriteLine($" ║{statementString}".PadRight(73, ' ') + "║");
                 
                }
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine(" ║                                                                       ║");
                Console.WriteLine(" ║                                                                       ║");
                Console.WriteLine(" ║ Error. Statements could not be acessed                                ║");
            }

            Console.WriteLine(" ╚═══════════════════════════════════════════════════════════════════════╝");
            Console.WriteLine();
        }

        public string FirstName { get => firstName; set => firstName = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public string Address { get => address; set => address = value; }
        public string PhoneNo { get => phoneNo; set => phoneNo = value; }
        public string Email { get => email; set => email = value; }
        public double Balance { get => balance; set => balance = value; }
        public int AccNumber { get => accNumber; set => accNumber = value; }


    }
}
