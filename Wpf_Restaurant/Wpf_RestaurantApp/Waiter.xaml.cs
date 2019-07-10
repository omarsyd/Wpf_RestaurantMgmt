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
    /// Interaction logic for Waiter.xaml
    /// </summary>
    public partial class Waiter : Window
    {
        public ObservableCollection<MenuCard> _menuCard;
        public Waiter()
        {
            InitializeComponent();
        }

        private void Txb_searchItem_KeyUp(object sender, KeyEventArgs e)
        {
            bool found = false;
            var border = (resultStack.Parent as ScrollViewer).Parent as Border;

            string itemCategory = Cbx_itemCategory.SelectedItem.ToString();
          
            string query = (sender as TextBox).Text.ToLower();
            var data = from s in _menuCard where (s.category.ToString() == itemCategory && s.itemName.ToLower().Contains(query)) select s;
            if (query.Length == 0)
            {
                // Clear   
                resultStack.Children.Clear();
                border.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                border.Visibility = System.Windows.Visibility.Visible;
            }

            // Clear the list   
            resultStack.Children.Clear();

            // Add the result   
            foreach (var obj in data)
            {
                //if (obj.ToLower().StartsWith(query.ToLower()))
                //{
                //    // The word starts with this... Autocomplete must work   
                //    addItem(obj);
                //    found = true;
                //}
            }

            if (!found)
            {
                resultStack.Children.Add(new TextBlock() { Text = "No results found." });
            }
        }
    }
   
}
