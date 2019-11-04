using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uppgift2
{
    [Serializable]
    class Customer
    {
        //  Inkapsling - properties är ptivata + de kan sätta och läsa genom enbart publika metoder
        string firstname { get; set; }
        string lastname { get; set; }
        string address { get; set; }
        string fullname{ get; set; }
        string cellphone { get; set; }
        int CustomerNumber { get; set; }

        public List<BankAccount> BankAccounts { get; private set; } // En associasion med inkapsling
        
        //  Konstruktor
        public Customer()
        {
            BankAccounts = new List<BankAccount>();
        }

        // ↓↓Publika metoder
        // För att lägga in kunders info
        public void AddCustomerInfo (string a, string b, string c, string d, int number)
        {
            firstname = a;
            lastname = b;
            fullname = $"{a} {b}";
            address = c;
            cellphone = d;
            CustomerNumber = number;
        }

        public void AddBankAccounts(BankAccount a)
        {
            BankAccounts.Add(a);
        }
        // Visar kundens namn
        public string GetCustomersName()
        {
            return fullname;
        }
        // Visar kontonummer
        public int GetCustomerNumber()
        {
            return CustomerNumber;
        }


    }
}
