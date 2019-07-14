using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Wpf_Restaurant
{
    public class MenuCard
    {
        public int id { get; set; }

        public Category category { get; set; }

        public string itemName { get; set; }

        public string allergens { get; set; }

        public string description { get; set; }

        [XmlIgnore]
        public double price { get; set; }

        [XmlElement("price")]
        public string priceFormatted
        {
            get { return price.ToString(System.Globalization.CultureInfo.CurrentCulture); }
            set { price = double.Parse(value, System.Globalization.CultureInfo.CurrentCulture); }
        }

        public bool isVeg { get; set; }

        public bool hasAlcohol { get; set; }

        public string cookingTime { get; set; }
    }

    public enum Category
    {
        Beverages,
        Appetizers,
        MainCourse,
        Desserts

    }
}
