using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uppgift2
{
    [Serializable]
    abstract class BankAccount  // abstrakta klassen
    {
        // Properties med inkapsling: private (för att de används bara här)
        public int AccountNumber{get; private set;} 

        // Properties med inkapsling
        public int Balance { get; protected set; }
        public string AccountType { get; protected set; }

        // För barn klasser
        public double Interest { get; protected set; }
        public double CommissionPaid { get; protected set; }
        public int Credit { get; protected set; }
        public int CreditLimit { get; protected set; }

        // Konstruktorn
        public BankAccount()
        {
            Balance = 0;
        }
       
        // För att skapa ett bankkonto
        public void CreateBankAccount(string account, int accountnumber)
        {
            AccountType = account;
            AccountNumber = accountnumber;
        }

        // När kunden vill sätta in pengar
        public int GetDeposit(int depositMoney)
        {
            Balance = Balance + depositMoney;
            return Balance;
        }

        // När kunden vill ta ut pengar
        public virtual int GetWithdraw(int withDrawMoney)
        {
            Balance = Balance - withDrawMoney;
            return Balance;
        }

        // När kunden vill låna pengar
        public void CreditCalc(int creditMoney)
        {
            Credit = Credit + creditMoney;
        }


    }
}
