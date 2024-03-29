﻿using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Wpf_Restaurant
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Btn_startApp_Click(object sender, RoutedEventArgs e)
        {
            var waiterWin = new Waiter();
            waiterWin.Owner = this;
            waiterWin.Show();

            var kitchenWin = new Kitchen();
            kitchenWin.Owner = this;
            kitchenWin.Show();

        }
    }
}
