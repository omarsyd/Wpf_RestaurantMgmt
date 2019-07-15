using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Wpf_Restaurant;

namespace Wpf_Restaurant
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static ObservableCollection<Order> _orders;
        public static ObservableCollection<MenuCard> _menu;
        public static ObservableCollection<RestaurantTable> _tables;
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            _orders = MyStorage.ReadXml<ObservableCollection<Order>>("orders.xml");
            if (_orders == null)
                _orders = new ObservableCollection<Order>();

            _menu = MyStorage.ReadXml<ObservableCollection<MenuCard>>("menuCard.xml");
            if (_menu == null)
                _menu = new ObservableCollection<MenuCard>();

            _tables = MyStorage.ReadXml<ObservableCollection<RestaurantTable>>("tables.xml");
            if (_tables == null)
                _tables = new ObservableCollection<RestaurantTable>();
        }
    }
}
