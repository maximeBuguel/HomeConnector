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
            List<Scenario> senarios = Scenario.getScenarios(this.client);
            foreach (Event ev in evList)
            {

                DockPanel dp = new DockPanel();
                dp.Background = Utils.getColor(Utils.lightBlue);
                dp.Margin = new Thickness(10, 10, 10, 0);


                Label name = new Label();
                name.FontWeight = FontWeights.Bold;
                name.FontFamily = new FontFamily("Open Sans");
                name.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                name.FontSize = 24;
                name.Foreground = Utils.getColor(Utils.white);

                System.Drawing.Bitmap bmpDel = (System.Drawing.Bitmap)Properties.Resources.ResourceManager.GetObject("delete"); ;
                Image imgDel = new Image();
                imgDel.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(bmpDel.GetHbitmap(),
                    IntPtr.Zero,
                    System.Windows.Int32Rect.Empty,
                    BitmapSizeOptions.FromWidthAndHeight(100, 100));
                imgDel.Width = 24;
                imgDel.Height = 24;
                imgDel.Margin = new Thickness(5, 0, 5, 0);
                imgDel.Cursor = Cursors.Hand;
                imgDel.Tag = ev;
                imgDel.MouseLeftButtonDown += imgEventDel_MouseLeftButtonDown;
                System.Drawing.Bitmap bmpSetting = (System.Drawing.Bitmap)Properties.Resources.ResourceManager.GetObject("settings"); ;
                Image imgSetting = new Image();
                imgSetting.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(bmpSetting.GetHbitmap(),
                    IntPtr.Zero,
                    System.Windows.Int32Rect.Empty,
                    BitmapSizeOptions.FromWidthAndHeight(100, 100));
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
            System.Drawing.Bitmap bmpPlus = (System.Drawing.Bitmap)Properties.Resources.ResourceManager.GetObject("Plus"); ;
            Image imgPlus = new Image();
            imgPlus.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(bmpPlus.GetHbitmap(),
                IntPtr.Zero,
                System.Windows.Int32Rect.Empty,
                BitmapSizeOptions.FromWidthAndHeight(100, 100));
            imgPlus.Width = 32;
            imgPlus.Height = 32;
            imgPlus.Margin = new Thickness(10, 10, 10, 10);
            imgPlus.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            add.Tag = senarios;
            add.MouseLeftButtonDown += imgPlus_MouseLeftDown;
            add.Background = Utils.getColor(Utils.green); ;
            add.Children.Add(imgPlus);
            this.Children.Add(add);

        }

        private void imgPlus_MouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            List<Scenario> se = (List<Scenario>)((DockPanel)sender).Tag;
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
