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
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace pinsnip_app.Views
{
    /// <summary>
    /// Interaction logic for SnipWindow.xaml
    /// </summary>
    public partial class SnipWindow : Window
    {
        MainWindow mainWindow;
        BitmapImage screenShotBI;
        CroppedBitmap snipCB;
        int virtualScreenLeft = (int)SystemParameters.VirtualScreenLeft;
        int virtualScreenTop = (int)SystemParameters.VirtualScreenTop;
        int virtualScreenWidth = (int)SystemParameters.VirtualScreenWidth;
        int virtualScreenHeight = (int)SystemParameters.VirtualScreenHeight;
        int numScreen; // assumes 1 or 2
        int clickState; // 0 is start, 1 is end
        bool clickFreeze = false;
        System.Windows.Point origPos, currPos;
        double tempPosY, tempPosX;
        Rect snipRect;
        Rect nullRect = new Rect(0,0,0,0);

        private const int VALIDATE_APPROVE = 0;
        private const int VALIDATE_UNDO_POINT = 1;
        private const int VALIDATE_UNDO_BOTH = 2;
        private const int VALIDATE_CANCEL = 3;
        SnipValidationWindow snipValidationWindow;
        int snipAction;
        
        public SnipWindow(MainWindow parentWindow)
        {
            InitializeComponent();
            System.Diagnostics.Debug.WriteLine("========== TESTING SNIPWINDOW ==========");
            mainWindow = parentWindow;
            numScreen = getNumScreen();
            System.Diagnostics.Debug.WriteLine("Number of Screens: {0}", numScreen);
            clickState = 0;

            createScreenShot();
            displayScreenShot();

            maskRect.Rect = new Rect(0, 0, Window.GetWindow(this).Width, Window.GetWindow(this).Height);
            Cursor = Cursors.Cross;
        }

        public void SnipScreen_Click(object sender, MouseButtonEventArgs e)
        {
            // Starting point for snip
            if(clickState == 0)
            {
                origPos = Mouse.GetPosition(Window.GetWindow(this));
                snipRect = new Rect(origPos.X, origPos.Y, 0, 0);
                snipSec.Rect = snipRect;
                clickState = 1;
            }
            // Done getting Snip
            // Validation State with pop up window
            // Ending point for snip
            else if (clickState == 1 && Application.Current.MainWindow == this)
            {
                clickFreeze = true;
                snipValidationWindow = new SnipValidationWindow(this);
                Application.Current.MainWindow = snipValidationWindow;
            }
        }

        public async void exitSnipValidationWindow()
        {
            snipAction = snipValidationWindow.getAction();
            //Close stuff
            Application.Current.MainWindow = this;
            snipValidationWindow.Close();

            switch (snipAction)
            {
                case VALIDATE_APPROVE:
                    // Snip finished & approved
                    pathOutline.StrokeThickness = 0;
                    await Task.Delay(50);
                    clickState = 0;
                    cropScreenShot();
                    mainWindow.exitSnipWindow(snipCB);
                    break;
                case VALIDATE_UNDO_POINT:
                    clickFreeze = false;
                    clickState = 1;
                    break;
                case VALIDATE_UNDO_BOTH:
                    clickState = 0;
                    clickFreeze = false;
                    snipSec.Rect = nullRect;
                    break;
                case VALIDATE_CANCEL:
                default:
                    mainWindow.exitSnipWindow(null);
                    break;
            }

        }

        public void SnipScreen_Move(object sender, MouseEventArgs e)
        {
            
            if (clickState == 1 && !clickFreeze)
            {
                Cursor = Cursors.Cross;
                currPos = Mouse.GetPosition(Window.GetWindow(this));
                tempPosY = currPos.Y - origPos.Y;
                tempPosX = currPos.X - origPos.X;
                if ((tempPosY < 0) || (tempPosY < 0 && tempPosX < 0))
                {
                    tempPosY = -(tempPosY);
                    snipRect.Y = currPos.Y;
                }
                if ((tempPosX < 0) || (tempPosY < 0 && tempPosX < 0))
                {
                    tempPosX = -(tempPosX);
                    snipRect.X = currPos.X;

                }
                snipRect.Width = tempPosX;
                snipRect.Height = tempPosY;
                snipSec.Rect = snipRect;
            }
        }
        private void cropScreenShot()
        {
            double ratioH = virtualScreenHeight / SystemParameters.PrimaryScreenHeight;
            double ratioW = virtualScreenWidth / (SystemParameters.PrimaryScreenWidth * numScreen);
            snipCB = new CroppedBitmap(screenShotBI, new Int32Rect((int)(Math.Round(snipRect.X * ratioW)), (int)(Math.Round(snipRect.Y * ratioH)), (int)(Math.Round(snipRect.Width * ratioW)), (int)(Math.Round(snipRect.Height * ratioH))));
        }

        private void createScreenShot()
        {
            // Weird Resolution Settings
            if(numScreen == 1)
            {
                virtualScreenWidth = (int)(virtualScreenWidth * 1.5);
                virtualScreenHeight = (int)(virtualScreenHeight * 1.5);
            } 
            using (Bitmap bmp = new Bitmap(virtualScreenWidth, virtualScreenHeight))
            {
                // Screen shot to Bitmap bmp
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.CopyFromScreen(virtualScreenLeft, virtualScreenTop, 0, 0, bmp.Size);
                }

                System.Diagnostics.Debug.WriteLine("Bmp Height {0}, Primary Width {1}", bmp.Size.Height, bmp.Size.Width);
                // Convert Bitmap bmp to BitmapImage screenShotBI
                screenShotBI = new BitmapImage();
                using (MemoryStream s = new MemoryStream())
                {
                    bmp.Save(s, ImageFormat.Bmp);
                    s.Seek(0, SeekOrigin.Begin);

                    screenShotBI.BeginInit();
                    screenShotBI.StreamSource = s;
                    screenShotBI.CacheOption = BitmapCacheOption.OnLoad;
                    screenShotBI.EndInit();
                    //screenShotBI.Freeze();
                }
            }
        }

        private void displayScreenShot()
        {
            Window.GetWindow(this).Height = SystemParameters.PrimaryScreenHeight;
            Window.GetWindow(this).Width = SystemParameters.PrimaryScreenWidth * numScreen;

            System.Diagnostics.Debug.WriteLine("Window Height {0}, Window Width {1}", Window.GetWindow(this).Height, Window.GetWindow(this).Width);
            
            screenShotImg.Source = screenShotBI;

            this.Show();
        }

        private int getNumScreen()
        {
            if (virtualScreenWidth > (int)SystemParameters.PrimaryScreenWidth)
                return 2;
            return 1;
        }
    }
}
