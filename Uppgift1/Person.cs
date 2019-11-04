using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;

namespace Uppgift1
{
    [Serializable]   // peker ut klassen för metoden "Serialize"
   
    // Klassen Person: om människors namn, ålder och godisar
    public class Person
    {
        private string FirstName { get; set; }
        private string LastName { get; set; }
        private int Age { get; set; }
        private string FullName {get; set; }
        private int AmountOfCandy { get; set; }

        // För att få människors information som matas in 
        public void AddPersonInfo(string a, string b, int c)
        {
            FirstName = a;
            LastName = b;
            FullName = $"{FirstName} {LastName}";
            Age = c;
            AmountOfCandy = 0;
        }

        // Människors namn kallas
        public string GetPersonName()
        {
            return FullName;
        }

        // Kan få infomation hur många godisar som är nu
        public int GetAmountOfCandy()
        {
            return AmountOfCandy;
        }

        // Mennidkors ålder kallas
        public int GetAge()
        {
            return Age;
        }

        // Räknare för godisar
        public void AddCandy()
        {
            AmountOfCandy = AmountOfCandy + 1;
        }

        // Rensa antalet godisar
        public void ResetAmountOfCandy()
        {
            AmountOfCandy = 0;
        }

    }

}
