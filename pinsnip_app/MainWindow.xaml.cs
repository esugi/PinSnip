using System;
using System.Collections.Generic;
using System.IO;
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
using System.Drawing;

namespace pinsnip_app
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CroppedBitmap snipBmpSource;
        Views.SnipWindow snipWindow;

        public MainWindow()
        {
            InitializeComponent();
            //createBMP();
            
        }

        private void createBMP()
        {
            System.Windows.Controls.Image myImage = new System.Windows.Controls.Image();
            myImage.Width = 200;
            BitmapImage myBI = new BitmapImage();
            myBI.BeginInit();
            myBI.UriSource = new Uri("/Assets/pinsnip_icon.png", UriKind.Relative);
            myBI.DecodePixelWidth = 200;
            myBI.EndInit();
            myImage.Source = myBI;
            snipImgDisplay.Source = myBI;
        }
        private void BorderMouse_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Testing...Click");
        }

        private void WindowClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void WindowMax_Click(object sender, RoutedEventArgs e)
        {
            if(WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
                WindowMax_Bttn.Content = this.Resources["WindowMaxIcon1"];
            } else
            {
                WindowState = WindowState.Maximized;
                WindowMax_Bttn.Content = this.Resources["WindowMaxIcon2"]; ;
            }
        }
        private void WindowMin_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        private void WindowDrag_Click(object sender, MouseEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("Window Size: {0} , {1}", this.Height, this.Width);
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (WindowState == WindowState.Maximized)
                {
                    WindowState = WindowState.Normal;

                    double pct = PointToScreen(e.GetPosition(this)).X / SystemParameters.VirtualScreenWidth;
                    Top = 0;
                    Left = e.GetPosition(this).X - (pct * Width);
                }

                DragMove();
            }
        }
        private void WindowResize_Click(object sender, MouseEventArgs e)
        {
            if(WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
            } else
            {
                WindowState = WindowState.Maximized;
            }
        }

        private void NewSnip_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            snipWindow = new Views.SnipWindow(this);
            Application.Current.MainWindow = snipWindow;  
        }

        private void CopySnip_Click(object sender, RoutedEventArgs e)
        {
            if(snipBmpSource == null)
            {

            }
            else
            {
                Clipboard.SetImage(snipBmpSource);
            }
        }

        public void exitSnipWindow(CroppedBitmap cb)
        {
            Application.Current.MainWindow = this;
            snipWindow.Close();
            snipImgDisplay.Source = cb;
            snipBmpSource = cb;
            snipImgDisplay.Stretch = Stretch.Uniform;

            this.Show();
        }
    }
}
