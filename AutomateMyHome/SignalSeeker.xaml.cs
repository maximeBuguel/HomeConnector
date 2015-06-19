using MahApps.Metro.Controls;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
using System.Windows.Threading;

namespace AutomateMyHome
{
    /// <summary>
    /// Logique d'interaction pour SignalSeeker.xaml
    /// </summary>
    public partial class SignalSeeker : MetroWindow
    {
        SshCommand commande;
        SshClient client;
        private Image b;
        public SignalSeeker(SshClient c)
        {
            InitializeComponent();
            client = c;
            commande = c.CreateCommand("sudo ./HomeConnector/GetNextSignal");
            commande.BeginExecute(new AsyncCallback(cb));
            System.Drawing.Bitmap bmp1 = (System.Drawing.Bitmap)Properties.Resources.ResourceManager.GetObject("signal"); ;
            img.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(bmp1.GetHbitmap(),
                IntPtr.Zero,
                System.Windows.Int32Rect.Empty,
                BitmapSizeOptions.FromWidthAndHeight(100, 100));
            
            this.Closing += SignalSeeker_Closing;
           // img.Width = 40;
           // img.Height = 40;
            
        }

        void SignalSeeker_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //this.DialogResult = false;
        }

        void cb(IAsyncResult result)
        {
            Console.WriteLine(commande.Result);
            
            //this.Close();
            System.Drawing.Bitmap bmp1 = (System.Drawing.Bitmap)Properties.Resources.ResourceManager.GetObject("signal"); ;
            List<Receptor> rcs = Receptor.getReceptors(client,new List<Room>());
            bool alreadyExist = false;
            List<Receptor>.Enumerator iterator = rcs.GetEnumerator();
            iterator.MoveNext();
            while (!alreadyExist && iterator.Current != null)
            {
                if (iterator.Current.commandeOn.Equals( commande.Result) || (iterator.Current.twoFrequencies && iterator.Current.commandeOff.Equals(commande.Result)))
                {
                    alreadyExist = true;
                }
                iterator.MoveNext();
            }
            Delegate myDelegate;

            if (!alreadyExist)
            {
                myDelegate = (Action)GoodCodeReceived;

            }
            else
            {
                myDelegate = (Action)BadCodeReceived;
                
            }
            this.Dispatcher.Invoke(DispatcherPriority.Normal, myDelegate);
        }
        private void GoodCodeReceived()
        {
            this.Tag = commande.Result;
            this.DialogResult = true;
            this.Close();
        }
        private void BadCodeReceived()
        {
            button.Content = "close";
            System.Drawing.Bitmap bmp1 = (System.Drawing.Bitmap)Properties.Resources.ResourceManager.GetObject("error"); ;
            img.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(bmp1.GetHbitmap(),
                IntPtr.Zero,
                System.Windows.Int32Rect.Empty,
                BitmapSizeOptions.FromWidthAndHeight(100, 100));

            textInfo.Content = "Code already used !!";
            textInfo.FontFamily = new FontFamily("Open sans");
            BrushConverter bc = new BrushConverter();
            textInfo.Foreground = (Brush)bc.ConvertFrom("#BD1D49");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
