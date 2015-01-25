using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.IO;
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

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        // Alustetaan henkilölista.
        public static List<Person> PersonList = new List<Person>();
        // Alustetaan hakutulokset lista.
        public static List<Person> SearchresultList = new List<Person>();
        // Henkilön id listassa.
        public static int PersonAt = 0;

        public MainWindow()
        {
            InitializeComponent();
       
            // Dummy data
            //PersonList.Add(new Person { PersonId = 1, Name = "Teuvo Testi", Address = "Testikatu 12 D 2", PhoneNumber = "+358505588123", Email="teuvo.testi@gmail.com" });
            //PersonList.Add(new Person { PersonId = 2, Name = "Maija Malli", Address = "Mallitie 18 E 3", PhoneNumber = "+35850123456", Email = "maija.malli@firma.com" });
            //PersonList.Add(new Person {  PersonId = 3, Name = "Matti Meikäläinen", Address = "Jokukatu 8", PhoneNumber = "+358987654321", Email= "matti.meikalainen@hotmail.com" });
            // Lista aakkosjörjestykseen nimen mukaan.
            //PersonList = PersonList.OrderBy(x => x.Name).ToList();

            // Lista myListview:n lähteeksi.
            myListview.ItemsSource = PersonList;

        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // Luodaan lisää henkilö ikkuna.
            AddPersonWindow Addperson = new AddPersonWindow();
            Addperson.Show();
        }

        private void myListview_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Mikäli ei valintaa.
            if (myListview.SelectedIndex == -1)
            {
                // Disabloidaan muokkaa ja lisää napit
                EditButton.IsEnabled = false;
                DeleteButton.IsEnabled = false;
            }
            // muuten...
            else
            {
                // Enabloidaan muokkaa ja lisää napit
                EditButton.IsEnabled = true;
                DeleteButton.IsEnabled = true;

                // Luodaan valinnasta Person olio. 
                Person P = myListview.SelectedItem as Person;
                
                // Näytetään valita tesksikentissä.
                NameLabel.Content = P.Name;
                AddressLabel.Content = P.Address;
                PhoneNumerLabel.Content = P.PhoneNumber;
                EmailLabel.Content = P.Email;
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            // Valittu henkilö oliona.
            Person P = myListview.SelectedItem as Person;

            // Valitun henkilön id:tä vastaaava indeksi listasta.
            PersonAt = PersonList.FindIndex(i => i.PersonId == P.PersonId);

            // Luodaan muokkaa henkilö ikkuna.
            EditPersonWindow EditPerson = new EditPersonWindow();
            EditPerson.Show();
        }

        private void SearchTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Tyhjennetään hakutulokset lista.
            SearchresultList.Clear();

            // Jos hakukenttä on tyhjä näytetään koko henkilölista.
            if (SearchTextbox.Text == "")
            {
                myListview.ItemsSource = PersonList;
                myListview.Items.Refresh();
            }
            // muuten...
            else
            {
               // Käydään läpi henkilölistan henkilöt.
                foreach(Person element in PersonList)
                {
                   // Tarkistetaan löytyykö hakusanaa vastaavaa merkkijonoa henkilön tiedoista.
                    // Isot ja pienet kirjaimet eivät ole merkitseviä.
                   if (element.Name.IndexOf(SearchTextbox.Text, 0, StringComparison.CurrentCultureIgnoreCase) != -1)
                   {
                        // ... jos löytyy, lisätään elementti hakutulos listaan.
                        SearchresultList.Add(element);
                        // ... jatketaan henkilölistan seuraavaan elementtiin. 
                        continue;
                   }
                   if (element.Address.IndexOf(SearchTextbox.Text, 0, StringComparison.CurrentCultureIgnoreCase) != -1)
                   {
                       SearchresultList.Add(element);
                       continue;
                   }
                   if (element.PhoneNumber.IndexOf(SearchTextbox.Text, 0, StringComparison.CurrentCultureIgnoreCase) != -1)
                   {
                       SearchresultList.Add(element);
                       continue;
                   }
                   if (element.Email.IndexOf(SearchTextbox.Text, 0, StringComparison.CurrentCultureIgnoreCase) != -1)
                   {
                       SearchresultList.Add(element);
                   }
                }
                // Vaihdetaan listview:n lähteeksi hakutuloslista.
                myListview.ItemsSource = SearchresultList;
                myListview.Items.Refresh();
            }

        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // Valittu henkilö oliona.
            Person P = myListview.SelectedItem as Person;

            // Varmistetaan poistaminen.
            if (MessageBox.Show("Poistetaankon " + P.Name + " osoitelistasta!", "", MessageBoxButton.YesNo, MessageBoxImage.Question ) == MessageBoxResult.Yes)
            {
                //Valitun henkilön id:tä vastaaava indeksi listasta.
                PersonAt = PersonList.FindIndex(i => i.PersonId == P.PersonId);
                
                // Poistetaan henkilö henkilölistasta.
                PersonList.RemoveAt(PersonAt);
                
                // ... mikäli haku aktiivinen...
                if (SearchTextbox.Text != "")
                {
                    // poistetaan henkilö hakutuloksista.
                    SearchresultList.RemoveAt(myListview.SelectedIndex);
                }
                
                // päivitetään listview.
                myListview.Items.Refresh();
                
                // Tyhjennetään teksitikentät.
                NameLabel.Content = "";
                AddressLabel.Content = "";
                PhoneNumerLabel.Content = "";
                EmailLabel.Content = "";
                
                // Disaploidaan muokkaa ja poista nappulat.
                EditButton.IsEnabled = false;
                DeleteButton.IsEnabled = false;
            }
        }

        private void MenuOpen_Click(object sender, RoutedEventArgs e)
        {
            PersonList.Clear();

            Stream myStream = null;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Avaa osoitelista";
            openFileDialog.Filter = "All files (*.*)|*.*|txt files (*.txt)|*.txt";
            openFileDialog.InitialDirectory = @"C:\";

            bool? result = openFileDialog.ShowDialog();

            if(result == true)
            {
                try
                {
                    if ((myStream = openFileDialog.OpenFile()) != null)
                    {
                        using (StreamReader reader = new StreamReader(myStream))
                        {
                            string line;
                            while ((line = reader.ReadLine()) != null)
                            {
                                Person P = new Person();
                                string[] elements = line.Split('|');
                                P.PersonId = Int32.Parse(elements[0]);
                                P.Name = elements[1];
                                P.Address = elements[2];
                                P.PhoneNumber = elements[3];
                                P.Email = elements[4];

                                PersonList.Add(P);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Virhe avatessa tiedostoa!: " + ex.Message);
                }

                myListview.Items.Refresh();
            }
        }

        private void MenuClose_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void MenuSave_Click(object sender, RoutedEventArgs e)
        {
           
            Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "All files (*.*)|*.*|txt files (*.txt)|*.txt";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            bool? result = saveFileDialog1.ShowDialog();

            if (result == true)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                  
                    using (StreamWriter writer = new StreamWriter(myStream))
                    {
                        foreach (Person element in PersonList)
                        {
                            writer.WriteLine(element.PersonId + "|" + element.Name + "|" + element.Address + "|" + element.PhoneNumber + "|" + element.Email);
                        }
                    }
                    myStream.Close();
                }
            }

        }

    }
    public class Person{
        public int PersonId;
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
    
}
