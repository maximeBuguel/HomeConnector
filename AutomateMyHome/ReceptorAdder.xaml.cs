using MahApps.Metro.Controls;
using Renci.SshNet;
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

namespace AutomateMyHome
{
    /// <summary>
    /// Logique d'interaction pour ReceptorAdder.xaml
    /// </summary>
    public partial class ReceptorAdder : MetroWindow
    {
        TextBox nameBox;
        TextBox roomBox;
        TextBox freq1Box;
        TextBox freq2Box;
        SshClient client;
        ComboBox cb;
        String type;
        /// <summary>
        /// Create a new windows to add new receptors
        /// </summary>
        public ReceptorAdder(SshClient c, String s)
        {
            InitializeComponent();
            this.client = c;
            this.type = s;
            nameBox = new TextBox();
            nameBox.FontFamily = Utils.appFont;
            nameBox.FontWeight = Utils.weightFont;
            nameBox.Foreground = Utils.getColor(Utils.white);
            nameBox.Background = Utils.getColor(Utils.lightBlue);
            nameBox.FontSize = 24;
            nameBox.Text = "Enter a name here";
            nameBox.TextAlignment = TextAlignment.Center;
            nameBox.Margin = new Thickness(10, 10, 10, 10);
            mainPanel.Children.Add(nameBox);
            Label roomLabel = new Label();
            roomLabel.FontFamily = Utils.appFont;
            roomLabel.FontWeight = Utils.weightFont;
            roomLabel.Foreground = Utils.getColor(Utils.white);
            roomLabel.Background = Utils.getColor(Utils.lightBlue);
            roomLabel.FontSize = 24;
            roomLabel.Content = "Room :";
            roomBox = new TextBox();
            roomBox.FontFamily = Utils.appFont;
            roomBox.FontWeight = Utils.weightFont;
            roomBox.Foreground = Utils.getColor(Utils.lightBlue);
            roomBox.Background = Utils.getColor(Utils.white);
            roomBox.FontSize = 24;
            DockPanel roomPanel = new DockPanel();
            roomPanel.Children.Add(roomLabel);
            roomPanel.Children.Add(roomBox);
            roomPanel.Margin = new Thickness(10, 10, 10, 10);
            //roomPanel.Width = mainPanel.Width - 20;
            mainPanel.Children.Add(roomPanel);
            DockPanel nbFreqPanel = new DockPanel();
            Label freqLabel = new Label();
            freqLabel.FontFamily = Utils.appFont;
            freqLabel.FontWeight = Utils.weightFont;
            freqLabel.Foreground = Utils.getColor(Utils.white);
            freqLabel.Background = Utils.getColor(Utils.lightBlue); ;
            freqLabel.FontSize = 24;
            freqLabel.Content = "Type :";
            ComboBoxItem cbi1 = new ComboBoxItem();
            cbi1.FontFamily = Utils.appFont;
            cbi1.FontSize = 24;
            cbi1.Content = "Toggle";
            ComboBoxItem cbi2 = new ComboBoxItem();
            cbi2.FontFamily = Utils.appFont;
            cbi2.FontSize = 24;
            cbi2.Content = "On/Off";
            List<ComboBoxItem> cbL = new List<ComboBoxItem>();
            cbL.Add(cbi1);
            cbL.Add(cbi2);
            DockPanel freq2Panel = new DockPanel();
            cb = new ComboBox();
            cb.SelectedIndex = 0;
            cb.ItemsSource = cbL;
            cb.Tag = freq2Panel;
            cb.SelectionChanged += cb_SelectionChanged;
            nbFreqPanel.Children.Add(freqLabel);
            nbFreqPanel.Children.Add(cb);
            nbFreqPanel.Margin = new Thickness(10, 10, 10, 10);
            mainPanel.Children.Add(nbFreqPanel);

            Label toggleLabel = new Label();
            toggleLabel.FontFamily = Utils.appFont;
            toggleLabel.FontWeight = Utils.weightFont;
            toggleLabel.Foreground = Utils.getColor(Utils.white);
            toggleLabel.Background = Utils.getColor(Utils.lightBlue); 
            toggleLabel.FontSize = 24;
            toggleLabel.Content = "Button 1 :";
            freq1Box = new TextBox();
            freq1Box.FontFamily = Utils.appFont;
            freq1Box.FontWeight = Utils.weightFont;
            freq1Box.Foreground = Utils.getColor(Utils.lightBlue);
            freq1Box.Background = Utils.getColor(Utils.white);
            freq1Box.FontSize = 24;
            freq1Box.Width = 150;
            freq1Box.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;

            System.Drawing.Bitmap bmpSetting = (System.Drawing.Bitmap)Properties.Resources.settingsB;
            Image imgSetting = new Image();
            imgSetting.Source = Utils.getImageSource(bmpSetting);
            imgSetting.Width = 24;
            imgSetting.Height = 24;
            // imgSetting.Margin = new Thickness(5, 5, 5, 5);
            imgSetting.Cursor = Cursors.Hand;
            imgSetting.Tag = freq1Box;
            imgSetting.MouseLeftButtonDown += imgSetting_MouseLeftButtonDown;
            DockPanel freq1Panel = new DockPanel();
            freq1Panel.Children.Add(toggleLabel);
            freq1Panel.Children.Add(freq1Box);
            WrapPanel imgPanel1 = new WrapPanel();
            imgPanel1.Children.Add(imgSetting);
            imgPanel1.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            DockPanel.SetDock(imgPanel1, Dock.Right);
            imgPanel1.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            freq1Panel.Children.Add(imgPanel1);
            freq1Panel.Margin = new Thickness(10, 10, 10, 10);
            mainPanel.Children.Add(freq1Panel);

            Label onOffLabel = new Label();
            onOffLabel.FontFamily = Utils.appFont;
            onOffLabel.FontWeight = Utils.weightFont;
            onOffLabel.Foreground = Utils.getColor(Utils.white);
            onOffLabel.Background = Utils.getColor(Utils.lightBlue);
            onOffLabel.FontSize = 24;
            onOffLabel.Content = "Button 2 :";
            freq2Box = new TextBox();
            freq2Box.FontFamily = Utils.appFont;
            freq2Box.FontWeight = Utils.weightFont;
            freq2Box.Foreground = Utils.getColor(Utils.lightBlue);
            freq2Box.Background = Utils.getColor(Utils.white);
            freq2Box.FontSize = 24;
            freq2Box.Width = 150;
            freq2Box.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;

            
            freq2Panel.Children.Add(onOffLabel);
            freq2Panel.Children.Add(freq2Box);
            WrapPanel imgPanel2 = new WrapPanel();
            Image imgSetting2 = new Image();
            imgSetting2.Source = Utils.getImageSource(bmpSetting);
            imgSetting2.Width = 24;
            imgSetting2.Height = 24;
            imgSetting2.Tag = freq2Box;
            imgSetting2.MouseLeftButtonDown += imgSetting_MouseLeftButtonDown;
            // imgSetting.Margin = new Thickness(5, 5, 5, 5);
            imgSetting2.Cursor = Cursors.Hand;
            imgPanel2.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            imgPanel2.Children.Add(imgSetting2);
            imgPanel2.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            DockPanel.SetDock(imgPanel2, Dock.Right);
            freq2Panel.Children.Add(imgPanel2);
            freq2Panel.Visibility = System.Windows.Visibility.Collapsed;
            freq2Panel.Margin = new Thickness(10, 10, 10, 10);
            mainPanel.Children.Add(freq2Panel);
            WrapPanel btns = new WrapPanel();
            btns.Margin = new Thickness(10, 10, 10, 0);
            btns.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;

            Image img1 = new Image();
            img1.Source = Utils.getImageSource(Properties.Resources.ok);
            img1.Width = 40;
            img1.Height = 40;
            img1.MouseLeftButtonDown += img1_MouseLeftButtonDown;
            btns.Children.Add(img1);

            Image img2 = new Image();
            img2.Source = Utils.getImageSource(Properties.Resources.cancel);
            img2.Width = 40;
            img2.Height = 40;
            img2.MouseLeftButtonDown += img2_MouseLeftButtonDown;

            btns.Children.Add(img2);

            mainPanel.Children.Add(btns);
        }
        /// <summary>
        /// Close the windows on cancel button
        /// </summary>
        private void img2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //Cancel
            this.Close();
        }

