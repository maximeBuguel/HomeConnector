using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Renci.SshNet;
using System.Windows.Media;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Input;


namespace AutomateMyHome
{
    
    public class EventPanel : StackPanel
    {
        public SshClient client { get; set; }
        public EventPanel(SshClient c)
        {
            this.client = c;
            InitializeEventList();
        }
        public void InitializeEventList()
        {
            this.Children.Clear();
            List<Event> evList = Event.getAllEvent(client);
            this.Width = this.Width - 110;
            this.Background = Utils.getColor(Utils.darkBlue);
            //evList.Add(new Event());
            List<String> senarios = Scenario.getScenariosNames(this.client);
            
            foreach (Event ev in evList)
            {

                DockPanel dp = new DockPanel();
                dp.Background = Utils.getColor(Utils.lightBlue);
                dp.Margin = new Thickness(10, 10, 10, 0);


                Label name = new Label();
                name.FontWeight = Utils.weightFont;
                name.FontFamily = Utils.appFont;
                name.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                name.FontSize = 24;
                name.Foreground = Utils.getColor(Utils.white);
                Image imgDel = new Image();
                imgDel.Source = Utils.getImageSource(Properties.Resources.delete);
                imgDel.Width = 24;
                imgDel.Height = 24;
                imgDel.Margin = new Thickness(5, 0, 5, 0);
                imgDel.Cursor = Cursors.Hand;
                imgDel.Tag = ev;
                imgDel.MouseLeftButtonDown += imgEventDel_MouseLeftButtonDown;
                Image imgSetting = new Image();
                imgSetting.Source = Utils.getImageSource(Properties.Resources.settings);
                imgSetting.Width = 24;
                imgSetting.Height = 24;
                // imgSetting.Margin = new Thickness(5, 5, 5, 5);
                imgSetting.Cursor = Cursors.Hand;
                imgSetting.Tag = new SpecialEventToSendEvAndScenario(senarios, ev);
                imgSetting.MouseLeftButtonDown += imgEventSetting_MouseLeftDown;

                WrapPanel dbutt = new WrapPanel();
                dbutt.Children.Add(imgSetting);
                if (ev.isACreator)
                {
                    name.Content = "Create an Event";
                }
                else
                {
                    name.Content = ev.getName();
                    dbutt.Children.Add(imgDel);
                }
                dbutt.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                dbutt.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                //dp.Children.Add(imgStart);
                dp.Children.Add(name);
                DockPanel.SetDock(dbutt, Dock.Right);
                dp.Children.Add(dbutt);
                this.Children.Add(dp);
            }
            DockPanel add = new DockPanel();
            add.Background = Utils.getColor(Utils.lightBlue);
            add.Margin = new Thickness(10, 10, 10, 0);
            Button btnPlus = new Button();
            Image imgPlus = new Image();
            imgPlus.Source = Utils.getImageSource(Properties.Resources.Plus);
            imgPlus.Width = 32;
            imgPlus.Height = 32;
            imgPlus.Margin = new Thickness(10, 10, 10, 10);
            imgPlus.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            btnPlus.Tag = senarios;
            btnPlus.Content = imgPlus;
            btnPlus.Background = Utils.getColor(Utils.green);
            btnPlus.Click += btnPlus_Click;
            add.Background = Utils.getColor(Utils.green); ;
            add.Children.Add(btnPlus);
            this.Children.Add(add);

        }

        void btnPlus_Click(object sender, RoutedEventArgs e)
        {
            List<String> se = (List<String>)((Button)sender).Tag;
            EventEditor Ee = new EventEditor(se, this.client, new Event());
            Ee.ShowDialog();
            if ((bool)Ee.DialogResult)
            {
                InitializeEventList();
            }
        }

        void imgEventSetting_MouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            SpecialEventToSendEvAndScenario se = (SpecialEventToSendEvAndScenario)((Image)sender).Tag;

            EventEditor Ee = new EventEditor(se.scenarList, this.client, se.ev);
            Ee.ShowDialog();
            if ((bool)Ee.DialogResult)
            {

                InitializeEventList();
            }
        }

        void imgEventDel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            Event ev = (Event)((Image)sender).Tag;
            ev.removeFromContrab();
            InitializeEventList();
            Mouse.OverrideCursor = Cursors.Arrow;

        }

    }
}
