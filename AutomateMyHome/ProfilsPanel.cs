using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AutomateMyHome
{
    public class ProfilsPanel : StackPanel
    {
        public SshClient client { get; set; }
        public List<Receptor> Receptors { get; set; }
        public ProfilsPanel(SshClient c, List<Receptor> r)
        {
            this.client = c;
            this.Receptors = r;
            InitialyzeProfils();
        }
        public void InitialyzeProfils()
        {
            this.Children.Clear();
            List<Scenario> scs = Scenario.getScenarios(client);
            this.Width = this.Width - 110;
            this.Background = Utils.getColor(Utils.darkBlue);
            foreach (Scenario sc in scs)
            {

                DockPanel dp = new DockPanel();
                dp.Background = Utils.getColor(Utils.lightBlue);
                dp.Margin = new Thickness(10, 10, 10, 0);

                System.Drawing.Bitmap bmpStart = (System.Drawing.Bitmap)Properties.Resources.ResourceManager.GetObject("start"); ;
                Image imgStart = new Image();
                imgStart.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(bmpStart.GetHbitmap(),
                    IntPtr.Zero,
                    System.Windows.Int32Rect.Empty,
                    BitmapSizeOptions.FromWidthAndHeight(100, 100));
                imgStart.Width = 24;
                imgStart.Height = 48;
                imgStart.Margin = new Thickness(5, 5, 5, 5);
                imgStart.Cursor = Cursors.Hand;
                imgStart.Tag = sc;
                imgStart.MouseLeftButtonDown += imgStart_MouseLeftButtonDown;

                Label name = new Label();
                name.Content = sc.name.Replace("-", " ");
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
                imgDel.Tag = sc;
                imgDel.MouseLeftButtonDown += imgDel_MouseLeftButtonDown;


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
                imgSetting.Tag = sc;
                imgSetting.MouseLeftButtonDown += imgSetting_MouseLeftButtonDown;

                WrapPanel dbutt = new WrapPanel();
                dbutt.Children.Add(imgSetting);
                dbutt.Children.Add(imgDel);
                dbutt.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                dbutt.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                //DockPanel.SetDock(imgStart, Dock.Left);
                dp.Children.Add(imgStart);
                dp.Children.Add(name);

                DockPanel.SetDock(dbutt, Dock.Right);
                dp.Children.Add(dbutt);

                // DockPanel.SetDock(imgSetting, Dock.Right);

                // dp.Children.Add(imgSetting);


                //dp.Children.Add(dbutt);

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
            add.MouseLeftButtonDown += add_MouseLeftButtonDown;

            imgPlus.Width = 32;
            imgPlus.Height = 32;
            imgPlus.Margin = new Thickness(10, 10, 10, 10);
            imgPlus.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            add.Background = Utils.getColor(Utils.green);
            add.Children.Add(imgPlus);
            this.Children.Add(add);
        }

        void add_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Scenario sc = new Scenario();
            sc.initName(client);
            ScenarioEditor sce = new ScenarioEditor(client, sc);
            sce.ShowDialog();
            if ((bool)sce.DialogResult)
            {
                Mouse.OverrideCursor = Cursors.Wait;
                this.client.RunCommand("touch HomeConnector/profils/" + ((Scenario)(sce.Tag)).name + ".sh");

                foreach (string s in ((Scenario)(sce.Tag)).codes)
                {
                    this.client.RunCommand("echo \"sudo ./HomeConnector/codesend " + s + "\" | tee -a HomeConnector/profils/" + ((Scenario)(sce.Tag)).name + ".sh");
                }

                client.RunCommand("chmod +x HomeConnector/profils/" + ((Scenario)(sce.Tag)).name + ".sh");
                InitialyzeProfils();
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        void imgSetting_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ScenarioEditor sce = new ScenarioEditor(client, (Scenario)((Image)sender).Tag);
            sce.ShowDialog();
            if ((bool)sce.DialogResult)
            {
                Mouse.OverrideCursor = Cursors.Wait;
                this.client.RunCommand("rm HomeConnector/profils/" + ((Scenario)((Image)sender).Tag).name + ".sh");
                this.client.RunCommand("touch HomeConnector/profils/" + ((Scenario)(sce.Tag)).name + ".sh");

                foreach (string s in ((Scenario)(sce.Tag)).codes)
                {
                    this.client.RunCommand("echo \"sudo ./HomeConnector/codesend " + s + "\" | tee -a HomeConnector/profils/" + ((Scenario)(sce.Tag)).name + ".sh");
                }

                client.RunCommand("chmod +x HomeConnector/profils/" + ((Scenario)(sce.Tag)).name + ".sh");
                InitialyzeProfils();
                Mouse.OverrideCursor = Cursors.Arrow;
            }

        }

        void imgDel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;

            Event.deleteAllEventWhoContainsThatScenario(((Scenario)((Image)sender).Tag).name,client);
            this.client.RunCommand("rm HomeConnector/profils/" + ((Scenario)((Image)sender).Tag).name + ".sh");
            InitialyzeProfils();
            InitialyzeProfils();
            Mouse.OverrideCursor = Cursors.Arrow;

        }

        void imgStart_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Scenario s = (Scenario)((Image)sender).Tag;
            s.launch(client);
        }

    }
}
