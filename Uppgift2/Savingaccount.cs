using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uppgift2
{
    [Serializable]
    class Savingaccount: BankAccount
    {
        // Konstruktorn
        public Savingaccount()
        {
            Interest = 0.005;   // ränta 0.5%
            CommissionPaid = 0; // inga uttagavgifter

            Credit = 0;
            CreditLimit = 0; // har ingen kredit
        }


    }
}
