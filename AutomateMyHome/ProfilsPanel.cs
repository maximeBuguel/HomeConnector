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
    /// <summary>
    /// Panel representing a list of scenario
    /// </summary>
    public class ProfilsPanel : StackPanel
    {
        public SshClient client { get; set; }
        public List<Receptor> Receptors { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="c">SshClient Connection with the box</param>
        /// <param name="r">List of Receptors</param>
        public ProfilsPanel(SshClient c, List<Receptor> r)
        {
            this.client = c;
            this.Receptors = r;
            InitialyzeProfils();
        }

        /// <summary>
        /// Get the informations about scenarios on the box, Then refesh the view 
        /// </summary>
        public void InitialyzeProfils()
        {
            this.Children.Clear();
            List<String> scsNames = Scenario.getScenariosNames(client);
            this.Width = this.Width - 110;
            this.Background = Utils.getColor(Utils.darkBlue);

            foreach (String sc in scsNames)
            {

                DockPanel dp = new DockPanel();
                dp.Background = Utils.getColor(Utils.lightBlue);
                dp.Margin = new Thickness(10, 10, 10, 0);
                Image imgStart = new Image();
                imgStart.Source = Utils.getImageSource(Properties.Resources.start);
                imgStart.Width = 24;
                imgStart.Height = 48;
                imgStart.Margin = new Thickness(5, 5, 5, 5);
                imgStart.Cursor = Cursors.Hand;
                imgStart.Tag = sc;
                imgStart.MouseLeftButtonDown += imgStart_MouseLeftButtonDown;

                Label name = new Label();
                name.Content = sc.Replace("-", " ");
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
                imgDel.Tag = sc;
                imgDel.MouseLeftButtonDown += imgDel_MouseLeftButtonDown;

                Image imgSetting = new Image();
                imgSetting.Source = Utils.getImageSource(Properties.Resources.settings);
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
            Button btnPlus = new Button();
            Image imgPlus = new Image();
            imgPlus.Source = Utils.getImageSource(Properties.Resources.Plus);
            imgPlus.Width = 32;
            imgPlus.Height = 32;
            imgPlus.Margin = new Thickness(10, 10, 10, 10);
            imgPlus.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            add.Background = Utils.getColor(Utils.green);
            btnPlus.Content = imgPlus;
            btnPlus.Background = Utils.getColor(Utils.green);
            btnPlus.Click += btnPlus_Click;
            add.Children.Add(btnPlus);
            this.Children.Add(add);
        }


        /// <summary>
        /// btnPlus on click function, launch Scenario Editor with a empty scenario 
        /// </summary>
        void btnPlus_Click(object sender, RoutedEventArgs e)
        {
            try
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
            catch
            {
                //Event client_ErrorOccurred
            }
        }

        /// <summary>
        /// imgSetting on click function, launch Scenario Editor with an existing scenario 
        /// </summary>
        void imgSetting_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                ScenarioEditor sce = new ScenarioEditor(client, new Scenario((String)((Image)sender).Tag, Scenario.getCodes(client, (String)((Image)sender).Tag + ".sh")));
                sce.ShowDialog();
                if ((bool)sce.DialogResult)
                {
                    Mouse.OverrideCursor = Cursors.Wait;
                    this.client.RunCommand("rm HomeConnector/profils/" + (String)((Image)sender).Tag + ".sh");
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
            catch
            {
                //Event client_ErrorOccurred
            }

        }

        /// <summary>
        /// imgDel on click function, remove an existing scenario 
        /// </summary>
        void imgDel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;

            Event.deleteAllEventWhoContainsThatScenario(((String)((Image)sender).Tag), client);
            this.client.RunCommand("rm HomeConnector/profils/" + (String)((Image)sender).Tag + ".sh");
            InitialyzeProfils();
            InitialyzeProfils();
            Mouse.OverrideCursor = Cursors.Arrow;

        }

        /// <summary>
        /// imgStart on click function, launch an existing scenario 
        /// </summary>
        void imgStart_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Scenario s = new Scenario((String)((Image)sender).Tag, null);
            s.launch(client);
        }

    }
}
