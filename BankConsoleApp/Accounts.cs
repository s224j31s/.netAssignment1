using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private List<string> newStatements;

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
            newStatements = new List<string>();
            oldStatements = new List<string>();

        }


        //method for depositing money into an account
        public void Deposit(double amount)
        {
            balance += amount;
            depositStatement(amount);

            string FileName = Path.Combine(accNumber.ToString()) + ".txt";//text file joining with account number

            //File.WriteAllText(string.Format($"{accNumber}.txt"),
            //string.Format($"{accountNumber}\n{firstName}\n{lastName}\n" +
            //$"{address}\n{phoNumber}\n{balance:0.00}\n{email}\n{statementStringBlock}"));
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
                foreach (string line in oldStatements)
                {
                    accDetails.WriteLine(line);
                }
                //accDetails.WriteLine(existingStatements);
                accDetails.WriteLine(statementLists);
            }

        }

        private void depositStatement(double amount)
        {
            bankStatement.Add(new Transactions(DateTime.Now, "Deposit", amount, balance));
        }


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
                    foreach (string line in oldStatements)
                    {
                        accDetails.WriteLine(line);
                    }
                    //accDetails.WriteLine(existingStatements);
                    accDetails.WriteLine(statementLists);

                }
            }

        }

        private void withdrawStatement(double amount)
        {
            bankStatement.Add(new Transactions(DateTime.Now, "Withdraw", amount, balance));
        }
        public void AddOldStatements()
        {
            string FileName = Path.Combine(accNumber.ToString()) + ".txt";
            string[] accountFile = File.ReadAllLines(FileName);
            string[] existingStatements = accountFile.Skip(7).ToArray();
            
            foreach (string transaction in existingStatements)
            {
                ///char[] delimiter = { '|' };
                //string[] txnInfo = transaction.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
                oldStatements.Add(transaction);
                // bankStatement.Add(new Transactions(Convert.ToDateTime(txnInfo[0]), txnInfo[1], Convert.ToDouble(txnInfo[2]),
                //        Convert.ToDouble(txnInfo[3])));

            }
        }


        public bool matchNumbers(int accountNumber)
        {
            return this.AccNumber == accountNumber;
        }

        public bool checkBalance(double enoughBalance)
        {
            return balance >= enoughBalance;
        }

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

        public void PrintStatement()
        {
            Console.WriteLine(" Date".PadRight(29, ' ') + "Description".PadRight(24, ' ') + "Debit".PadRight(16, ' ') +
                              "Credit".PadRight(16, ' ') + "Balance".PadRight(16, ' '));
            Console.WriteLine(" ".PadRight(100, '-'));
            List<string> infoList = new List<string>();
            foreach (string line in oldStatements)
            {
                //Console.WriteLine(" " + line);
                infoList.Add(line);

            }
            foreach (Transactions transaction in bankStatement) {
                //Console.WriteLine(" " + transaction.fileString());
                infoList.Add(transaction.fileString());
            }

            infoList.Reverse();
            string[] lastStatements = infoList.Take(5).ToArray();
            List<string> fiveStatements = new List<string>();
            fiveStatements.AddRange(lastStatements);
            foreach (string statement in fiveStatements) {
                
                Console.WriteLine(" " + statement);
            }
                
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
