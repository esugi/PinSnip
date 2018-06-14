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
using System.Drawing;

namespace pinsnip_app.Views
{
    /// <summary>
    /// Interaction logic for OptionsPage.xaml
    /// </summary>
    public partial class OptionsPage : Page
    {
        MainWindow win = (MainWindow)Application.Current.MainWindow;

        public OptionsPage()
        {
            InitializeComponent();
        }

        private async void NewSnip_Click(object sender, RoutedEventArgs e)
        {
            win.Hide();
            await Task.Delay(400);
            win.snipWindow = new Views.SnipWindow(win);
            Application.Current.MainWindow = win.snipWindow;
        }

        private void CopySnip_Click(object sender, RoutedEventArgs e)
        {
            if (win.snipBmpSource == null)
            {
                win.showNotification("Copy Failed. No Image.");
            }
            else
            {
                Clipboard.SetImage(win.snipBmpSource);
                win.showNotification("Copied Image Successful.");
            }
        }
        
        private void SaveSnip_Click(object sender, RoutedEventArgs e)
        {
            if(win.snipBmpSource == null)
            {
                win.showNotification("No Image Saved.");
            } else {

                var saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "JPEG Image|*.jpg|PNG Image|*.png";
                saveFileDialog1.Title = "Save an Image File";
                saveFileDialog1.ShowDialog();

                // If the file name is not an empty string open it for saving.  
                if (saveFileDialog1.FileName != "")
                {
                    // Saves the Image via a FileStream created by the OpenFile method.  
                    System.IO.FileStream fs =
                       (System.IO.FileStream)saveFileDialog1.OpenFile();
                    var source = win.snipBmpSource;

                    switch (saveFileDialog1.FilterIndex)
                    {
                        case 1:

                            BitmapEncoder encoder = new JpegBitmapEncoder();
                            encoder.Frames.Add(BitmapFrame.Create(source));
                            encoder.Save(fs);
                            break;

                        case 2:
                            BitmapEncoder encoder1 = new PngBitmapEncoder();
                            encoder1.Frames.Add(BitmapFrame.Create(source));
                            encoder1.Save(fs);
                            break;

                    }

                    fs.Close();
                    String message = "Saved " + saveFileDialog1.FileName + "!"; 
                    win.showNotification(message);
                }
            }
            //win.optionsFrame.Navigate(new SavePage());
        }
    }
}
