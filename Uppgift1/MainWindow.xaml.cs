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
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        // Lister för personers info
        List<Person> personlist = new List<Person>();
        List<Person> sortedlist = new List<Person>();
        List<Person> filelist = new List<Person>();
        
        // Metoden för att visa personers namn och godisar som personerna har fått
        private void GetList(List<Person> a)
        {
            listboxPersons.Items.Clear();   //för att börja om
            foreach (var item in a) // Innehåll i listen placeras
            {
                listboxPersons.Items.Add($"{item.GetPersonName()}: {item.GetAmountOfCandy()} godisar");     // visar personsnamn och antalet godisar där personen har fått
            }
        }

        // Metoden för att sortera innehåll i listen av ålder
        private void SortByAge()
        {
            listboxPersons.Items.Clear(); // för att börja om
            sortedlist = personlist.OrderBy(p => p.GetAge()).ToList(); // listen sorteras: ung → gammal personer
        }

        // Metoden för att rensa lådorna
        private void Clear()
        {
            boxFirstName.Clear();
            boxlastName.Clear();
            boxAge.Clear();
            boxFirstName.Focus(); 
        }

        // Metoden för att börja om 
        private void ResetCandyInList(List<Person> a)
        {
            foreach (var item in a)
            {
                item.ResetAmountOfCandy(); // godisar blir 0
            }
        }

        // Metoden för att spara en fil
        private void SaveFile()
        {
            FileOperations.Serialize(personlist);
        }

        // Metoden för att kalla en fil som har sparats
        private void LoadFile()
        {
            filelist = (List<Person>)FileOperations.Deserialize("personfile"); // filen som har sparats kallas
            personlist = filelist;
            GetList(filelist);
        }

        // Metoden för att undersöka om det finns en fil eller inte
        private void FileChecker()
        {
            if (System.IO.File.Exists("personfile"))    // Om det finns någon fil.bin 
            {
                LoadFile(); // metoden för att kalla filen som har sparats 
            }
            else //Om det inte finns 
            {
                filelist = new List<Person>();  // listen blir tom bara
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            boxFirstName.Focus();
            FileChecker();  // metoden för att hantera en fil.bin kallas

        }

        // När knappen "Lägg till" trycks på
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {

            string firstName, lastName;
            firstName = boxFirstName.Text;
            lastName = boxlastName.Text;
            int age = int.Parse(boxAge.Text);

            Person person = new Person();   // klassen kallas
            person.AddPersonInfo(firstName, lastName, age);     // personens info skickas
            personlist.Add(person);     // personens info läggs in i listen

            GetList(personlist);     // metoden för att personens info visas i listlådan
            
            Clear();    // metoden för att börja om kallas
        }

        // När knappen "Fördela i inmatningsordning" trycks på
        private void btnDiv_Click(object sender, RoutedEventArgs e)
        {
            int inputNum;
            inputNum = int.Parse(boxCandy.Text);

            CandyCalculator candyCalculator = new CandyCalculator();    // klassen för att hantera godisar kallas
            candyCalculator.CandyCalc(inputNum, personlist);    // metoden för att dela godisar kallas

            GetList(personlist); 
            ResetCandyInList(personlist);     // metoden för att börja om kallas
        }

        // När knappen "Fördela efter ålder" trycks på
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int inputNum;
            inputNum = int.Parse(boxCandy.Text);

            CandyCalculator candyCalculator = new CandyCalculator();

            SortByAge();    // metoden för att sortera listen kallas 

            candyCalculator.CandyCalc(inputNum, sortedlist);
            GetList(sortedlist);
            ResetCandyInList(sortedlist);
        }

        // När knappen "Spara" trycks på
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SaveFile();
            MessageBox.Show("Filen har sparats!");
            boxCandy.Clear();
        }

        // När knappen "Ladda file" trycks på
        private void btntest_Click(object sender, RoutedEventArgs e)
        {
            FileChecker();
        }

        // När knappen "Rensa" trycks på
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            listboxPersons.Items.Clear();
            personlist.Clear();
            boxCandy.Clear();
        }
    }
}
