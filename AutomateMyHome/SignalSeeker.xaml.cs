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

        /// <summary>
        /// Create a signal seeker and launch the programme on the box to scan the next code
        /// </summary>
        public SignalSeeker(SshClient c)
        {
            InitializeComponent();
            client = c;
            commande = c.CreateCommand("sudo ./HomeConnector/GetNextSignal");
            commande.BeginExecute(new AsyncCallback(cb));
            img.Source = Utils.getImageSource(Properties.Resources.signal);
            
        }

        /// <summary>
        /// CallBack function called when the Box Sniffer programme is over
        /// </summary>
        void cb(IAsyncResult result)
        {
            
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
            if(commande.Result.Equals("-1")){
                myDelegate = (Action)DelayPassedReceived;
            }
            else if (!alreadyExist )
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
            
            img.Source = Utils.getImageSource(Properties.Resources.error);

            textInfo.Content = "Code already used !!";
            textInfo.FontFamily = Utils.appFont;
            textInfo.Foreground = Utils.getColor(Utils.red);
        }

        /// <summary>
        /// 
        /// </summary>
        private void DelayPassedReceived()
        {
            button.Content = "close";

            img.Source = Utils.getImageSource(Properties.Resources.error);

            textInfo.Content = "No signal received  !!";
            textInfo.FontFamily = Utils.appFont;
            textInfo.Foreground = Utils.getColor(Utils.red);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
