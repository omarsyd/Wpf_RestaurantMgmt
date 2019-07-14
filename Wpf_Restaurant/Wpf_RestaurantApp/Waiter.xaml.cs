using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Interaction logic for Waiter.xaml
    /// </summary>
    public partial class Waiter : Window
    {
        public static ObservableCollection<MenuCard> _menuCard;
        public static ObservableCollection<MenuCard> _menu;
        public static ObservableCollection<Order> _orders;
        public static ObservableCollection<RestaurantTable> _tables;
        public static ObservableCollection<Order> orderLst;
        public static Order _orderList;

        public Waiter()
        {
            InitializeComponent();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Lbx_items.Visibility = Visibility.Collapsed;
            var tables = MyStorage.ReadXml<ObservableCollection<RestaurantTable>>("tables.xml");
            string tableCategory = Cbx_tableCategory.SelectedItem.ToString();
            bool isIndoor = false;
            if (tableCategory == "Indoor")
            {
                isIndoor = true;
            }
            var selectedCategoryTables = (from t in tables where (t.isIndoor == isIndoor) select t).ToList();
            _tables = new ObservableCollection<RestaurantTable>(selectedCategoryTables);
            _tables.Insert(0, new RestaurantTable { tableNo = "Choose a table" });
            Cbx_tables.ItemsSource = _tables;
            Cbx_tables.SelectedItem = _tables[0];
            Cbx_itemCategory.ItemsSource = Enum.GetValues(typeof(Category));
            Cbx_itemCategory.SelectedItem = Category.Beverages;

            _menu = MyStorage.ReadXml<ObservableCollection<MenuCard>>("menuCard.xml");
            if (_menu != null)
            {
                var result = (from s in _menu where s.category.ToString() == Cbx_itemCategory.SelectedItem.ToString() select s).ToList();
                ObservableCollection<MenuCard> myCollection = new ObservableCollection<MenuCard>(result);

                _menuCard = myCollection;
                _menuCard.Insert(0, new MenuCard { itemName = "Choose an Item" });
                Cbx_items.SelectedItem = _menuCard[0];
                Cbx_items.ItemsSource = _menuCard;


            }


        }

        private void Cbx_itemCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string itemCategory = Cbx_itemCategory.SelectedItem.ToString();

            if (_menu != null)
            {
                var result = (from s in _menu where s.category.ToString() == itemCategory select s).ToList();
                ObservableCollection<MenuCard> myCollection = new ObservableCollection<MenuCard>(result);
                _menuCard = myCollection;
                _menuCard.Insert(0, new MenuCard { itemName = "Choose an Item" });
                Cbx_items.ItemsSource = _menuCard;
                Cbx_items.SelectedItem = _menuCard[0];

            }


        }

        private void Txb_searchItem_KeyUp(object sender, KeyEventArgs e)
        {
            string itemCategory = Cbx_itemCategory.SelectedItem.ToString();
            string query = (sender as TextBox).Text.ToLower();
            if (string.IsNullOrEmpty(query))
            {
                Lbx_items.Visibility = Visibility.Collapsed;
            }
            else
            {
                if (_menu != null)
                {
                    var result = (from s in _menu where (s.itemName.ToLower().Contains(query) && s.category.ToString() == itemCategory) select s).ToList();
                    ObservableCollection<MenuCard> myCollection = new ObservableCollection<MenuCard>(result);
                    _menuCard = myCollection;
                    Lbx_items.Visibility = Visibility.Visible;
                    Lbx_items.ItemsSource = _menuCard;

                }

            }
            // Add the result   
        }

        private void Lbx_items_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MenuCard item = Lbx_items.SelectedItem as MenuCard;
            if (item != null)
            {
                Random rnd = new Random();
                Tbx_searchItem.Text = item.itemName;
                var res = (from p in _menuCard where p.itemName == item.itemName select p).FirstOrDefault();
                orderLst = MyStorage.ReadXml<ObservableCollection<Order>>("orders.xml");
                int itemId = rnd.Next(0, 99);
                if (orderLst.Count == 0)
                {
                    if (_orderList == null)
                    {
                        int id = rnd.Next(0, 999);
                        _orderList = new Order { id = id, orderedDate = DateTime.Now, table = new RestaurantTable { isIndoor = false, isBooked = true, tableNo = "3" }, waiterId = 3, orderItems = new List<OrderItems> { new OrderItems { itemName = res.itemName, allergens = res.allergens, price = res.price, totalQuantityPrice = res.price, quantity = 1, itemId = itemId } } };
                    }
                    else
                    {
                        _orderList.orderItems.Add(new OrderItems { itemName = res.itemName, allergens = res.allergens, price = res.price, totalQuantityPrice = res.price, quantity = 1, itemId = itemId });
                    }

                    var updatedOrdr = _orderList.orderItems;
                    Lbx_order.ItemsSource = null;
                    Lbx_order.ItemsSource = updatedOrdr;
                }
                else
                {
                    ObservableCollection<Order> orderList = new ObservableCollection<Order>(orderLst);
                    _orderList = (from o in orderList where o.table.tableNo == Cbx_tables.SelectedItem.ToString() select o).FirstOrDefault();
                    if (_orderList != null)
                    {
                        _orderList.orderItems.Add(new OrderItems { itemName = res.itemName, allergens = res.allergens, price = res.price, totalQuantityPrice = res.price, quantity = 1, itemId = itemId });
                        var updatedOrdr = _orderList.orderItems;

                    }
                    else
                    {
                        int id = rnd.Next(0, 999);
                        _orderList = new Order { id = id, orderedDate = DateTime.Now, table = new RestaurantTable { isIndoor = false, isBooked = true, tableNo = "3" }, waiterId = 3, orderItems = new List<OrderItems> { new OrderItems { itemName = res.itemName, allergens = res.allergens, price = res.price, totalQuantityPrice = res.price, quantity = 1, itemId = itemId } } };
                    }
                    Lbx_order.Visibility = Visibility.Visible;
                    Lbx_order.ItemsSource = null;
                    Lbx_order.ItemsSource = _orderList.orderItems;
                }
            }
            this.DataContext = _orderList;
        }

        private void Cbx_items_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                MenuCard item = Cbx_items.SelectedItem as MenuCard;
                if (item != null && item.itemName != "Choose an Item")
                {
                    bool hasAlcohol = false;
                    if (Ckbx_hasAlcohol.IsChecked == true)
                    {
                        hasAlcohol = true;
                    }
                    bool isVegetarian = false;
                    if (Ckbk_vegetarian.IsChecked == true)
                    {
                        isVegetarian = true;
                    }
                    Random rnd = new Random();
                    Cbx_items.SelectedItem = item.itemName;
                    var res = (from p in _menuCard where (p.itemName == item.itemName && p.hasAlcohol == hasAlcohol && p.isVeg == isVegetarian) select p).FirstOrDefault();
                    orderLst = MyStorage.ReadXml<ObservableCollection<Order>>("orders.xml");
                    int itemId = rnd.Next(0, 99);
                    if (orderLst.Count == 0)
                    {
                        int id = rnd.Next(0, 999);
                        _orderList = new Order { id = id, orderedDate = DateTime.Now, table = new RestaurantTable { isIndoor = false, isBooked = true, tableNo = "3" }, waiterId = 3, orderItems = new List<OrderItems> { new OrderItems { itemName = res.itemName, allergens = res.allergens, price = res.price, totalQuantityPrice = res.price, quantity = 1, itemId = itemId } } };
                        Lbx_order.ItemsSource = null;
                        Lbx_order.ItemsSource = _orderList.orderItems;
                    }
                    else
                    {
                        ObservableCollection<Order> orderList = new ObservableCollection<Order>(orderLst);
                        var list = (from o in orderList where o.table.tableNo == Cbx_tables.SelectedItem.ToString() select o).FirstOrDefault();
                        if (list != null)
                        {
                            list.orderItems.Add(new OrderItems { itemName = res.itemName, allergens = res.allergens, price = res.price, totalQuantityPrice = res.price, quantity = 1, itemId = itemId });
                            _orderList = list;
                        }
                        else
                        {
                            int id = rnd.Next(0, 999);
                            _orderList = new Order { id = id, orderedDate = DateTime.Now, table = new RestaurantTable { isIndoor = false, isBooked = true, tableNo = "3" }, waiterId = 3, orderItems = new List<OrderItems> { new OrderItems { itemName = res.itemName, allergens = res.allergens, price = res.price, totalQuantityPrice = res.price, quantity = 1, itemId = itemId } } };

                        }

                        Lbx_order.Visibility = Visibility.Visible;
                        Lbx_order.ItemsSource = null;
                        Lbx_order.ItemsSource = _orderList.orderItems;
                    }
                }
                this.DataContext = _orderList;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.InnerException);
            }
        }

        private void Btn_increaseQuantity_Click(object sender, RoutedEventArgs e)
        {
            var itemId = (sender as Button).Tag.ToString();
            foreach (var orderItem in _orderList.orderItems)
            {
                if (orderItem.itemId.ToString() == itemId)
                {
                    double priceVal;

                    //if (double.TryParse(orderItem.price, System.Globalization.NumberStyles.Float, germanCulture, out priceVal))
                    //{
                    //    double valInGermanFormat = priceVal;

                    //}

                    orderItem.quantity = orderItem.quantity + 1;
                    double totalItemPrice = (orderItem.price * orderItem.quantity);
                    orderItem.totalQuantityPrice = totalItemPrice;
                    break;
                }

            }
            double total = _orderList.orderItems.Sum(item => item.totalQuantityPrice);
            _orderList.billAmount = total.ToString();
            Lbx_order.ItemsSource = null;
            Lbx_order.ItemsSource = _orderList.orderItems;
            this.DataContext = _orderList;
        }

        private void Btn_decreaseQuantity_Click(object sender, RoutedEventArgs e)
        {
            var itemId = (sender as Button).Tag.ToString();
            foreach (var orderItem in _orderList.orderItems)
            {
                if (orderItem.itemId.ToString() == itemId)
                {
                    double priceVal;

                    //if (double.TryParse(orderItem.price, System.Globalization.NumberStyles.Float, germanCulture, out priceVal))
                    //{
                    //    double valInGermanFormat = priceVal;

                    //}

                    orderItem.quantity = orderItem.quantity - 1;
                    double totalItemPrice = (orderItem.price * orderItem.quantity);
                    orderItem.totalQuantityPrice = totalItemPrice;
                    break;
                }

            }
            Lbx_order.ItemsSource = null;
            Lbx_order.ItemsSource = _orderList.orderItems;
            this.DataContext = _orderList;
        }

        private void Btn_deleteItem_Click(object sender, RoutedEventArgs e)
        {
            if (Lbx_order.SelectedItem == null)
            {
                MessageBox.Show("Select an item first to delete", "Caution");
                return;
            }
            else
            {
                foreach (var item in _orderList.orderItems)
                {
                    var dishItem = Lbx_order.SelectedItem as OrderItems;
                    if (item.itemName.Equals(dishItem.itemName))
                    {
                        _orderList.orderItems.Remove(item);
                        break;
                    }
                }
                Lbx_order.ItemsSource = null;
                Lbx_order.ItemsSource = _orderList.orderItems;
            }

        }

        private void Btn_customization_Click(object sender, RoutedEventArgs e)
        {
            string itemId = (sender as Button).Tag.ToString();
            int id = int.Parse(itemId);
            Window win = new Customization(id);
            win.Owner = this;
            win.Show();
        }

        private void Btn_cancelOrder_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_Billing_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_placeOrder_Click(object sender, RoutedEventArgs e)
        {
            _orders = MyStorage.ReadXml<ObservableCollection<Order>>("orders.xml");
            string orderButtonName = (sender as Button).Content.ToString();
            string orderTagName = (sender as Button).Tag.ToString();
            int orderId = int.Parse(orderTagName);
            if (orderButtonName.Contains("Place"))
            {
                _orderList.table.isBooked = true;
                _orders.Add(_orderList);
                Btn_placeOrder.Content = "Update Order";
            }
            else
            {
                foreach (var order in _orders)
                {
                    if (order.id == orderId)
                    {
                        _orders.Remove(order);
                        _orders.Add(_orderList);
                        break;
                    }
                }

            }
            MyStorage.WriteXml<ObservableCollection<Order>>(_orders, "orders.xml");
            MessageBox.Show("Order Placed Successfully!", "Success", MessageBoxButton.OK);
        }

        private void Cbx_tableCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var tables = MyStorage.ReadXml<ObservableCollection<RestaurantTable>>("tables.xml");
            string tableCategory = Cbx_tableCategory.SelectedItem.ToString();
            bool isIndoor = false;
            if (tableCategory.Contains("Indoor"))
            {
                isIndoor = true;
            }
            var selectedCategoryTables = (from t in tables where t.isIndoor == isIndoor select t).ToList();
            _tables = new ObservableCollection<RestaurantTable>(selectedCategoryTables);
            _tables.Insert(0, new RestaurantTable { tableNo = "Choose a table" });
            Cbx_tables.ItemsSource = _tables;
            Cbx_tables.SelectedItem = _tables[0];
        }

        private void Cbx_tables_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Cbx_tables.SelectedItem != null)
            {
                var tables = (Cbx_tables.SelectedItem as RestaurantTable).tableNo;
                if (tables.Contains("Choose"))
                {
                    return;
                }
                else
                {
                    var list = MyStorage.ReadXml<ObservableCollection<Order>>("orders.xml");
                    var res = (from o in list where o.table.tableNo == tables select o).FirstOrDefault();
                    if(res != null)
                    Lbx_order.ItemsSource = res.orderItems;
                }
            }
        }
    }


}