        /// <summary>
        /// Send the code 1 of a receptor (often the turn On)
        /// </summary>
        private void img1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            String name = nameBox.Text;
            String roomName = roomBox.Text;
            String F1;
            String F2;
            bool twoFrequencies;
            if (cb.SelectedIndex == 0)
            {
                F1 = freq1Box.Text;
                F2 = freq1Box.Text;
                twoFrequencies = false;
            }
            else {
                F1 = freq1Box.Text;
                F2 = freq2Box.Text;
                twoFrequencies = true;
            }
            Room room = new Room(roomName);
            if (name.Equals("") || roomName.Equals("") || F1.Equals("") || F2.Equals(""))
            {
                //Err
            }
            else
            {
                List<Room> rooms = new List<Room>();
                List<Receptor> receps = Receptor.getReceptors(client, rooms);
                Receptor r = new Receptor(name, room, twoFrequencies, F1, F2, client, type);
                receps.Add(r);
                Receptor.refresh(client, receps);
                this.DialogResult = true;
                this.Close();
            }
            
        }
        /// <summary>
        /// Start the seeking signal window 
        /// </summary>
        void imgSetting_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TextBox tb = (TextBox)((Image)sender).Tag;
            SignalSeeker sngSeeker = new SignalSeeker(client);
            sngSeeker.ShowDialog();
            if ((bool)sngSeeker.DialogResult)
            {
                tb.Text = (String) sngSeeker.Tag;
            }

        }


        /// <summary>
        /// Add a new signal if the combo box change from toggle to 2 frequencies
        /// </summary>
        void cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            if (cb.SelectedIndex == 1)
            {
                ((DockPanel)cb.Tag).Visibility = System.Windows.Visibility.Visible;
            }
            else {
                ((DockPanel)cb.Tag).Visibility = System.Windows.Visibility.Collapsed;
            }

        }
    }
}
