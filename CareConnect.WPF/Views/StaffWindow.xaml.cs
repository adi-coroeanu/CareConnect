using CareConnect.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CareConnect.WPF.Views
{
    public partial class StaffWindow : Window
    {
        public StaffWindow(StaffViewModel staffViewModel)
        {
            InitializeComponent();

            DataContext = staffViewModel;
        }
    }
}
