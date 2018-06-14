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
    /// Literally just imported the old code rn. Gotta go thru it
    /// </summary>
    public partial class AuditPage : Page
    {
        public string _hallCode;
        private List<InventoryItem> _inventory = Inventory.GetInventory();
        private List<InventoryItem> scannedItems;
        private List<InventoryItem> expectedItems;
        private List<InventoryItem> modifiedItems;

        public AuditPage(string hallCode)
        {
            InitializeComponent();

            Title.Text = "Audit for hall " + hallCode;

            scannedItems = new List<InventoryItem>();
            expectedItems = new List<InventoryItem>();
            modifiedItems = new List<InventoryItem>();

            ScannedList.ItemsSource = scannedItems;
            ExpectedList.ItemsSource = expectedItems;

            //Stuff that sorts out the expected items using hallcode
            foreach (InventoryItem item in _inventory)
            {
                if (item.room.Contains(hallCode + " "))
                {
                    expectedItems.Add(item);
                }
            }
        }

        private void SubmitTag_ButtonClick(object sender, RoutedEventArgs e)
        {
            if (ServiceTagInputBox.Text == "")
            {
                return;
            }
            var item = Inventory.CheckServiceTag(ServiceTagInputBox.Text.ToUpper());
            if (item == null)
            {
                //this means it don't exist in all of hits
                //run the create it function, and pop on the add/modify list
                CreateItemBox dlg = new CreateItemBox(ServiceTagInputBox.Text.ToUpper());
                if (dlg.ShowDialog() == true)
                {
                    item = dlg.NewItem;
                    modifiedItems.Add(item);
                }
            }
            else
            {
                //it does exist in hits. First confirm correct info
                var boxResult = MessageBox.Show("Does all this info look good?\n" + item.type + " " + item.serviceTag + " " + item.make + " " + item.model + " " + item.staff + " " + item.room, "Confirm the info:", MessageBoxButton.YesNo);
                if (boxResult == MessageBoxResult.No)
                {
                    //Run the add/correct item function or something
                    //and then pop it on the "to add/modify" list
                    CreateItemBox dlg = new CreateItemBox(ServiceTagInputBox.Text.ToUpper());
                    if (dlg.ShowDialog() == true)
                    {
                        item = dlg.NewItem;
                        modifiedItems.Add(item);
                    }
                }

                //then add it to the scanned list with possibly updated data try catch is in case item is not in expected list
                try
                {
                    expectedItems.Remove(item);
                }
                catch
                {
                    //lol @best practice
                }
            }

            scannedItems.Add(item);
            ScannedList.Items.Refresh();
            ExpectedList.Items.Refresh();
            ServiceTagInputBox.Clear();
        }
        private void EndAudit_ButtonClick(object sender, RoutedEventArgs e)
        {
            //Do the ending audit cleanup
            //Navigate back to home
        }

        private void ExportScanned_ButtonClick(object sender, RoutedEventArgs e)
        {
            //Make sure to export all the data on scanned items, not just service tag
        }

        private void ExportRemain_ButtonClick(object sender, RoutedEventArgs e)
        {
            //yep export all the deets same deal here
        }
    }
}
