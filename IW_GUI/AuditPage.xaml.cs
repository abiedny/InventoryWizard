using System;
using System.Collections.Generic;
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
using Microsoft.WindowsAPICodePack.Dialogs;

namespace IW_GUI
{
    /// <summary>
    /// Err this is the audit page I guess
    /// </summary>
    public partial class AuditPage : Page
    {
        public string _hallCode;
        private List<InventoryItem> _inventory = Inventory.GetInventory();
        private List<InventoryItem> scannedItems;
        private List<InventoryItem> expectedItems;
        private List<InventoryItem> modifiedItems;
        private List<InventoryItem> possibleMatches;

        public AuditPage(string hallCode)
        {
            InitializeComponent();
            _hallCode = hallCode;
            Title.Text = "Audit for hall " + hallCode;

            scannedItems = new List<InventoryItem>();
            expectedItems = new List<InventoryItem>();
            modifiedItems = new List<InventoryItem>();
            possibleMatches = new List<InventoryItem>();

            ServiceTagInputBox.KeyUp += ServiceTagInputBox_KeyUp;

            ScannedList.ItemsSource = scannedItems;
            ExpectedList.ItemsSource = expectedItems;
            AutoList.ItemsSource = possibleMatches;

            //Stuff that sorts out the expected items using hallcode
            foreach (InventoryItem item in _inventory)
            {
                if (item.room.Contains(hallCode + " "))
                {
                    expectedItems.Add(item);
                }
            }
        }

