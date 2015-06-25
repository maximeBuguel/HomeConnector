using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AutomateMyHome
{
    public class SettingPanel : StackPanel
    {
    public SshClient client { get; set; }
    /// <summary>
    /// create the setting panel
    /// </summary>
    public SettingPanel (SshClient c)
	{
            this.client = c;
            
           
	}
    /// <summary>
    /// Send the code 1 of a receptor (often the turn On)
    /// </summary>
        public void InitialyzeSettingPanel()
        {
            this.Children.Clear();
            StackPanel panel = new StackPanel();
            panel.Background = Utils.getColor(Utils.lightBlue);
            panel.Margin = new Thickness(10, 10, 10, 10);
            Label pwdChgLbl = new Label();
            pwdChgLbl.FontWeight = Utils.weightFont;
            pwdChgLbl.FontFamily = Utils.appFont;
            pwdChgLbl.Foreground = Utils.getColor(Utils.white);
            pwdChgLbl.FontSize = 34;
            pwdChgLbl.Margin = new Thickness(5, 5, 5, 5);
            pwdChgLbl.Content = "Change password";
            panel.Children.Add(pwdChgLbl);
            Label resultLbl = new Label();
            WrapPanel newPwdPnl1 = new WrapPanel();
            newPwdPnl1.Margin = new Thickness(5, 5, 5, 5);
            Label newPwdLbl1 = new Label();
            newPwdLbl1.FontWeight = Utils.weightFont;
            newPwdLbl1.FontFamily = Utils.appFont;
            newPwdLbl1.Foreground = Utils.getColor(Utils.white);
            newPwdLbl1.Content = "New password : ";
            newPwdLbl1.FontSize = 24;
            PasswordBox newPwdBox1 = new PasswordBox();
            newPwdBox1.Width = 250;
            newPwdPnl1.Children.Add(newPwdLbl1);
            newPwdPnl1.Children.Add(newPwdBox1);
            newPwdBox1.FontSize = 24;

            newPwdBox1.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            newPwdBox1.FontFamily = Utils.appFont;
            newPwdBox1.FontWeight = Utils.weightFont;
            panel.Children.Add(newPwdPnl1);
            WrapPanel newPwdPnl2 = new WrapPanel();
            newPwdPnl2.Margin = new Thickness(5, 5, 5, 5);
            Label newPwdLbl2 = new Label();
            newPwdLbl2.FontWeight = Utils.weightFont;
            newPwdLbl2.FontFamily = Utils.appFont;
            newPwdLbl2.Foreground = Utils.getColor(Utils.white);
            newPwdLbl2.Content = "New password : ";
            newPwdLbl2.FontSize = 24;
            PasswordBox newPwdBox2 = new PasswordBox();
            newPwdBox2.Width = 250;
            newPwdBox2.FontSize = 24;
            newPwdPnl2.Children.Add(newPwdLbl2);
            newPwdPnl2.Children.Add(newPwdBox2);
            newPwdBox2.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            newPwdBox2.FontFamily = Utils.appFont;
            newPwdBox2.FontWeight = Utils.weightFont;
            panel.Children.Add(newPwdPnl2);
            Button changePwdBtn = new Button();
            changePwdBtn.Margin = new Thickness(5, 5, 5, 5);
            changePwdBtn.Width = 150;
            changePwdBtn.Content = "Change";
            changePwdBtn.FontFamily = Utils.appFont;
            changePwdBtn.FontWeight = Utils.weightFont;
            changePwdBtn.FontSize = 24;
            Object[] pwdChanges = new Object[] { newPwdBox1, newPwdBox2, resultLbl };
            changePwdBtn.Tag = pwdChanges;
            changePwdBtn.Click += changePwdBtn_Click;
            panel.Children.Add(changePwdBtn);
            panel.Children.Add(resultLbl);
            this.Children.Add(panel);

            DockPanel addPanel = new DockPanel();
            addPanel.Margin = new Thickness(10, 10, 10, 10);
            addPanel.Background = Utils.getColor(Utils.lightBlue);
            Label addLbl = new Label();
            WrapPanel centeredPanel = new WrapPanel();
            addLbl.FontWeight = Utils.weightFont;
            addLbl.FontFamily = Utils.appFont;
            addLbl.Foreground = Utils.getColor(Utils.white);
            addLbl.FontSize = 34;
            addLbl.Margin = new Thickness(5, 5, 5, 5);
            addLbl.Content = "Add receptor";
            DockPanel.SetDock(addLbl, Dock.Top);
            addPanel.Children.Add(addLbl);
            Image imgLight = new Image();
            imgLight.Source = Utils.getImageSource(Properties.Resources.lampPlus);
            
            imgLight.MouseLeftButtonDown += addLight_MouseLeftButtonDown;

            imgLight.Width = 128;
            imgLight.Height = 128;
            imgLight.Margin = new Thickness(10, 10, 10, 10);
            imgLight.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            centeredPanel.Children.Add(imgLight);
            Image imgPlug = new Image();
            imgPlug.Source = Utils.getImageSource(Properties.Resources.plugPlus);
            imgPlug.MouseLeftButtonDown += addPlug_MouseLeftButtonDown;

            imgPlug.Width = 128;
            imgPlug.Height = 128;
            imgPlug.Margin = new Thickness(10, 10, 10, 10);
            imgPlug.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            centeredPanel.Children.Add(imgPlug);
            centeredPanel.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            centeredPanel.VerticalAlignment = System.Windows.VerticalAlignment.Center;

            DockPanel.SetDock(centeredPanel, Dock.Bottom);
            addPanel.Children.Add(centeredPanel);


            WrapPanel removePanel = new WrapPanel();
            removePanel.Margin = new Thickness(10, 10, 10, 10);
            removePanel.Background = Utils.getColor(Utils.lightBlue);

            Label rmvLabel = new Label();
            rmvLabel.FontFamily = Utils.appFont;
            rmvLabel.FontWeight = Utils.weightFont;
            rmvLabel.Foreground = Utils.getColor(Utils.white);
            rmvLabel.Background = Utils.getColor(Utils.lightBlue);
            rmvLabel.FontSize = 34;
            rmvLabel.Content = "Remove receptor :";
            List<ComboBoxItem> cbL = new List<ComboBoxItem>();
            //ComboBoxItem cbi1 = new ComboBoxItem();
            List<Receptor> receptors = Receptor.getReceptors(client, new List<Room>());
            foreach (Receptor r in receptors)
            {
                ComboBoxItem cbi = new ComboBoxItem();
                cbi.FontFamily = Utils.appFont;
                cbi.FontSize = 34;
                cbi.Content = r.Name + " in " +r.Room.Name;
                cbL.Add(cbi);
            }
            ComboBox cb = new ComboBox();
            cb.ItemsSource = cbL;
            cb.Margin = new Thickness(10, 10, 10, 10);
            cb.Width = 350;
            Image imgDel = new Image();
            imgDel.Source = Utils.getImageSource(Properties.Resources.delete);
            imgDel.Width = 24;
            imgDel.Height = 24;
            imgDel.Margin = new Thickness(5, 0, 5, 0);
            imgDel.Tag = cb;
            imgDel.MouseLeftButtonDown += imgDel_MouseLeftButtonDown;

            WrapPanel wp = new WrapPanel();
            wp.Margin = new Thickness(10, 10, 10, 10);
            removePanel.Children.Add(rmvLabel);
            removePanel.Children.Add(cb);
            wp.Children.Add(imgDel);
            wp.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            removePanel.Children.Add(wp);
            this.Children.Add(addPanel);
            this.Children.Add(removePanel);
        }

        /// <summary>
        /// Remove the prise currently selected in the comboBox
        /// </summary>
        void imgDel_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ComboBox cb = (ComboBox)((Image)sender).Tag;
            List<Receptor> receptors = Receptor.getReceptors(client, new List<Room>());
            int index = cb.SelectedIndex;
            Receptor r = receptors.ElementAt(index);
            Scenario.removeReceptorFromScenarios(client, r);
            receptors.RemoveAt(index);
            Receptor.refresh(client, receptors);
            this.InitialyzeSettingPanel();
        }

        /// <summary>
        /// Show a new window to add a new plug 
        /// </summary>
        private void addPlug_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ReceptorAdder ra = new ReceptorAdder(client,"Plug");
            ra.ShowDialog();
            if ((bool)ra.DialogResult)
            {
                InitialyzeSettingPanel();
            }
        }

        /// <summary>
        /// Show a new window to add a new Light 
        /// </summary>
        private void addLight_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ReceptorAdder ra = new ReceptorAdder(client, "Light");
            ra.ShowDialog();
            if ((bool)ra.DialogResult)
            {               
                InitialyzeSettingPanel();
            }
        }

        /// <summary>
        /// Change the password of the box if the two passwordBox have the same content
        /// </summary>
        void changePwdBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Object[] pwds = (Object[])((Button)sender).Tag;
                PasswordBox pwd1 = (PasswordBox)pwds[0];
                PasswordBox pwd2 = (PasswordBox)pwds[1];
                Label returnLbl = (Label)pwds[2];
                if (pwd1.Password == pwd2.Password && pwd1.Password != "")
                {
                    String cmdSt = "echo \"pi:" + pwd1.Password + "\" | sudo chpasswd ";
                    SshCommand cmd = client.RunCommand(cmdSt);
                    pwd1.Password = "";
                    pwd2.Password = "";
                    if (cmd.Result == "")
                    {
                        BrushConverter bc = new BrushConverter();
                        returnLbl.Foreground = (Brush)bc.ConvertFrom("#00a11a"); ;
                        returnLbl.Content = "Password changed";
                        returnLbl.FontWeight = Utils.weightFont;
                    }
                    else
                    {
                        BrushConverter bc = new BrushConverter();
                        returnLbl.Foreground = (Brush)bc.ConvertFrom("#B64741");
                        returnLbl.Content = "Could not change the password";
                        returnLbl.FontWeight = Utils.weightFont;
                    }
                    pwd1.Password = "";
                    pwd2.Password = "";
                }
                else
                {
                    BrushConverter bc = new BrushConverter();
                    returnLbl.Foreground = (Brush)bc.ConvertFrom("#B64741");
                    returnLbl.Content = "Could not change the password";
                    returnLbl.FontWeight = Utils.weightFont;
                }
            }
            catch
            {
                //Event client_ErrorOccurred
            }

        }
    }
}
