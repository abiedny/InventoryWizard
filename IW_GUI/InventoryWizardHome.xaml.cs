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

namespace IW_GUI
{
    /// <summary>
    /// Interaction logic for InventoryWizardHome.xaml
    /// </summary>
    public partial class InventoryWizardHome : Page
    {
        public InventoryWizardHome()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string currentHall = "";
            var hallCode = Convert.ToInt32((sender as Button).Tag);
            switch (hallCode)
            {
                case 1:
                    currentHall = "17";
                    break;
                case 2:
                    currentHall = "BA";
                    break;
                case 3:
                    currentHall = "CE";
                    break;
                case 4:
                    currentHall = "CO";
                    break;
                case 5:
                    currentHall = "FR";
                    break;
                case 6:
                    currentHall = "KE";
                    break;
                case 7:
                    currentHall = "MI";
                    break;
                case 8:
                    currentHall = "PI";
                    break;
                case 9:
                    currentHall = "RA";
                    break;
                case 10:
                    currentHall = "SA";
                    break;
                case 11:
                    currentHall = "TE";
                    break;
                case 12:
                    currentHall = "WI";
                    break;
                case 13:
                    currentHall = "UV";
                    break;
                case 14:
                    currentHall = "YU";
                    break;
            }

            try
            {
                AuditPage _auditPage = new AuditPage(currentHall);
                this.NavigationService.Navigate(_auditPage);
            }
            catch
            {
                MessageBox.Show("You forgot to import the inventory csv you silly goose!", "Error", MessageBoxButton.OK);
            }
        }
        private void Import_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".csv";
            dlg.Filter = "Comma Seperated Value Files (*.csv)|*.csv";

            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                //This is the path to the csv. Use this to do stuff I guess?
                string filename = dlg.FileName;
                //Then run import function and put everything into a the masterlist
                try
                {
                    Inventory.ImportInventory(filename);
                    Properties.Settings.Default.CSVpath = filename;
                }
                catch
                {
                    //if an exception is caught, reset the filepath value in settings
                    Properties.Settings.Default.CSVpath = "\\HITS.csv";
                    MessageBox.Show("Ummmm that's an invalid CSV ya got there buckaroo. Remember that we only take the weird format that HITS exports too.", "Error", MessageBoxButton.OK);
                }
                Properties.Settings.Default.Save();
            }
        }
    }
}
