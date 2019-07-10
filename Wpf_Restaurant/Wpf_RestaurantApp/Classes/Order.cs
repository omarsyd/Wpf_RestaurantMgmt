using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf_Restaurant
{
    public class Order
    {
        public int id { get; set; }

        public RestaurantTable table { get; set; }

        public List<OrderItems> orderItems { get; set; }

        public DateTime orderedDate { get; set; }

        public int waiterId { get; set; }
    }

    public class OrderItems
    {
        public string itemName { get; set; }

        public string customization { get; set; }//special requests by the customer

        public int quantity { get; set; }

        public double price { get; set; }

    }


}
