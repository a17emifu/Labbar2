using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace Uppgift1
{
    // Klassen för att slippa mata in personer
    static class FileOperations
    {
        // För att filen sparas med en binary formatter
        public static void Serialize(List<Person> a)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (Stream fStream = new FileStream("personfile", FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
            {
                formatter.Serialize(fStream, a);
            }
        }

        // Filen som har skapats kallas och öppnas
        public static object Deserialize(string fileName)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (Stream fStream = File.OpenRead(fileName))
            {
                return formatter.Deserialize(fStream);
            }
        }
    }
}
