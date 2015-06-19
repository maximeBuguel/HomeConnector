﻿using MahApps.Metro.Controls;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AutomateMyHome
{
    /// <summary>
    /// Logique d'interaction pour ScenarioEditor.xaml
    /// </summary>
    public partial class ScenarioEditor : MetroWindow
    {
        public string output { get; set; }
        TextBox nameBox;
        SshClient client;
        public ScenarioEditor(SshClient client,Scenario sc)
        {
            this.Tag = sc;
            this.client = client;
            output = "";
            InitializeComponent();
            nameBox = new TextBox();
            nameBox.FontFamily = new FontFamily("Open Sans");
            nameBox.FontWeight = FontWeights.Bold;
            nameBox.Foreground = Utils.getColor(Utils.white);
            nameBox.Background = Utils.getColor(Utils.lightBlue);
            nameBox.FontSize = 24;
            nameBox.Text = sc.name.Replace("-"," ");
            nameBox.TextAlignment = TextAlignment.Center;
            nameBox.Margin = new Thickness(10, 10, 10, 0);
            nameBox.KeyDown += nameBox_KeyDown;
            mainPanel.Children.Add(nameBox);

            List<ComboBox> argList = new List<ComboBox>();
            List<Receptor> rcs = Receptor.getReceptors(client, new List<Room>());
            foreach (Receptor rc in rcs)
            {
                
                DockPanel dp = new DockPanel();
                dp.Background = Utils.getColor(Utils.lightBlue);
                dp.Margin = new Thickness(10, 10, 10, 0);
                

                System.Drawing.Bitmap bmp = rc.getIcon();
                Image img = new Image();
                img.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(bmp.GetHbitmap(),
                    IntPtr.Zero,
                    System.Windows.Int32Rect.Empty,
                    BitmapSizeOptions.FromWidthAndHeight(100, 100));
                img.Width = 30;
                img.Height = 30;

                Label name = new Label();
                name.FontFamily = new FontFamily("Open Sans");
                name.FontWeight = FontWeights.Bold;
                name.Content = rc.Name;
                name.FontSize = 24;
                name.Foreground = Utils.getColor(Utils.white);

                Label roomName = new Label();
                roomName.FontFamily = new FontFamily("Open Sans");
                roomName.Content = "- " + rc.Room.Name;
                roomName.FontSize = 24;
                roomName.Foreground = Utils.getColor(Utils.white);
                List<ComboBoxItem> listCb = new List<ComboBoxItem>();

                ComboBoxItem cbi = new ComboBoxItem();
                cbi.FontFamily = new FontFamily("Open Sans");
                cbi.FontSize = 24;
                cbi.Content = "Do nothing";

                listCb.Add(cbi);
                

                if (rc.twoFrequencies)
                {
                    cbi = new ComboBoxItem();
                    cbi.FontFamily = new FontFamily("Open Sans");
                    cbi.FontSize = 24;
                    cbi.Content = "Turn on";
                    cbi.Foreground = Utils.getColor(Utils.green);
                    listCb.Add(cbi);

                    cbi = new ComboBoxItem();
                    cbi.FontFamily = new FontFamily("Open Sans");
                    cbi.FontSize = 24;
                    cbi.Content = "Turn off";
                    cbi.Foreground = Utils.getColor(Utils.green);
                    listCb.Add(cbi);
                }
                else
                {
                    cbi = new ComboBoxItem();
                    cbi.FontFamily = new FontFamily("Open Sans");
                    cbi.FontSize = 24;
                    cbi.Content = "Toggle";
                    cbi.Foreground = (Brush)bc.ConvertFrom("#5e3ab8");
                    listCb.Add(cbi);
                }
                
                ComboBox cb = new ComboBox();
                cb.Tag = rc;
                cb.SelectedIndex = 0;

                if (sc.codes.Contains(rc.commandeOn))
                {
                    cb.SelectedIndex = 1;
                }
                else if (rc.twoFrequencies && sc.codes.Contains(rc.commandeOff))
                {
                    cb.SelectedIndex = 2;
                }
                
                
                cb.ItemsSource = listCb;

                

                dp.Children.Add(img);
                dp.Children.Add(name);
                dp.Children.Add(roomName);
                dp.Children.Add(cb);
                argList.Add(cb);
                mainPanel.Children.Add(dp);
            }
            WrapPanel btns = new WrapPanel();
            btns.Margin = new Thickness(10, 10, 10, 0);
            btns.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;

            System.Drawing.Bitmap bmp1 = (System.Drawing.Bitmap)Properties.Resources.ResourceManager.GetObject("ok"); ;
            Image img1 = new Image();
            img1.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(bmp1.GetHbitmap(),
                IntPtr.Zero,
                System.Windows.Int32Rect.Empty,
                BitmapSizeOptions.FromWidthAndHeight(100, 100));
            img1.Width = 40;
            img1.Height = 40;
            img1.MouseLeftButtonDown += img1_MouseLeftButtonDown;
            img1.Tag = argList;

            btns.Children.Add(img1);

            System.Drawing.Bitmap bmp2 = (System.Drawing.Bitmap)Properties.Resources.ResourceManager.GetObject("cancel"); ;
            Image img2 = new Image();
            img2.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(bmp2.GetHbitmap(),
                IntPtr.Zero,
                System.Windows.Int32Rect.Empty,
                BitmapSizeOptions.FromWidthAndHeight(100, 100));
            img2.Width = 40;
            img2.Height = 40;
            img2.MouseLeftButtonDown += img2_MouseLeftButtonDown;

            btns.Children.Add(img2);
            
            mainPanel.Children.Add(btns);
            nameBox.Focus();
        }

        void nameBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.D6 || (e.Key == Key.Subtract))
            {
                e.Handled = true;
            }
        }

        void img1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if ((((Scenario)this.Tag).name.Equals(nameBox.Text.Replace(" ", "-"))) || (Scenario.canCreate(client, nameBox.Text.Replace(" ", "-") + ".sh") && !nameBox.Text.Equals("")))
            {
                this.DialogResult = true;
                List<ComboBox> args = (List<ComboBox>)((Image)sender).Tag;
                List<string> codes = new List<string>();
                string ouput = "";
                foreach (ComboBox cb in args)
                {
                    string act = cb.Text;

                    if (act.Equals("Turn on") || act.Equals("Toggle"))
                    {
                        Receptor r = (Receptor)cb.Tag;
                        ouput += ouput + "sudo ./HomeConnector/codesend " + r.commandeOn + "\n";
                        codes.Add(r.commandeOn);

                    }
                    else if (act.Equals("Turn off"))
                    {
                        Receptor r = (Receptor)cb.Tag;
                        ouput += ouput + "sudo ./HomeConnector/codesend " + r.commandeOff + "\n";
                        codes.Add(r.commandeOff);
                    }
                }

                this.Tag = new Scenario(nameBox.Text.Replace(" ", "-"), codes);
                this.output = output;
                this.Close();
            }
            else
            {
                nameBox.Background = Brushes.Red;
            }
            
        }

        void img2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
