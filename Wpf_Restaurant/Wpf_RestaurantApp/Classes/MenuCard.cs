using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf_Restaurant
{
    public class MenuCard
    {
        public int id { get; set; }

        public Category category { get; set; }

        public string itemName { get; set; }

        public string allergens { get; set; }

        public string description { get; set; }

        public double price { get; set; }

        public bool isVegetarian { get; set; }

        public bool hasAlcohol { get; set; }
    }

    public enum Category
    {
        Soups,
        Salads,
        Beverages,
        Appetizers,
        MainCourse,
        Desserts

    }
}
