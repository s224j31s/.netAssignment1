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
        private List<string> noDupes;

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
            noDupes = new List<string>();
            oldStatements = new List<string>();

        }


        //method for depositing money into an account
        public void Deposit(double amount)
        {
            balance += amount;
            depositStatement(amount);
            //List<string> newList = new List<string>(oldStatements);
            //oldStatements.Clear();
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

            string fname = ("First Name|" + firstName);
            string lname = ("First Name|" + lastName);
            string addr = ("address|" + address);
            string phoneNumber = ("Phone Number|" + phoneNo.ToString());
            string accEmail = ("First Name|" + email.ToString());
            string accountNumber = ("Account Number|" + accNumber.ToString());
            string accBalance = ("Balance|" + balance.ToString());


           // List<string> fileContent = new List<string> { fname, lname, addr, phoneNumber, accEmail, accountNumber,accBalance};
            //fileContent.AddRange(oldStatements);

           //oldStatements.Add(statementLists);
            using (StreamWriter accDetails = new StreamWriter(FileName))//update account balance
            {
                // foreach (string s in fileContent) {
                //  accDetails.WriteLine(s);
                //}
                accDetails.WriteLine("First Name|" + firstName);
                accDetails.WriteLine("Last Name|" + lastName);
                accDetails.WriteLine("address|" + address);
                accDetails.WriteLine("Phone Number|" + phoneNo);
                accDetails.WriteLine("email|" + email);
                accDetails.WriteLine("Account Number|" + accNumber);
                accDetails.WriteLine("Balance|" + balance);
                accDetails.WriteLine(existingStatements.Trim());
               // accDetails.WriteLine((statementLists).Trim());
                //foreach (string line in noDupes)
                // {
                //  accDetails.WriteLine((line).Trim());
                //}
                //foreach(string s in oldStatements)
                // accDetails.WriteLine((s).Trim());
                // 


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

                string fname = ("First Name|" + firstName);
                string lname = ("First Name|" + lastName);
                string addr = ("address|" + address);
                string phoneNumber = ("Phone Number|" + phoneNo.ToString());
                string accEmail = ("First Name|" + email.ToString());
                string accountNumber = ("Account Number| " + accNumber.ToString());
                string accBalance = ("Balance|" + balance.ToString());

                foreach (Transactions transaction in bankStatement)
                {
                    statementLists += transaction.fileString();
                }

                foreach (string line in oldStatements)
                {
                    existingStatements += line + "\n";
                }

                //noDupes = oldStatements.Distinct().ToList();
                 existingStatements += statementLists;
                // oldStatements.Add(statementLists);
                using (StreamWriter accDetails = new StreamWriter(FileName))//update account balance
                {
                    accDetails.WriteLine("First Name|" + firstName);
                    accDetails.WriteLine("Last Name|" + lastName);
                    accDetails.WriteLine("address|" + address);
                    accDetails.WriteLine("Phone Number|" + phoneNo);
                    accDetails.WriteLine("email|" + email);
                    accDetails.WriteLine("Account Number|" + accNumber);
                    accDetails.WriteLine("Balance|" + balance);
                  //   foreach (string line in noDupes)
                    //{
                      //accDetails.WriteLine((line).Trim());
                    //}
                    accDetails.WriteLine((existingStatements).Trim());
                   // accDetails.WriteLine(statementLists);

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
              //  oldStatements.Clear();
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
            
            List<string> infoList = new List<string>();
            foreach (string line in oldStatements)
            {
                infoList.Add(line);
            }
            foreach (Transactions transaction in bankStatement)
            {
                infoList.Add(transaction.statementString());
            }

            infoList.Reverse();
            string[] lastStatements = infoList.Take(5).ToArray();
            List<string> fiveStatements = new List<string>();
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

            try {
                foreach (string statement in fiveStatements)
                {
                    char[] delimiter = { '|' };
                    string[] txtInfo = statement.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
                    string statementString = String.Format("Date: " + txtInfo[0] + " " + txtInfo[1] + " Amount: " + txtInfo[2] + " Current Balance: " + txtInfo[3]);

                    Console.WriteLine($" ║{statementString}".PadRight(73, ' ') + "║");
                    //Console.WriteLine(" " + statement);
                }
            }
            catch(IndexOutOfRangeException) {
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
