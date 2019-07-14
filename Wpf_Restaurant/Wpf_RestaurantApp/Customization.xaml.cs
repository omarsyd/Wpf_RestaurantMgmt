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

namespace Wpf_Restaurant
{
    /// <summary>
    /// Interaction logic for Customization.xaml
    /// </summary>
    public partial class Customization : Window
    {
        public static ObservableCollection<Order> _ordersList;
        public Customization(int itemId)
        {
            InitializeComponent();
        }


        private void Btn_cancelCustomization_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Btn_saveCustomization_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
