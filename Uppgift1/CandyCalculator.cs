using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uppgift1
{
    class CandyCalculator
    {
        // Metoden för att dela godisar
        public void CandyCalc(int d, List<Person> b) // antalet godisar som matas in och en list som ska användas kallas
        {
            int totalCandy = d;

            while (totalCandy > 0)  // while-satsen för att försötta loopen tills godisar blir 0
            {
                foreach (var item in b) // foreach för att dela ut godisar en och en
                {
                    if (totalCandy > 0)
                    {
                        item.AddCandy(); // en godis ges en person
                        totalCandy = totalCandy - 1; // en godis tas bort från antalet godisar
                    }
                }
            }
        }
    }
}
