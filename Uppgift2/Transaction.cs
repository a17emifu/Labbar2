using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uppgift2
{
    [Serializable]
    class Transaction
    {
        private int Amount { get; set; }
        private DateTime DateTime { get; set; }
        private string TransactionType { get; set; }

        public void AddTransactionsInfo(int a, string b)
        {
            DateTime = DateTime.Now;
            Amount = a;
            TransactionType = b;
        }

        public int GetAmount()
        {
            return Amount;
        }

        public DateTime GetDateTime()
        {
            return DateTime;
        }

        public string GetTransactionType()
        {
            return TransactionType;
        }
    }
}
