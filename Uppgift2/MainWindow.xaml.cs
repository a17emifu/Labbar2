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
using System.IO;
using System.Text.RegularExpressions;


namespace Uppgift2
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    
    public partial class MainWindow : Window
    {

        string AccountFileName, BankFileName, TransaksionsFileName;   // namn för filer
        string Transaksionerstype; // string för transaksions typ

        // Räknare för kund/bankkonto
        int accountNumber = 0;
        int NumberForChecking, NumberForSaving, NumberForRetire;

        // Lister för kunder och bankar där kunderna har
        List<Customer> Customers = new List<Customer>();
        List<BankAccount> CustomersBanks = new List<BankAccount>();
        
        // Lister som har laddas
        List<Customer> LoadedCustomers = new List<Customer>();
        List<BankAccount> LoadedBanks = new List<BankAccount>();

        // Lister för att spara historia transaksioner
        List<Transaction> transactions = new List<Transaction>();
        List<Transaction> Loadedtransactions = new List<Transaction>();

        int nowBalance =0;

        public MainWindow()
        {
            InitializeComponent();
            btnBank.IsEnabled = false;
            btnAddMoney.IsEnabled = false;
            btnCancel.IsEnabled = false;
            btnCredit.IsEnabled = false;
            btnHistory.IsEnabled = false;
            btnDrawMoney.IsEnabled = false;
        }

        // För att spara en fil.bin som innehåller kunders info
        private void SaveFile(Customer c)
        {
            AccountFileName = accountNumber.ToString();
            FileOperations.Serialize(c, AccountFileName);
        }

        // För att sparan en fil.bin för bankkonton 
        private void SaveBankFile(List<BankAccount> b, string fileName)
        {
            FileOperations.Serialize(b, fileName);
        }

        // För att ladda filen  om kunder
        private void LoadFile(Customer c)
        {
            c = (Customer)FileOperations.Deserialize(AccountFileName);
            LoadedCustomers.Add(c);
            LoadInfoInListbox1();
        }

        // Visar kunders info i listbox1
        private void LoadInfoInListbox1()
        {
            foreach (var customer in LoadedCustomers)
            {
                listbox1.Items.Add($"No.{customer.GetCustomerNumber()}: {customer.GetCustomersName()}");
            }
        }

        private void LoadInfoInListbox2()
        {

            foreach (var bank in CustomersBanks)   // visar banksinfo i listbox2
            {
                listbox2.Items.Add($"{bank.AccountType}{bank.AccountNumber}");
            }
        }

        // För att börja om transaktioner
        private void ResetBox()
        {
            listboxBank.Items.Clear();
            boxInputNum.Clear();
            boxInputNum.Focus();
        }

        //  För att förhindra fel som programmet skulle kraschas
        private void PreventionError(BankAccount a)
        {
            //  Om något i listboxen väljs 
            if (listbox1.SelectedIndex >= 0) //.SelectedIndex [https://dobon.net/vb/dotnet/control/lbselectitem.html]
            {
                GetList(a); 
            }

            // Om inget i listboxen bäljs
            else
            {
                MessageBox.Show("Välj någon!"); // ett felmeddelande kommer

                btnAddMoney.IsEnabled = false;
                btnCancel.IsEnabled = false;
                btnCredit.IsEnabled = false;
                btnHistory.IsEnabled = false;
                btnDrawMoney.IsEnabled = false;
            }
        }

        //  För att visa bankars info (vilka konton som kunden har) i listboxen2
        private void GetList(BankAccount account)
        {
            listbox2.Items.Clear();

            foreach (var customer in LoadedCustomers)  // kunder som har registerats placerats genom en foreach-loop
            {
                if (listbox1.SelectedItem.Equals($"No.{customer.GetCustomerNumber()}: {customer.GetCustomersName()}") && (File.Exists($"{customer.GetCustomerNumber()}s bankkonto") == false))    // När användaren väljer någon kund i listbox1
                {
                    customer.AddBankAccounts(account);    // En kund har en bankkonto (läggs i listan)
                    CustomersBanks = customer.BankAccounts;

                    BankFileName = $"{customer.GetCustomerNumber()}s bankkonto";
                    SaveBankFile(CustomersBanks, BankFileName);  // sparar en fil.bin för bankkonto
                    LoadInfoInListbox2();

                }
                else if (listbox1.SelectedItem.Equals($"No.{customer.GetCustomerNumber()}: {customer.GetCustomersName()}") && (File.Exists($"{customer.GetCustomerNumber()}s bankkonto") == true))  // om filen för bankkonton existerats redan
                {
                    BankFileName = $"{customer.GetCustomerNumber()}s bankkonto";
                    LoadedBanks = (List<BankAccount>)FileOperations.Deserialize(BankFileName);  // kallar banksinfo
                    LoadedBanks.Add(account);   // läggs in ett nytt bankkonto
                    SaveBankFile(LoadedBanks, BankFileName);

                    CustomersBanks = LoadedBanks;
                    LoadInfoInListbox2();
                }

            }
        }

        // För att visa banks info (ränta/balans/etc) i listboxBank
        private void ViewListBoxBank()
        {
            string banksdetail;

            foreach (var customer in LoadedCustomers)   // visar kunder som har registerats genom en foreach-loop
            {
                if (listbox1.SelectedItem.Equals($"No.{customer.GetCustomerNumber()}: {customer.GetCustomersName()}"))  // om en kund i listbox1 väljs
                {
                    foreach (var bank in CustomersBanks)    // visar bankkonton där kunden som användaren väljs i listbox1 har
                    {
                        if ((listbox2.SelectedIndex >= 0) && (listbox2.SelectedItem.Equals($"{bank.AccountType}{bank.AccountNumber}")))   // Om något bankkonto i listbox2 väljs
                        {
                            // Enbart Lönekonto kan ha kredit
                            if (bank.CreditLimit >0)
                            {
                                
                                banksdetail = $"{bank.AccountType}{bank.AccountNumber} ränta: {bank.Interest*100} % uttagavgifter: {bank.CommissionPaid*100}% kredit: {bank.Credit} / {bank.CreditLimit} kr. Din balans (saldo + kredit): {nowBalance} kr.";
                                listboxBank.Items.Add(banksdetail);     // banks info (banksdetail) är i listboxBank

                                btnCredit.IsEnabled = true;
                                txtCredit.Text = "";
                            }
                            else    // om konton inte har kredit
                            {
                                banksdetail = $"{bank.AccountType}{bank.AccountNumber} ränta: {bank.Interest*100} % uttagavgifter: {bank.CommissionPaid * 100}% Din balans: {bank.Balance} kr.";
                                listboxBank.Items.Add(banksdetail);     // banks info (banksdetail) är i listboxBank
                                btnCredit.IsEnabled = false;
                                txtCredit.Text = "※ Det här kontot har ingen kredit.";
                            }
                        }
                    }
                }
            }
        }

        //  För att pengar insatts 
        private void BankCalcDeposit(int input)
        {
            foreach (var bank in CustomersBanks)
            {
                if (listbox2.SelectedItem.Equals($"{bank.AccountType}{bank.AccountNumber}"))
                {
                    MessageBox.Show($"Pengarna har insatts. Nu har du {bank.GetDeposit(input).ToString()} kr!");    // polymorfa: GetDeposit() kallas oavsett kontotyp
                    SaveBankFile(CustomersBanks, BankFileName); // uppdateras banks info genom att spara en fil vars namn är samma
                    nowBalance = bank.Balance;
                }
            }
            ResetBox();
            ViewListBoxBank();
        }

        // För att låna pengar (kredit)
        private string BankCalcCredit(int input)
        {
            foreach (var bank in CustomersBanks)
            {
                if (listbox2.SelectedItem.Equals($"{bank.AccountType}{bank.AccountNumber}"))
                {
                    int totalCredit;
                    totalCredit = input + bank.Credit;

                    if(totalCredit <= bank.CreditLimit)    
                    {
                        bank.CreditCalc(input);
                        MessageBox.Show($"Nu har du lånat {bank.Credit} kr! ");
                        SaveBankFile(CustomersBanks, BankFileName);
                        nowBalance = bank.Balance + bank.Credit;
                        Transaksionerstype = "Kredit";
                    }
                    else      // om användaren försöker låna mer pengar än gränsen
                    {
                        MessageBox.Show($"Du kan låna pengar totalt max. {bank.CreditLimit} kr!");
                        Transaksionerstype = "Fel - kunde inte låna pengar";
                    }

                }
            }
            ResetBox();
            ViewListBoxBank();
            return Transaksionerstype;
        }


        // För att pengar uttags
        private string BankCalcWithDraw(int input)
        {
                foreach (var bank in CustomersBanks)
                {
                    if (listbox2.SelectedItem.Equals($"{bank.AccountType}{bank.AccountNumber}"))
                    {
                        double fee = input * bank.CommissionPaid;
                        int diffirence = bank.Balance + bank.Credit - input - (int)fee; // int för att förhindra att kunden ska kunna göra uttag mer pengar än balans

                        if (diffirence >= 0)
                        {
                            MessageBox.Show($"Pengarna har dragit. Nu har du {bank.GetWithdraw(input).ToString()} kr!");    // polymorfa: GetWithdraw() kallas oavsett kontotyp
                            //BankFileName = $"{customer.GetCustomerNumber()}s bankkonto";
                            SaveBankFile(CustomersBanks, BankFileName);  // uppdateras banks info genom att spara en fil vars namn är samma
                            nowBalance = bank.Balance;
                            Transaksionerstype = "Uttag";
                        }
                        else if (diffirence < 0)
                        {
                            MessageBox.Show($"Du kan inte göra uttag pengar mer än din balans: {nowBalance} kr!");
                            Transaksionerstype = "Fel - kunde inte göra uttag";
                        }
                    }

                }
            ResetBox();
            ViewListBoxBank();
            return Transaksionerstype;
        }

        // För att hantera bankkontos nummer ordentligt 
        private int AccountNumberCalc( string accountType)
        {
            int bankNumber = 0;

            if (listbox1.SelectedIndex >= 0)
            {
                foreach (var customer in LoadedCustomers)
                {
                    BankFileName = $"{customer.GetCustomerNumber()}s bankkonto";

                    // Om filen om ett bankkonto redan 
                    if (File.Exists(BankFileName) && (listbox1.SelectedItem.Equals($"No.{customer.GetCustomerNumber()}: {customer.GetCustomersName()}")))
                    {
                        LoadedBanks = (List<BankAccount>)FileOperations.Deserialize(BankFileName);  // filen laddas

                        foreach (var bank in LoadedBanks)
                        {
                            if (accountType == bank.AccountType)
                            {
                                bankNumber = bank.AccountNumber; //läser bankkontos nummer
                            }
                        }
                        bankNumber++;    // nya bankkontos nummer
                        LoadedBanks = new List<BankAccount>();  // för att börja om
                    }

                    // Om filen inte existerats (första gången för att skapa ett konto)
                    else if ((File.Exists(BankFileName) == false) && (listbox1.SelectedItem.Equals($"No.{customer.GetCustomerNumber()}: {customer.GetCustomersName()}")))
                    {
                        bankNumber = 1;
                    }
                }
            }


            return bankNumber;
        }

        private void ButtonControl()
        {
            btnAddMoney.IsEnabled = true;
            btnCancel.IsEnabled = true;
            btnCredit.IsEnabled = true;
            btnDrawMoney.IsEnabled = true;
        }

        // När knappen "Registera" trycks på
        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            btnBank.IsEnabled = true;
            listbox1.Items.Clear();

            string fName, lName, adress;
            string tel;
            fName = boxFName.Text;
            lName = boxLName.Text;
            adress = boxAdress.Text;
            tel = boxCellphone.Text;

            listbox1.SelectedIndex = -1;    // listan väjls inte nu

            // Klassen "Customer" kallas och kundens info läggs in i en List<Customer>
            Customer customer = new Customer();
            accountNumber++;    // räknare för kundnummer blir plus ett
            customer.AddCustomerInfo(fName, lName, adress, tel, accountNumber);
            Customers.Add(customer);

            SaveFile(customer);
            LoadFile(customer);
        }


        // När knappen "Skapa" trycks på
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (rbtnL.IsChecked == true)    // om lönekonto vill skapas
            {
                NumberForChecking = AccountNumberCalc("Lönekonto");   // räknar nya bankkontos nummer

                CheckingAccount checkingAccount = new CheckingAccount();
                checkingAccount.CreateBankAccount("Lönekonto", NumberForChecking);
                ButtonControl();
                PreventionError(checkingAccount);
                NumberForChecking = 0;  // För att börja om
            }

            if(rbtnS.IsChecked == true) // om sparkonto vill skapas
            {
                NumberForSaving = AccountNumberCalc("Sparkonto");

                Savingaccount savingaccount = new Savingaccount();
                savingaccount.CreateBankAccount("Sparkonto", NumberForSaving);
                ButtonControl();
                PreventionError(savingaccount);
                NumberForSaving = 0;    // för att bölja om
            }

            if (rbtnP.IsChecked == true)    // om pensionkonto vill skapas
            {
                NumberForRetire = AccountNumberCalc("Pensionkonto");

                RetirementAccount retirementAccount = new RetirementAccount();
                retirementAccount.CreateBankAccount("Pensionkonto", NumberForRetire);
                ButtonControl();
                PreventionError(retirementAccount);
                NumberForRetire= 0;
            }
        }

        // För att spara en fil för transaksioners
        private void SaveTransaksioner(int inputNum, string banktype)
        {
            Transaction transactionhistory = new Transaction();
            transactionhistory.AddTransactionsInfo(inputNum, banktype);
            transactions.Add(transactionhistory);

            foreach (var customer in LoadedCustomers)
            {
                if (listbox1.SelectedItem.Equals($"No.{customer.GetCustomerNumber()}: {customer.GetCustomersName()}"))
                { 
                    foreach (var bank in CustomersBanks)
                    {
                        if (listbox2.SelectedItem.Equals($"{bank.AccountType}{bank.AccountNumber}"))
                        {
                            TransaksionsFileName = $"{customer.GetCustomerNumber()}s {bank.AccountType}{bank.AccountNumber} historia";
                            FileOperations.Serialize(transactions, TransaksionsFileName);
                        }
                    }
                }
            }
        }

        //  Om knappen "Insättning" trycks på
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            btnHistory.IsEnabled = true;
            string input = boxInputNum.Text;

            if (input == "")    // om användaren inte matar in något alls
            {
                MessageBox.Show("Mata in siffror!");
            }
            else
            {
                int inputNum = int.Parse(boxInputNum.Text);

                // Förhindrar att progrannet skulle kraschas
                if (listbox2.SelectedIndex >= 0) // om något bankkonto väljs
                {
                    BankCalcDeposit(inputNum);
                    SaveTransaksioner(inputNum, "Insättning");

                }
                else { MessageBox.Show("Välj konto!"); }
            }

        }

        // Om knappen "Uttag" trycks på
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            btnHistory.IsEnabled = true;
            string input = boxInputNum.Text;

            if (input == "")    // om användaren inte matar in något alls
            {
                MessageBox.Show("Mata in siffror!");
            }
            else
            {
                int inputNum = int.Parse(boxInputNum.Text);

                // Förhindrar att progrannet skulle kraschas
                if (listbox2.SelectedIndex >= 0)
                {
                    string withDrawsType; // För att visa transaksions typ
                    withDrawsType = BankCalcWithDraw(inputNum);
                    SaveTransaksioner(inputNum, withDrawsType);
                }
                else { MessageBox.Show("Välj konto!"); }
            }
        }

        // När knappen "Kredit" trycks på
        private void btnCredit_Click(object sender, RoutedEventArgs e)
        {
            btnHistory.IsEnabled = true;
            string input = boxInputNum.Text;

            if (input == "")    // om användaren inte matar in något alls
            {
                MessageBox.Show("Mata in siffror!");
            }
            else
            {
                int inputNum = int.Parse(boxInputNum.Text);

                // Förhindrar att progrannet skulle kraschas
                if (listbox2.SelectedIndex >= 0) // om något bankkonto väljs
                {
                    string creditType;
                    creditType = BankCalcCredit(inputNum);
                    SaveTransaksioner(inputNum, creditType);
                }
                else { MessageBox.Show("Välj konto!"); }
            }

        }

        // När knappen "Avregistrera" trycks på
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
                foreach (var accounts in LoadedCustomers)
                {
                    if (listbox1.SelectedItem.Equals($"No.{accounts.GetCustomerNumber()}: {accounts.GetCustomersName()}"))
                    {
                            BankFileName = $"{accounts.GetCustomerNumber()}s bankkonto";
                            AccountFileName = $"{accounts.GetCustomerNumber()}";

                            // Filar om info för kunden som har valts tas bort
                            File.Delete(BankFileName);  
                            File.Delete(AccountFileName);

                            MessageBox.Show("Din konto tas bort.");
                    }
                    
                }

                // För att visa en ny list utan kunden som tas bort i listbox1
                LoadedCustomers.Clear();

                foreach (var customer in Customers)
                {
                    AccountFileName = $"{customer.GetCustomerNumber()}";
                    if (File.Exists(AccountFileName)) { LoadedCustomers.Add((Customer)FileOperations.Deserialize(AccountFileName)); }   // kundersinfo som existerats filen läggs in i List "Customers"
                }

                listbox1.Items.Clear();
                LoadInfoInListbox1();

                listbox1.SelectedIndex = -1;    // För att börja om
                listbox2.SelectedIndex = -1;
        }

        // När knappen "Historia" trycks på
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            listboxBank_History.Items.Clear();

            // Om användaren inte väljer i listboxar
            if ((listbox1.SelectedIndex == -1) || (listbox2.SelectedIndex == -1))
            {
                MessageBox.Show("Välj något konto!");
            }

            else
            {
                foreach (var customer in LoadedCustomers)
                {
                    if (listbox1.SelectedItem.Equals($"No.{customer.GetCustomerNumber()}: {customer.GetCustomersName()}"))
                    {
                        foreach (var bank in CustomersBanks)
                        {
                            if (listbox2.SelectedItem.Equals($"{bank.AccountType}{bank.AccountNumber}"))
                            {
                                TransaksionsFileName = $"{customer.GetCustomerNumber()}s {bank.AccountType}{bank.AccountNumber} historia";

                                if (File.Exists(TransaksionsFileName))
                                {
                                    Loadedtransactions = (List<Transaction>)FileOperations.Deserialize(TransaksionsFileName);
                                    foreach (var t in Loadedtransactions)
                                    {
                                        listboxBank_History.Items.Add($"{t.GetDateTime()} {t.GetTransactionType()}: {t.GetAmount()} kr");
                                    }
                                    Loadedtransactions = new List<Transaction>();   // för att börja om
                                }
                                else
                                {
                                    MessageBox.Show("Du har inte gjort transaksioner hos det här kontot ännu!");
                                    Loadedtransactions = new List<Transaction>();
                                }
                            }
                        }
                    }
                }
            }
        }

        //  När användaren ändrar kunder i listbox1
        private void listbox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // För att börja om
            listbox2.Items.Clear();
            listboxBank_History.Items.Clear();

                if (listbox1.SelectedIndex >= 0)    // förhindrar fel när en kund har avregisterats
                {
                    LoadedBanks = new List<BankAccount>();

                    foreach (var accounts in LoadedCustomers)
                    {
                        if (listbox1.SelectedItem.Equals(($"No.{accounts.GetCustomerNumber()}: {accounts.GetCustomersName()}")) && (System.IO.File.Exists($"{accounts.GetCustomerNumber()}s bankkonto")))
                        {
                            BankFileName = $"{accounts.GetCustomerNumber()}s bankkonto";
                            LoadedBanks = (List<BankAccount>)FileOperations.Deserialize(BankFileName); // laddas en fil om bankkonton där kunden som har valts i lixtbox1 
                            listbox2.Items.Clear();

                            foreach (var a in LoadedBanks)  // bankkonton som laddas visar i listbox2
                            {
                                listbox2.Items.Add($"{a.AccountType}{a.AccountNumber}");
                            }

                            CustomersBanks = LoadedBanks;   // för transaksioner
                        }
                    }

                }
            
            listboxBank.Items.Clear();
        }

        // När användaren ändrar bankkonto i listbox2
        private void listbox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listbox2.SelectedIndex >= 0)    // Om man väljer något i listan
            {
                //  För att börja om
                listboxBank.Items.Clear();
                listboxBank_History.Items.Clear();  
                transactions = new List<Transaction>(); 

                // ladda en fil för transaktioner och läggs in i List<Transaksioner>
                foreach (var customer in LoadedCustomers)
                {
                    if (listbox1.SelectedItem.Equals($"No.{customer.GetCustomerNumber()}: {customer.GetCustomersName()}"))
                    {
                        foreach (var bank in CustomersBanks)
                        {
                            TransaksionsFileName = $"{customer.GetCustomerNumber()}s {bank.AccountType}{bank.AccountNumber} historia";
                            if ((File.Exists(TransaksionsFileName)) && (listbox2.SelectedItem.Equals($"{bank.AccountType}{bank.AccountNumber}")))
                            {
                                transactions = (List<Transaction>)FileOperations.Deserialize(TransaksionsFileName);
                            }
                        }
                    }
                }

                ViewListBoxBank();  // detaljer om bankkontot  
            }
        }

        // Användaren kan mata in bara siffror [https://kagasu.hatenablog.com/entry/2017/02/14/155824]
        private void boxInputNum_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !new Regex("[0-9]").IsMatch(e.Text);
        }

        private void Window_Closing_1(object sender, System.ComponentModel.CancelEventArgs e)   // filar som har sparats tas bort när programmet är stängd
        {
            foreach (var customer in Customers)
            {
                AccountFileName = $"{customer.GetCustomerNumber()}";
                BankFileName = $"{customer.GetCustomerNumber()}s bankkonto";

                foreach (var bank in CustomersBanks)
                {
                    TransaksionsFileName = $"{customer.GetCustomerNumber()}s {bank.AccountType}{bank.AccountNumber} historia";

                    File.Delete(AccountFileName);
                    File.Delete(BankFileName);
                    File.Delete(TransaksionsFileName);
                }
            }
        }
    }
}

