﻿using System;
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

        public MainWindow()
        {


            InitializeComponent();
            System.Drawing.Bitmap homeConnectorBmp = (System.Drawing.Bitmap)Properties.Resources.ResourceManager.GetObject("homeconnector"); ;
            logo.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(homeConnectorBmp.GetHbitmap(),
                       IntPtr.Zero,
                       System.Windows.Int32Rect.Empty,
                       BitmapSizeOptions.FromWidthAndHeight(100, 100));
            this.Rooms = new List<Room>();
            this.SizeChanged += MainWindow_SizeChanged;
            pwdTxtBox.KeyDown += pwdTxtBox_KeyDown;
            ipTxtBox.KeyDown += pwdTxtBox_KeyDown;


        }

        void pwdTxtBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) {
                loginBtn_Click(sender, e);
            }
        }

        void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            tabPanel.Width = this.Width - 110;
        }



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

            }
            catch (SshException sshEx)
            {
                if (sshEx.Message == "Permission denied (password).")
                {
                    ShowLogin();
                    errLbl.Content = "IP/Password incorect";
                    errLbl.Visibility = System.Windows.Visibility.Visible;
                }

            }
            catch (Exception exep)
            {
                Console.WriteLine(exep.Message);
                errLbl.Content = "Host unreacheable";
                ShowLogin();
                errLbl.Visibility = System.Windows.Visibility.Visible;
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void ShowLogin()
        {
            TitleLabel.Content = "";
            menuPanel.Visibility = System.Windows.Visibility.Collapsed;
            loginPanel.Visibility = System.Windows.Visibility.Visible;

        }

        private void HideLogin()
        {
            hideAllMenu();
            this.ShowHome();
            ShowMenu();
            loginPanel.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void ShowMenu()
        {
            menuPanel.Visibility = System.Windows.Visibility.Visible;
        }

        private void ShowHome()
        {
            hmPanel.Rooms = new List<Room>();
            this.Receptors = Receptor.getReceptors(client, hmPanel.Rooms);
            TitleLabel.Content = "My Home";
            hmPanel.InitialyseHomePanel();
            hmPanel.Visibility = System.Windows.Visibility.Visible;
        }



        private void HideHome()
        {
            hmPanel.Children.Clear();
            hmPanel.Visibility = System.Windows.Visibility.Collapsed;
        }


        private void ShowProfils()
        {
            prfPanel.InitialyzeProfils();
            TitleLabel.Content = "My Scenarios";
            prfPanel.Visibility = System.Windows.Visibility.Visible;
            InitialyzeVoiceCommande();
        }

        private void HideProfils()
        {
            prfPanel.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void ShowSetting()
        {
            settingPanel.InitialyzeSettingPanel();
            TitleLabel.Content = "Settings";
            settingPanel.Visibility = System.Windows.Visibility.Visible;

        }

        private void HideSetting()
        {
            settingPanel.Children.Clear();
            settingPanel.Visibility = System.Windows.Visibility.Collapsed;
        }

        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.hideAllMenu();
            this.ShowHome();
        }

        private void actionBtn_Click(object sender, RoutedEventArgs e)
        {
            this.hideAllMenu();
            this.ShowProfils();

        }

        private void disconectBtn_Click(object sender, RoutedEventArgs e)
        {
            tabPanel.Visibility = System.Windows.Visibility.Collapsed;
            client.Disconnect();
            this.hideAllMenu();
            ShowLogin();

        }
        private void eventBtn_Click(object sender, RoutedEventArgs e)
        {
            
            this.hideAllMenu();
            ShowEvent();

        }

        private void hideAllMenu()
        {
            HideHome();
            HideProfils();
            HideSetting();
            HideEvent();
        }

        private void HideEvent()
        {
            
            this.eventPanel.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void ShowEvent()
        {
            TitleLabel.Content = "My Events";
            this.eventPanel.Visibility = System.Windows.Visibility.Visible;
            this.eventPanel.InitializeEventList();
        }

        private void settingBtn_Click(object sender, RoutedEventArgs e)
        {
            hideAllMenu();
            ShowSetting();

        }
       

        public void InitialyzeVoiceCommande()
        {

            this.scenarios = Scenario.getScenarios(client);
            Choices choice = new Choices();
            List<string> words = new List<string>();
            foreach (Scenario s in scenarios)
            {
                words.Add(s.name.Replace('-',' '));
            }
            choice.Add(words.ToArray());
            SpeechRecognizer sr = new SpeechRecognizer();
            GrammarBuilder gb = new GrammarBuilder();
            gb.Append(choice);
            Grammar gr = new Grammar(gb);
            sr.LoadGrammar(gr);
            sr.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(sr_SpeechRecognized);
        }

        private void sr_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            foreach (Scenario s in scenarios)
            {
                if (e.Result.Text == s.name.Replace('-',' '))
                {
                    s.launch(client);
                }

            }
        }
    }
}