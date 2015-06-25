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
        /// <summary>
        /// Create a new Windows with combobox  
        /// </summary>
        public EventEditor(List<String> scenarioList,SshClient client,Event ev)
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
            dp.Background = Utils.getColor(Utils.lightBlue);
            dp.Margin = new Thickness(10, 10, 10, 0);


            addToDockPanel("Scenario",comboScenario);
            addToDockPanel("Minute",comboMinutes);
            addToDockPanel("Hour",comboHours);
            addToDockPanel("Day",comboDays);
            addToDockPanel("Month",comboMonth);
            addToDockPanel("Day of the Week",comboWeeksDay);
            

            WrapPanel btns = new WrapPanel();
            btns.Margin = new Thickness(10, 10, 10, 0);
            btns.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;

            
            Image img1 = new Image();
            img1.Source = Utils.getImageSource(Properties.Resources.ok);
            img1.Width = 40;
            img1.Height = 40;
            img1.MouseLeftButtonDown += img1_MouseLeftButtonDown;
            img1.Tag = new specialObjectForButtonSender(client, comboList,ev);
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
        /// add a dockPanel with combobox to the mainPanel
        /// </summary>
        private void addToDockPanel(String comboName,ComboBox combo){
            DockPanel dp = new DockPanel();
            BrushConverter bc = new BrushConverter();
            dp.Background = Utils.getColor(Utils.lightBlue);
            dp.Margin = new Thickness(10, 10, 10, 0);           
            Label name = new Label();
            name.FontFamily = Utils.appFont;
            name.FontWeight = Utils.weightFont;
            name.Content = comboName;
            name.FontSize = 24;
            name.Foreground = Utils.getColor(Utils.white);
            dp.Children.Add(name);
            dp.Children.Add(combo);
            mainPanel.Children.Add(dp);
           
        }

        /// <summary>
        /// save modification or add new Event creation
        /// </summary>
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

        /// <summary>
        /// cancel modification of that Event
        /// </summary>
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

        /// <summary>
        ///  fill a combobox with the right String content and add it to a List of combobox
        /// </summary>
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
            combo.FontFamily = Utils.appFont;
            combo.FontSize = 24;
            combo.Foreground = Utils.getColor(Utils.lightBlue);
            comboList.Add(combo);


        }

        /// <summary>
        ///  fill a combobox with all the scenarios name  and add it to a List of combobox
        /// </summary>
        private void addAllScenarioToCombobox(ComboBox combo, List<String> scenarioList, List<ComboBox> comboList)
        {
            combo.SelectedIndex = 0;
            foreach (String sc in scenarioList)
            {
                combo.Items.Add(sc.Replace("-", " "));
            }
            BrushConverter bc = new BrushConverter();
            combo.FontFamily = Utils.appFont;
            combo.FontSize = 24;
            combo.Foreground = Utils.getColor(Utils.lightBlue);
            comboList.Add(combo);
            
        }
    }
}
