using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uppgift2
{
    [Serializable]
    class CheckingAccount: BankAccount
    {
        // Konstruktorn
        public CheckingAccount()
        {
            Interest = 0;   // ingen ränta
            CommissionPaid = 0; // inga uttagsavfgifter
            Credit = 0;
            CreditLimit = 25000;
        }

        public override int GetWithdraw(int withDrawMoney)
        {
            Balance = Balance + Credit - withDrawMoney;
            return Balance;
        }

    }
}