        private void ServiceTagInputBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Back) && lastText != null) {
                var temp = lastText;
                lastText = null;
                ServiceTagInputBox.Text = temp;
                ServiceTagInputBox.CaretIndex = ServiceTagInputBox.Text.Length;
            }
        }

        private void SubmitTag_ButtonClick(object sender, RoutedEventArgs e)
        {
            lastText = null;
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
                var boxResult = MessageBox.Show("Does all this info look good?\nType: " + item.type + "\nService Tag: " + item.serviceTag + "\nMake: " + item.make + "\nModel: " + item.model + "\nStaff: " + item.staff + " \nRoom: " + item.room, "Confirm the info:", MessageBoxButton.YesNo);
                if (boxResult == MessageBoxResult.No || item.type == "" || item.serviceTag == "" || item.make == "" || item.model == "" || item.room == "") //it's OK if staff is blank
                {
                    //Run the add/correct item function or something
                    //and then pop it on the "to add/modify" list
                    CreateItemBox dlg = new CreateItemBox(item.type, item.serviceTag, item.make, item.model, item.staff, item.room);
                    if (dlg.ShowDialog() == true)
                    {
                        try
                        {
                            expectedItems.Remove(item);
                        }
                        catch
                        {
                            //lol @best practice
                        }
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
            ServiceTagInputBox.Focus(); //Set focus back on input box after submitting an item
        }

        private void EndAudit_ButtonClick(object sender, RoutedEventArgs e)
        {
            List<string> _scannedInventory = new List<string>();
            List<string> _remainingInventory = new List<string>();
            List<string> _modifiedInventory = new List<string>();

            try
            {
                foreach (InventoryItem item in scannedItems)
                {
                    _scannedInventory.Add(item.type + "    " + item.serviceTag + "    " + item.make + "    " + item.model + "    " + item.staff + "   " + item.room);
                }
                foreach (InventoryItem item in expectedItems)
                {
                    _remainingInventory.Add(item.type + "    " + item.serviceTag + "    " + item.make + "    " + item.model + "    " + item.staff + "   " + item.room);
                }
                foreach (InventoryItem item in modifiedItems)
                {
                    _modifiedInventory.Add(item.type + "    " + item.serviceTag + "    " + item.make + "    " + item.model + "    " + item.staff + "   " + item.room);
                }
            }
            catch
            {
                //they tried to end the audit withour scanning anything
            }

            CommonOpenFileDialog dlg = new CommonOpenFileDialog();
            dlg.Title = "Select Export Location";
            dlg.IsFolderPicker = true;
            dlg.AddToMostRecentlyUsedList = false;
            dlg.AllowNonFileSystemItems = false;
            dlg.EnsureFileExists = true;
            dlg.EnsurePathExists = true;
            dlg.EnsureReadOnly = false;
            dlg.EnsureValidNames = true;
            dlg.Multiselect = false;
            dlg.ShowPlacesList = true;

            if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
            {
                File.WriteAllLines(dlg.FileName + "\\" + _hallCode + "_scannedInventory.txt", _scannedInventory);
                File.WriteAllLines(dlg.FileName + "\\" + _hallCode + "_remainingInventory.txt", _remainingInventory);
                File.WriteAllLines(dlg.FileName + "\\" + _hallCode + "_modifiedInventory.txt", _modifiedInventory);
            }

            NavigationService.GoBack();
        }

        private void ExportScanned_ButtonClick(object sender, RoutedEventArgs e)
        {
            List<string> _scannedInventory = new List<string>();
            foreach (InventoryItem item in scannedItems)
            {
                _scannedInventory.Add(item.type + "    " + item.serviceTag + "    " + item.make + "    " + item.model + "    " + item.staff + "   " + item.room);
            }

            CommonOpenFileDialog dlg = new CommonOpenFileDialog();
            dlg.Title = "Select Export Location";
            dlg.IsFolderPicker = true;

            dlg.AddToMostRecentlyUsedList = false;
            dlg.AllowNonFileSystemItems = false;
            dlg.EnsureFileExists = true;
            dlg.EnsurePathExists = true;
            dlg.EnsureReadOnly = false;
            dlg.EnsureValidNames = true;
            dlg.Multiselect = false;
            dlg.ShowPlacesList = true;
            if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
            {
                File.WriteAllLines(dlg.FileName + "\\" + _hallCode + "_scannedInventory.txt", _scannedInventory);
            }
            return;
        }

        private void ExportRemain_ButtonClick(object sender, RoutedEventArgs e)
        {
            List<string> _remainingInventory = new List<string>();
            foreach (InventoryItem item in expectedItems)
            {
                _remainingInventory.Add(item.type + "    " + item.serviceTag + "    " + item.make + "    " + item.model + "    " + item.staff + "   " + item.room);
            }

            var dlg = new CommonOpenFileDialog();
            dlg.Title = "Select Export Location";
            dlg.IsFolderPicker = true;

            dlg.AddToMostRecentlyUsedList = false;
            dlg.AllowNonFileSystemItems = false;
            dlg.EnsureFileExists = true;
            dlg.EnsurePathExists = true;
            dlg.EnsureReadOnly = false;
            dlg.EnsureValidNames = true;
            dlg.Multiselect = false;
            dlg.ShowPlacesList = true;
            if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
            {
                File.WriteAllLines(dlg.FileName + "\\" + _hallCode + "_remainingInventory.txt", _remainingInventory);
            }
            return;
        }

        //Inefficient as HECK
        private string lastText = null;
        private void ServiceTagInputBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (lastText != null)
            {
                return;
            }

            possibleMatches.Clear();

            string currentInput = ServiceTagInputBox.Text.ToUpper();

            //search the entire inventory for matches screw efficient algorithms
            foreach (InventoryItem item in _inventory)
            {
                if (FindItem(item))
                {
                    possibleMatches.Add(item);
                }
            }

            if (possibleMatches.Count == 1)
            {
                lastText = currentInput.Remove(currentInput.Length - 1, 1); //remove last character that triggered this block
                string fullTag = possibleMatches[0].serviceTag.ToUpper();
                ServiceTagInputBox.Text = fullTag;
                ServiceTagInputBox.CaretIndex = ServiceTagInputBox.Text.Length;
            }
            else
            {
                lastText = null;
            }

            AutoList.Items.Refresh();
        }
        private bool FindItem(InventoryItem item)
        {
            if (item.serviceTag.Contains(ServiceTagInputBox.Text.ToUpper()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void AutoList_Selected(object sender, RoutedEventArgs e)
        {
            try
            {
                ServiceTagInputBox.Text = (AutoList.SelectedItem as InventoryItem).serviceTag;
            }
            catch
            {
                //Idk...
            }
        }
    }
}
