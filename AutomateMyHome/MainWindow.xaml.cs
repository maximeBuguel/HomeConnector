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
using MahApps.Metro.Controls;
using Renci.SshNet;
using Renci.SshNet.Common;
using System.Speech.Recognition;
using System.Windows.Threading;

namespace AutomateMyHome
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : MetroWindow
    {
        public SshClient client { get; set; }
        public List<Receptor> Receptors { get; set; }
        public List<Room> Rooms { get; set; }
        public HomePanel hmPanel { get; set; }
        public ProfilsPanel prfPanel { get; set; }
        public SettingPanel settingPanel { get; set; }
        public EventPanel eventPanel { get; set; }
        public List<Scenario> scenarios { get; set; }
        /// <summary>
        /// Initialyze the entire App
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            this.Closed += MainWindow_Closed;
            logo.Source = Utils.getImageSource(Properties.Resources.homeconnector);
            this.Rooms = new List<Room>();
            this.SizeChanged += MainWindow_SizeChanged;
            pwdTxtBox.KeyDown += pwdTxtBox_KeyDown;
            ipTxtBox.KeyDown += pwdTxtBox_KeyDown;
            Image imgHome = new Image();
            imgHome.Source = Utils.getImageSource(Properties.Resources.home);
            imgHome.Width = 48;
            imgHome.Height = 48;
            homeBtn.Background = Utils.getColor(Utils.darkBlue);
            homeBtn.Content = imgHome;
            Image imgSettings = new Image();
            imgSettings.Source = Utils.getImageSource(Properties.Resources.settingsBtn);
            imgSettings.Width = 48;
            imgSettings.Height = 48;
            settingBtn.Content = imgSettings;
            settingBtn.Background = Utils.getColor(Utils.darkBlue);
            Image imgProfil = new Image();
            imgProfil.Source = Utils.getImageSource(Properties.Resources.action);
            imgProfil.Width = 48;
            imgProfil.Height = 48;
            actionBtn.Content = imgProfil;
            actionBtn.Background = Utils.getColor(Utils.darkBlue);
            Image imgEvent = new Image();
            imgEvent.Source = Utils.getImageSource(Properties.Resources.calendar);
            imgEvent.Width = 48;
            imgEvent.Height = 48;
            eventBtn.Content = imgEvent;
            eventBtn.Background = Utils.getColor(Utils.darkBlue);
            Image imgExit = new Image();
            imgExit.Source = Utils.getImageSource(Properties.Resources.exit);
            imgExit.Width = 48;
            imgExit.Height = 48;
            disconectBtn.Content = imgExit;
            disconectBtn.Background = Utils.getColor(Utils.darkBlue);
        }

        /// <summary>
        /// Function called on close that disconect the ssh client
        /// </summary>
        void MainWindow_Closed(object sender, EventArgs e)
        {
            try
            {
                this.client.Disconnect();
            }
            catch { 
            
            }
        }

        /// <summary>
        /// Function that listen to key Down to start the login action
        /// </summary>
        void pwdTxtBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) {
                loginBtn_Click(sender, e);
            }
        }

        /// <summary>
        /// Function to resize the panel when the window is resized
        /// </summary>
        void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            tabPanel.Width = this.Width - 110;
        }


        /// <summary>
        /// Login in to the app
        /// </summary>
        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                errLbl.Visibility = System.Windows.Visibility.Hidden;
                String ip = ipTxtBox.Text;
                String pwd = pwdTxtBox.Password;
                this.client = new SshClient(ip, "pi", pwd);
                client.Connect();
                client.ErrorOccurred += client_ErrorOccurred;
                this.Receptors = Receptor.getReceptors(client, this.Rooms);
                this.hmPanel = new HomePanel(Rooms);
                tabPanel.Children.Add(hmPanel);
                this.settingPanel = new SettingPanel(client);
                tabPanel.Children.Add(settingPanel);
                this.prfPanel = new ProfilsPanel(client, Receptors);
                this.eventPanel = new EventPanel(client);
                tabPanel.Children.Add(eventPanel);
                tabPanel.Children.Add(prfPanel);
                tabPanel.Visibility = System.Windows.Visibility.Visible;
                pwdTxtBox.Password = "";
                HideLogin();
                //InitialyzeVoiceCommande();

            }
            catch (SshException sshEx)
            {
                if (sshEx.Message == "Permission denied (password).")
                {
                    //ShowLogin();
                    errLbl.Content = "IP/Password incorect";
                    errLbl.Visibility = System.Windows.Visibility.Visible;
                }

            }
            catch (Exception exep)
            {
                Console.WriteLine(exep.Message);
                errLbl.Content = "Host unreacheable";
                //ShowLogin();
                errLbl.Visibility = System.Windows.Visibility.Visible;
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        /// <summary>
        /// Event when an error occured in Ssh 
        /// </summary>
        void client_ErrorOccurred(object sender, Renci.SshNet.Common.ExceptionEventArgs e)
        {
            Delegate myDelegate = (Action)sshConectionLost;
            this.Dispatcher.Invoke(DispatcherPriority.Normal, myDelegate);
        }

        /// <summary>
        /// CallBack function called when sshConnection is lost
        /// </summary>
        public void sshConectionLost()
        {
            ShowLogin();
            errLbl.Content = "Connection lost...";
        }
        /// <summary>
        /// Show the login panel
        /// </summary>
        private void ShowLogin()
        {
            hideAllMenu();
            TitleLabel.Content = "";
            tabPanel.Visibility = System.Windows.Visibility.Collapsed;
            menuPanel.Visibility = System.Windows.Visibility.Collapsed;
            loginPanel.Visibility = System.Windows.Visibility.Visible;

        }

        /// <summary>
        /// Hide the login panel
        /// </summary>
        private void HideLogin()
        {
            hideAllMenu();
            this.ShowHome();
            ShowMenu();
            loginPanel.Visibility = System.Windows.Visibility.Collapsed;
        }

        /// <summary>
        /// Show the menu panel
        /// </summary>
        private void ShowMenu()
        {
            menuPanel.Visibility = System.Windows.Visibility.Visible;
        }

        /// <summary>
        /// Show the Home panel
        /// </summary>
        private void ShowHome()
        {
            hmPanel.Rooms = new List<Room>();
            this.Receptors = Receptor.getReceptors(client, hmPanel.Rooms);
            TitleLabel.Content = "My Home";
            hmPanel.InitialyseHomePanel();
            hmPanel.Visibility = System.Windows.Visibility.Visible;
        }


        /// <summary>
        /// Hide the Home panel
        /// </summary>
        private void HideHome()
        {
            hmPanel.Children.Clear();
            hmPanel.Visibility = System.Windows.Visibility.Collapsed;
        }

        /// <summary>
        /// Show the Profils panel
        /// </summary>
        private void ShowProfils()
        {
            prfPanel.InitialyzeProfils();
            TitleLabel.Content = "My Scenarios";
            prfPanel.Visibility = System.Windows.Visibility.Visible;
        }
        /// <summary>
        /// Hide the Profils panel
        /// </summary>
        private void HideProfils()
        {
            prfPanel.Visibility = System.Windows.Visibility.Collapsed;
        }
        /// <summary>
        /// Show the Setting panel
        /// </summary>
        private void ShowSetting()
        {
            settingPanel.InitialyzeSettingPanel();
            TitleLabel.Content = "Settings";
            settingPanel.Visibility = System.Windows.Visibility.Visible;

        }
        /// <summary>
        /// Hide the Setting panel
        /// </summary>
        private void HideSetting()
        {
            settingPanel.Children.Clear();
            settingPanel.Visibility = System.Windows.Visibility.Collapsed;
        }


        /// <summary>
        /// Home button click, show Home panel, hide everything else
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            this.hideAllMenu();
            this.ShowHome();
            Mouse.OverrideCursor = Cursors.Arrow;
        }


        /// <summary>
        /// Profils button click, show Profil panel, hide everything else
        /// </summary>
        private void actionBtn_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            this.hideAllMenu();
            this.ShowProfils();
            Mouse.OverrideCursor = Cursors.Arrow;
        }


        /// <summary>
        /// Disconect button click, show login panel, hide everything else , disconect client 
        /// </summary>
        private void disconectBtn_Click(object sender, RoutedEventArgs e)
        {
            tabPanel.Visibility = System.Windows.Visibility.Collapsed;
            client.Disconnect();
            this.hideAllMenu();
            ShowLogin();

        }

        /// <summary>
        /// Event button click, show Event panel, hide everything else
        /// </summary>
        private void eventBtn_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            this.hideAllMenu();
            ShowEvent();
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// Hide every panel
        /// </summary>
        private void hideAllMenu()
        {
            HideHome();
            HideProfils();
            HideSetting();
            HideEvent();
        }


        /// <summary>
        /// Hide event panel
        /// </summary>
        private void HideEvent()
        {
            
            this.eventPanel.Visibility = System.Windows.Visibility.Collapsed;
        }
        /// <summary>
        /// Show event panel
        /// </summary>
        private void ShowEvent()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            TitleLabel.Content = "My Events";
            this.eventPanel.Visibility = System.Windows.Visibility.Visible;
            this.eventPanel.InitializeEventList();
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// Setting button click, show setting panel, hide everything else
        /// </summary>
        private void settingBtn_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            hideAllMenu();
            ShowSetting();
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// Initialyze the voice commande
        /// </summary>
        //public void InitialyzeVoiceCommande()
        //{
        //    this.scenarios = Scenario.getScenarios(client);
        //    Choices choice = new Choices();
        //    List<string> words = new List<string>();
        //    foreach (Scenario s in scenarios)
        //    {
        //        words.Add(s.name.Replace('-',' '));
        //    }
        //    choice.Add(words.ToArray());
        //    SpeechRecognizer sr = new SpeechRecognizer();
        //    GrammarBuilder gb = new GrammarBuilder();
        //    gb.Append(choice);
        //    Grammar gr = new Grammar(gb);
        //    sr.LoadGrammar(gr);
        //    sr.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(sr_SpeechRecognized);
        //}

        ///// <summary>
        /////Event on speech recognition
        ///// </summary>
        //private void sr_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        //{
        //    foreach (Scenario s in scenarios)
        //    {
        //        if (e.Result.Text == s.name.Replace('-',' '))
        //        {
        //            s.launch(client);
        //        }

        //    }
        //}
    }
}
