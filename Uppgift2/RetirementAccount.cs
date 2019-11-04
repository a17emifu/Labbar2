using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uppgift2
{
    [Serializable]
    class RetirementAccount: BankAccount
    {
        // Konstruktorn
        public RetirementAccount()
        {
            Interest = 0.03;    // ränta 3%
            CommissionPaid = 0.1;   // 10% av uttaget belopp
            Credit = 0;
            CreditLimit = 0; // har ingen kredit
        }

        public override int GetWithdraw(int withDrawMoney)
        {
            int fee;
            fee = (int)SetDrawFees(withDrawMoney);
            Balance = Balance - withDrawMoney - fee;

            return Balance;
        }

        // Uttagavgift räknas
        private double SetDrawFees(int withDrawMoney)
        {
            double withDrawFees;
            withDrawFees = withDrawMoney * CommissionPaid;
            return withDrawFees;
        }
    }
}
