using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BankConsoleApp
{
    public class Transactions
    {
        private DateTime date;
        private string type;
        private double debitCredit;
        private double balance;

        public Transactions(DateTime date, string type, double debitCredit, double balance)
        {
            this.date = date;
            this.type = type;
            this.debitCredit = debitCredit;
            this.balance = balance;
        }

        public string fileString()
        {
            return string.Format($"{date.ToString("dd/MM/yyyy")}|" +
                                 $"{type}|{debitCredit}|{balance}\n");
        }

        public string statementString()
        {
            return string.Format($"{date.ToString("dd/MM/yyyy")}|" +
                                $"{type}|{debitCredit}|{balance}\n");
        }
    }
}
