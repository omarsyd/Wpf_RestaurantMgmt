using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Wpf_Restaurant
{
    public class Order
    {
        public int id { get; set; }

        public RestaurantTable table { get; set; }

        public List<OrderItems> orderItems { get; set; }

        public DateTime orderedDate { get; set; }

        public int waiterId { get; set; }

        public string billAmount { get; set; }

        public Status appetizerStatus { get; set; }

        public Status beveragesStatus { get; set; }

        public Status mainCourseStatus { get; set; }

        public Status dessertsStatus { get; set; }

    }

    public class OrderItems
    {
        public int itemId { get; set; }

        public string itemName { get; set; }

        public string allergens { get; set; }

        public string customization { get; set; }//special requests by the customer

        public string cookingTime { get; set; }

        public int quantity { get; set; }

        [XmlIgnore]
        public double price { get; set; }

        [XmlElement("price")]
        public string priceFormatted
        {
            get { return price.ToString(System.Globalization.CultureInfo.CurrentCulture); }
            set { price = double.Parse(value, System.Globalization.CultureInfo.CurrentCulture); }
        }

        [XmlIgnore]
        public double totalQuantityPrice { get; set; }


        [XmlElement("totalQuantityPrice")]
        public string totalQuantityPriceFormatted
        {
            get { return totalQuantityPrice.ToString(System.Globalization.CultureInfo.CurrentCulture); }
            set { totalQuantityPrice = double.Parse(value, System.Globalization.CultureInfo.CurrentCulture); }
        }
    }

    public enum Status
    {
        InQueue = 0,
        InProgress = 1,
        Ready = 2
    }
}
