using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BlackJackSL.Views
{
    /// <summary>
    /// Interaction logic for TableView.xaml
    /// </summary>
    public partial class TableView : UserControl
    {
        public TableView()
        {
            InitializeComponent();          
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void Canvas_Loaded(object sender, RoutedEventArgs e)
        {
                LoginView dialog = new LoginView();
                dialog.Show();
        }
    }
}