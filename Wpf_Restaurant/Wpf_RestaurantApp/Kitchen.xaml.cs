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
    /// Interaction logic for Kitchen.xaml
    /// </summary>
    public partial class Kitchen : Window
    {
        public static ObservableCollection<Order> _orders;
        public Kitchen()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //List<Order> orderList = new List<Order>();
            //orderList.Add(new Order { id = 567, orderedDate = DateTime.Now, orderItems = new List<OrderItems> { new OrderItems { itemName = "Chicken Chilly", customization = "Less spicy", quantity = 2 }, new OrderItems { itemName = "Chicken Biriyani", customization = "Less spicy", quantity = 2 } } });

            //Lbx_inQueue.ItemsSource = orderList;
            var result = MyStorage.ReadXml<ObservableCollection<Order>>("orders.xml");
            ObservableCollection<Order> orderList = new ObservableCollection<Order>(result);
            DataContext = orderList;

        }
    }
}
