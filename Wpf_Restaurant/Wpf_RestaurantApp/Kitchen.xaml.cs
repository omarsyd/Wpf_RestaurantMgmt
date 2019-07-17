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
        DispatcherTimer _timer;
        TimeSpan _time;
        bool isVisible;

        public Kitchen()
        {
            InitializeComponent();
            //timer.Interval = TimeSpan.FromSeconds(2);
            //timer.Tick += Timer_Tick;
            //timer.Start();
            _time = TimeSpan.FromSeconds(10);

            _timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                //TbName.Text = _time.ToString("c");
                if (_time == TimeSpan.Zero) _timer.Stop();
                _time = _time.Add(TimeSpan.FromSeconds(-1));
            }, Application.Current.Dispatcher);

            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (isVisible)
            {
                Lbx_completed.Visibility = Visibility.Visible;
                Lbx_completed.Background = Brushes.Red;
            }
            else
            {
                Lbx_completed.Visibility = Visibility.Hidden;
                Lbx_completed.Background = Brushes.White;

            }

            isVisible = !isVisible;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var allOrders = App._orders;
            var inQueue = from p in allOrders where (p.appetizerStatus == Status.InQueue) && (p.mainCourseStatus == Status.InQueue) select p;
            var inProgress = from p in allOrders where (p.appetizerStatus == Status.InProgress) || (p.mainCourseStatus == Status.InProgress) select p;
            var _completed = from p in allOrders where (p.appetizerStatus == Status.Ready) || (p.mainCourseStatus == Status.Ready) select p;
            List<Order> ordersList = new List<Order>();
            Order eachOrder = new Order();
            List<OrderItems> orderItems = new List<OrderItems>();

            foreach (var order in inQueue)
            {
                var appetizersAndMaincourse = (from p in order.orderItems where (p.category == Category.Appetizers.ToString()) || (p.category == Category.MainCourse.ToString()) select p).ToList();


                if (appetizersAndMaincourse.Count != 0)
                {
                    eachOrder = order;
                    eachOrder.orderItems = appetizersAndMaincourse;
                    ordersList.Add(eachOrder);
                    eachOrder = new Order();
                }

            }
            _inQueueOrders = new ObservableCollection<Order>(ordersList);
            Lbx_inQueue.ItemsSource = _inQueueOrders;



            _inProgressOrders = new ObservableCollection<Order>(inProgress);
            Lbx_inProgress.ItemsSource = _inProgressOrders;

            _completedOrders = new ObservableCollection<Order>(_completed);
            Lbx_completed.ItemsSource = _completedOrders;


        }

        private void Cbx_assignChef_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (Lbx_inQueue.SelectedItem == null)
            {
                return;
            }

            var chefId = (sender as ComboBox).Text.ToString();

            if (chefId != "Choose Chef")
            {
                List<Order> orderList = new List<Order>();

                var selectedOrder = Lbx_inQueue.SelectedItem as Order;

                //#region notworking
                //            Order appetizerOrder =  selectedOrder;
                //            Order mainCourseOrder =  selectedOrder;
                //            var appetizers = (from a in selectedOrder.orderItems where (a.category == Category.Appetizers.ToString()) select a).ToList();
                //            var mainCourse = (from m in selectedOrder.orderItems where m.category == Category.MainCourse.ToString() select m).ToList();

                //                if (appetizers.Count != 0)
                //                {
                //                  appetizerOrder.orderItems = appetizers;
                //                   appetizerOrder.appetizerStatus = Status.InProgress;
                //                   _inProgressOrders.Add(appetizerOrder);

                //                }
                //                if (mainCourse.Count != 0)
                //                {
                //                   mainCourseOrder.orderItems = mainCourse;
                //                    mainCourseOrder.mainCourseStatus = Status.InProgress;
                //                    orderList.Add(mainCourseOrder);
                //                }


                //            #endregion
                string color = "";
                switch(chefId)
                {
                    case "OSyed": color = "#DEFDE0";
                        break;
                    case "GLahare": color = "#F0DEFD";
                        break;
                }
              
                Order o1 = new Order { id = 26, chefName = chefId,chefColor=color, avgCookingTime = 25, categoryTitle = Category.Appetizers, orderItems = new List<OrderItems> { new OrderItems { quantity = 1, itemId = 29, itemName = "Mulligatawny Soup " }, new OrderItems { quantity = 1, itemId = 29, itemName = "Vegetable Samosa" }, new OrderItems { quantity = 1, itemId = 29, itemName = "Mulligatawny Soup " } } };
                Order o2 = new Order { id = 26, chefName = chefId, chefColor = color,avgCookingTime = 15, categoryTitle = Category.MainCourse, orderItems = new List<OrderItems> { new OrderItems { quantity = 1, itemId = 56, itemName = "Fish Curry" } } };
                _inProgressOrders.Add(o1);
                _inProgressOrders.Add(o2);

                _inQueueOrders.Remove(selectedOrder);
                Lbx_inProgress.ItemsSource = null;
                Lbx_inProgress.ItemsSource = _inProgressOrders;



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
                foreach (var order in _inProgressOrders)
                {
                    if (order.id == orderId)
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
