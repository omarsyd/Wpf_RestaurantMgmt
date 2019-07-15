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
using System.Windows.Threading;

namespace Wpf_Restaurant
{
    /// <summary>
    /// Interaction logic for Kitchen.xaml
    /// </summary>
    public partial class Kitchen : Window
    {
        public static ObservableCollection<Order> _orders;
        public static ObservableCollection<Order> _inQueueOrders;
        public static ObservableCollection<Order> _inProgressOrders;
        public static ObservableCollection<Order> _completedOrders;
        DispatcherTimer timer = new DispatcherTimer();
        bool isVisible;
        bool chefChanged;
        public Kitchen()
        {
            InitializeComponent();
            timer.Interval = TimeSpan.FromSeconds(2);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (isVisible)
                Tblk_restaurantName.Visibility = Visibility.Visible;
            else
                Tblk_restaurantName.Visibility = Visibility.Hidden;

            isVisible = !isVisible;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var allOrders = App._orders;
            var inQueue = from p in allOrders where (p.appetizerStatus == Status.InQueue) && (p.appetizerStatus == Status.InQueue) select p;
            var inProgress1 = from p in allOrders where (p.appetizerStatus == Status.InProgress) || (p.mainCourseStatus == Status.InProgress) select p;
            var inProgress2 = from p in allOrders where (p.appetizerStatus == Status.InProgress) || (p.mainCourseStatus == Status.InProgress) select p;
            List<Order> ordersList = new List<Order>();
            Order eachOrder = new Order();
            List<OrderItems> orderItems = new List<OrderItems>();
            foreach (var order in inQueue)
            {
                eachOrder = null;
                eachOrder = order;
                foreach (var orderItem in order.orderItems)
                {
                    if (orderItem.category == "Appetizers" || orderItem.category == "MainCourse")
                    {
                        orderItems.Add(orderItem);
                    }

                }
                eachOrder.orderItems = null;
                if (orderItems.Count != 0)
                {
                    eachOrder.orderItems = orderItems;
                    ordersList.Add(eachOrder);
                }



            }
            _inQueueOrders = new ObservableCollection<Order>(ordersList);
            Lbx_inQueue.ItemsSource = _inQueueOrders;

            //#region inProgress Not Working
            //List<Order> orderList2 = new List<Order>();

            //Order eachOrder1 = new Order();
            //Order eachOrder2 = new Order();
            //List<OrderItems> orderItems1 = new List<OrderItems>();
            //List<OrderItems> orderItems2 = new List<OrderItems>();
            //foreach(var order in inProgress1)
            //{
            //    foreach(var orderItem in order.orderItems)
            //    {
            //        if(orderItem.category == "Appetizers")
            //        {
            //            orderItems1.Add(orderItem);
            //        }
            //    }
            //    if (orderItems1.Count != 0)
            //    {
            //        eachOrder1 = order;
            //        eachOrder1.orderItems = null;
            //        eachOrder1.orderItems = orderItems1;
            //        orderList2.Add(eachOrder1);
            //    }

            //}

            //foreach(var order in inProgress2)
            //{
            //    foreach(var orderItem in order.orderItems)
            //    {
            //        if(orderItem.category == "MainCourse")
            //        {
            //            orderItems2.Add(orderItem);
            //        }
            //    }
            //    if(orderItems2.Count !=0)
            //    {
            //        eachOrder2 = order;
            //        eachOrder2.orderItems = null;
            //        eachOrder2.orderItems = orderItems2;
            //        orderList2.Add(eachOrder2);
            //    }
            //}

            //_inProgressOrders = new ObservableCollection<Order>(orderList2);
            //#endregion
            _inProgressOrders = new ObservableCollection<Order>(inProgress1);
            Lbx_inProgress.ItemsSource = _inProgressOrders;
        }

        //private void Cbx_assignChef_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    var chefId = (sender as ComboBox).Text.ToString();
        //    if (chefId != "Choose Chef")
        //    {
        //        var item = (sender as ComboBox).Tag.ToString();
        //        chefChanged = true;
        //        _inProgressOrders = new ObservableCollection<Order>();
        //        int orderId = int.Parse(item);
        //        foreach (var order in _inQueueOrders)
        //        {
        //            if (order.id == orderId)
        //            {
        //order.appetizerStatus = Status.InProgress;
        //order.mainCourseStatus = Status.InProgress;
        //            _inQueueOrders.Remove(order);
        //            _inProgressOrders.Add(order);
        //            Lbx_inProgress.ItemsSource = _inProgressOrders;
        //            break;
        //            }
        //        }


        //    }

        //}




        private void Lbx_order_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //_inProgressOrders = new ObservableCollection<Order>();
            var item = (sender as StackPanel).Tag.ToString();
            int orderId = int.Parse(item);
            foreach (var order in _inQueueOrders)
            {
                if (order.id == orderId)
                {
                    order.appetizerStatus = Status.InProgress;
                    order.mainCourseStatus = Status.InProgress;
                    _inQueueOrders.Remove(order);
                    _inProgressOrders.Add(order);
                    Lbx_inProgress.ItemsSource = _inProgressOrders;
                    break;
                }
            }

        }

        private void Btn_beginOrder_Click(object sender, RoutedEventArgs e)
        {
            if (_completedOrders == null)
            {
                _completedOrders = new ObservableCollection<Order>();
            }
            var btnStatus = (sender as Button).Content.ToString();
            if (btnStatus.Contains("Begin"))
            {
                (sender as Button).Content = "In Progress";
                return;
            }
            if (btnStatus.Contains("Progress"))
            {
                string oId = (sender as Button).Tag.ToString();
                int orderId = int.Parse(oId);
                foreach(var order in _inProgressOrders)
                {
                    if(order.id == orderId)
                    {
                        _inProgressOrders.Remove(order);
                        order.appetizerStatus = Status.Ready;
                        order.mainCourseStatus = Status.Ready;
                        _completedOrders.Add(order);
                        Lbx_completed.ItemsSource = _completedOrders;
                        break;

                    }
                }
            }

        }
    }
}
