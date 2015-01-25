using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for AddPersonWindow.xaml
    /// </summary>
    public partial class AddPersonWindow : Window
    {
       
        public AddPersonWindow()
        {
            InitializeComponent();
        }

        private void SavePersonButton_Click(object sender, RoutedEventArgs e)
        {
            // Luodaan henkilö olio.
            Person MyPerson = new Person();

            // Seuraava Person Id, eli suurin PersonId + 1.
            // Jos kyseessä on ensimmäinen lisättävä henkilö, PersonId on 1
            try
            {
                MyPerson.PersonId = MainWindow.PersonList.Max(t => t.PersonId) + 1;
            }
            catch
            {
                MyPerson.PersonId = 1;
            }

            // Henkilön tiedot Textboxeista.
            MyPerson.Name = NameTextbox.Text;
            MyPerson.Address = AddressTextbox.Text;
            MyPerson.PhoneNumber = PhoneNumberTextbox.Text;
            MyPerson.Email = EmailTextbox.Text;

            // Lisätään henkilö olio listaan.
            MainWindow.PersonList.Add(MyPerson);

            // Henkilölista aakkosjörjestykseen nimen mukaan.
            MainWindow.PersonList = MainWindow.PersonList.OrderBy(x => x.Name).ToList();
           
            // Päivitätään Mainwindow:n listview ja tekstikentät. 
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(MainWindow))
                {
                    (window as MainWindow).myListview.ItemsSource = MainWindow.PersonList;
                    (window as MainWindow).SearchTextbox.Text = "";
                    (window as MainWindow).NameLabel.Content = MyPerson.Name;
                    (window as MainWindow).AddressLabel.Content = MyPerson.Address;
                    (window as MainWindow).PhoneNumerLabel.Content = MyPerson.PhoneNumber;
                    (window as MainWindow).EmailLabel.Content = MyPerson.Email;
                }
            }

            // Suljetaan ikkuna.
            this.Close();
            
        }

        private void CancellButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
