using System;
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
using Microsoft.Win32;

namespace pinsnip_app.Views
{
    /// <summary>
    /// Interaction logic for SavePage.xaml
    /// </summary>
    public partial class SavePage : Page
    {
        public SavePage()
        {
            InitializeComponent();
            
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            var op = new OpenFileDialog();
            op.ShowDialog();
        }

    }
}
