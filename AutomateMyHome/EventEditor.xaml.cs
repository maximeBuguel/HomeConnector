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
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using System.Collections;
using Renci.SshNet;

namespace AutomateMyHome
{
    /// <summary>
    /// Logique d'interaction pour EventEditor.xaml
    /// </summary>
    public partial class EventEditor : MetroWindow
    {
        public EventEditor(List<Scenario> scenarioList,SshClient client,Event ev)
        {
            InitializeComponent();

            ComboBox comboScenario = new ComboBox();
            ComboBox comboMinutes = new ComboBox();
            ComboBox comboHours = new ComboBox();
            ComboBox comboDays = new ComboBox();
            ComboBox comboMonth = new ComboBox();
            ComboBox comboWeeksDay = new ComboBox();
            List<ComboBox> comboList = new List<ComboBox>();

            
            addAllScenarioToCombobox(comboScenario,scenarioList,comboList);
            addToComboBox(60, comboMinutes,comboList);//minutes
            addToComboBox(24, comboHours,comboList);//heures
            addToComboBox(31, comboDays,comboList);//jours
            addToComboBox(12, comboMonth,comboList);//mois
            addToComboBox(-1, comboWeeksDay,comboList);//jour de la semaine

            
            DockPanel dp = new DockPanel();
            BrushConverter bc = new BrushConverter();
            dp.Background = (Brush)bc.ConvertFrom("#41B1E1");
            dp.Margin = new Thickness(10, 10, 10, 0);


            addToDockPanel("Scenario",comboScenario);
            addToDockPanel("Minutes",comboMinutes);
            addToDockPanel("Hours",comboHours);
            addToDockPanel("Days",comboDays);
            addToDockPanel("Month",comboMonth);
            addToDockPanel("Day of the Week",comboWeeksDay);
            

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
            img1.Tag = new specialObjectForButtonSender(client, comboList,ev);
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
        }

        private void addToDockPanel(String comboName,ComboBox combo){
            DockPanel dp = new DockPanel();
            BrushConverter bc = new BrushConverter();
            dp.Background = (Brush)bc.ConvertFrom("#41B1E1");
            dp.Margin = new Thickness(10, 10, 10, 0);           
            Label name = new Label();
            name.FontFamily = new FontFamily("Open Sans");
            name.FontWeight = FontWeights.Bold;
            name.Content = comboName;
            name.FontSize = 24;
            name.Foreground = (Brush)bc.ConvertFrom("#FFFFFF");
            dp.Children.Add(name);
            dp.Children.Add(combo);
            mainPanel.Children.Add(dp);
           
        }
        private void img1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            specialObjectForButtonSender args = (specialObjectForButtonSender)((Image)sender).Tag;
            ArrayList list = new ArrayList();

            foreach (ComboBox cb in args.Combolist)
            {
                list.Add(cb.SelectedItem);
            }
            if (!args.Ev.isACreator)
            {
                args.Ev.removeFromContrab();
            }
            Event ev = new Event(args.Client, list);
            ev.addToContrab();
            this.DialogResult = true;
            this.Close();
        }

        private void img2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        enum Days
        {
            Monday,
            Tuesday,
            Wednesday,
            Thursday,
            Friday,
            Saturday,
            Sunday
        };

        private void addToComboBox(int number, ComboBox combo,List<ComboBox> comboList)
        {
            combo.SelectedIndex = 0;
            combo.Items.Add("*");
            if (number == -1)
            {
                
                foreach (Days day in Enum.GetValues(typeof(Days)))
                {
                    combo.Items.Add(day);
                }
            }
            else
            {
                int i = 0;
                if (number == 60 || number == 24)
                {
                    for (i = 0; i < number ; i++)
                    {
                        combo.Items.Add(i);
                    }
                }
                else
                {
                    for (i = 1; i < number +1; i++)
                    {
                        combo.Items.Add(i);
                    }
                }

                
                
            }
            BrushConverter bc = new BrushConverter();
            combo.FontFamily = new FontFamily("Open Sans");
            combo.FontSize = 24;
            combo.Foreground = (Brush)bc.ConvertFrom("#41B1E1");
            comboList.Add(combo);


        }

        private void addAllScenarioToCombobox(ComboBox combo, List<Scenario> scenarioList, List<ComboBox> comboList)
        {
            combo.SelectedIndex = 0;
            foreach (Scenario sc in scenarioList)
            {
                combo.Items.Add(sc.name.Replace("-", " "));
            }
            BrushConverter bc = new BrushConverter();
            combo.FontFamily = new FontFamily("Open Sans");
            combo.FontSize = 24;
            combo.Foreground = (Brush)bc.ConvertFrom("#41B1E1");
            comboList.Add(combo);
            
        }
    }
}