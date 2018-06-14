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
using System.Windows.Shapes;

namespace IW_GUI
{
    /// <summary>
    /// Interaction logic for CreateItemBox.xaml
    /// </summary>
    public partial class CreateItemBox : Window
    {
        public CreateItemBox(string scannedTag)
        {
            InitializeComponent();

            this.FontFamily = new FontFamily("Comic Sans MS");

            typeAnswer.Clear();
            serviceTagAnswer.Clear();
            makeAnswer.Clear();
            modelAnswer.Clear();
            staffAnswer.Clear();
            roomAnswer.Clear();

            serviceTagAnswer.Text = scannedTag;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            typeAnswer.SelectAll();
            typeAnswer.Focus();
        }

        private void OK_OnClick(object sender, RoutedEventArgs e)
        {
            NewItem = new InventoryItem
            {
                type = typeAnswer.Text,
                serviceTag = serviceTagAnswer.Text,
                make = makeAnswer.Text,
                model = modelAnswer.Text,
                staff = staffAnswer.Text,
                room = roomAnswer.Text
            };

            this.DialogResult = true;
            this.Close();
            return;
        }

        public InventoryItem NewItem { get; set; }
    }
}
