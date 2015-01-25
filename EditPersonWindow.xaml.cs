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
    /// Interaction logic for EditPersonWindow.xaml
    /// </summary>
    public partial class EditPersonWindow : Window
    {
       
        public EditPersonWindow()
        {
            InitializeComponent();
            
            // Muokattavan hankilön tiedot tekstikenttiin. 
            NameTextbox.Text = MainWindow.PersonList[MainWindow.PersonAt].Name;
            AddressTextbox.Text = MainWindow.PersonList[MainWindow.PersonAt].Address;
            PhoneNumberTextbox.Text = MainWindow.PersonList[MainWindow.PersonAt].PhoneNumber;
            EmailTextbox.Text = MainWindow.PersonList[MainWindow.PersonAt].Email;
        }

        private void SavePersonButton_Click(object sender, RoutedEventArgs e)
        {  
            // Päivitetään tiedot henkilölistaan.
            MainWindow.PersonList[MainWindow.PersonAt].Name = NameTextbox.Text;
            MainWindow.PersonList[MainWindow.PersonAt].Address = AddressTextbox.Text;
            MainWindow.PersonList[MainWindow.PersonAt].PhoneNumber = PhoneNumberTextbox.Text;
            MainWindow.PersonList[MainWindow.PersonAt].Email = EmailTextbox.Text;
            
            // Henkilölista aakkosjärjestykseen nimen mukaan.
            MainWindow.PersonList = MainWindow.PersonList.OrderBy(x => x.Name).ToList();

            // Päivitetään MainWindow:n tekstikentät ja listview.
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(MainWindow))
                {
                    //(window as MainWindow).myListview.Items.Refresh();
                    (window as MainWindow).myListview.ItemsSource = MainWindow.PersonList;
                    (window as MainWindow).SearchTextbox.Text = "";
                    (window as MainWindow).NameLabel.Content = NameTextbox.Text;
                    (window as MainWindow).AddressLabel.Content = AddressTextbox.Text;
                    (window as MainWindow).PhoneNumerLabel.Content = PhoneNumberTextbox.Text;
                    (window as MainWindow).EmailLabel.Content = EmailTextbox.Text;
                }
            }

            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
