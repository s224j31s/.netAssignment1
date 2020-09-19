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
        //formats certain statements into a prescribed format and adds a new line.
        public string fileString()
        {
            return string.Format($"{date.ToString("dd.MM.yyyy")}|" +
                                 $"{type}|{debitCredit}|{balance}\n");
        }
        //formats certain statements into a prescribed format
        public string statementString()
        {
            return string.Format($"{date.ToString("dd.MM.yyyy")}|" +
                                $"{type}|{debitCredit}|{balance}");
        }
    }
}
