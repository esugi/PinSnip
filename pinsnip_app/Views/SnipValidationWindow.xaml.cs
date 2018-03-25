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
using System.Windows.Shapes;

namespace pinsnip_app.Views
{
    /// <summary>
    /// Interaction logic for SnipValidationWindow.xaml
    /// </summary>
    public partial class SnipValidationWindow : Window
    {
        public int snipAction = 4;
        SnipWindow snipWindow;

        private const int VALIDATE_APPROVE = 0;
        private const int VALIDATE_UNDO_POINT = 1;
        private const int VALIDATE_UNDO_BOTH = 2;
        private const int VALIDATE_CANCEL = 3;

        public SnipValidationWindow(SnipWindow snipWindow)
        {
            InitializeComponent();
            // adjust where to place based on location of position so that it doesnt go off screen
            Window.GetWindow(this).Top = Mouse.GetPosition(Window.GetWindow(snipWindow)).Y - Window.GetWindow(this).Height;
            Window.GetWindow(this).Left = Mouse.GetPosition(Window.GetWindow(snipWindow)).X - Window.GetWindow(this).Width;

            this.snipWindow = snipWindow;
            this.Show();
        }
        private void Approve_Click(object sender, RoutedEventArgs e)
        {
            snipAction = VALIDATE_APPROVE;
            snipWindow.exitSnipValidationWindow();
        }
        private void UndoPoint_Click(object sender, RoutedEventArgs e)
        {
            snipAction = VALIDATE_UNDO_POINT;
            snipWindow.exitSnipValidationWindow();
        }
        private void UndoBoth_Click(object sender, RoutedEventArgs e)
        {
            snipAction = VALIDATE_UNDO_BOTH;
            snipWindow.exitSnipValidationWindow();
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            snipAction = VALIDATE_CANCEL;
            snipWindow.exitSnipValidationWindow();
        }
        public int getAction()
        {
            return snipAction;
        }
    }
}
