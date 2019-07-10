using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf_Restaurant
{
    public class RestaurantTable
    {
        public string tableNo { get; set; }

        public bool isIndoor { get; set; }

        public bool isBooked { get; set; }

        public string clubbedTables { get; set; } //Will be separated with ','.
    }
}
