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
        public SettingPanel (SshClient c)
	{
            this.client = c;
            
           
	}

        public void InitialyzeSettingPanel()
        {
            this.Children.Clear();
            BrushConverter bc = new BrushConverter();
            StackPanel panel = new StackPanel();
            panel.Background = (Brush)bc.ConvertFrom("#41B1E1");
            panel.Margin = new Thickness(10, 10, 10, 10);
            Label pwdChgLbl = new Label();
            pwdChgLbl.FontWeight = FontWeights.Bold;
            pwdChgLbl.FontFamily = new FontFamily("Open Sans");
            pwdChgLbl.Foreground = (Brush)bc.ConvertFrom("#FFFFFF");
            pwdChgLbl.FontSize = 34;
            pwdChgLbl.Margin = new Thickness(5, 5, 5, 5);
            pwdChgLbl.Content = "Change password";
            panel.Children.Add(pwdChgLbl);
            Label resultLbl = new Label();
            WrapPanel newPwdPnl1 = new WrapPanel();
            newPwdPnl1.Margin = new Thickness(5, 5, 5, 5);
            Label newPwdLbl1 = new Label();
            newPwdLbl1.FontWeight = FontWeights.Bold;
            newPwdLbl1.FontFamily = new FontFamily("Open Sans");
            newPwdLbl1.Foreground = (Brush)bc.ConvertFrom("#FFFFFF");
            newPwdLbl1.Content = "New password : ";
            PasswordBox newPwdBox1 = new PasswordBox();
            newPwdBox1.Width = 250;
            newPwdPnl1.Children.Add(newPwdLbl1);
            newPwdPnl1.Children.Add(newPwdBox1);
            panel.Children.Add(newPwdPnl1);
            WrapPanel newPwdPnl2 = new WrapPanel();
            newPwdPnl2.Margin = new Thickness(5, 5, 5, 5);
            Label newPwdLbl2 = new Label();
            newPwdLbl2.FontWeight = FontWeights.Bold;
            newPwdLbl2.FontFamily = new FontFamily("Open Sans");
            newPwdLbl2.Foreground = (Brush)bc.ConvertFrom("#FFFFFF");
            newPwdLbl2.Content = "New password : ";
            PasswordBox newPwdBox2 = new PasswordBox();
            newPwdBox2.Width = 250;
            newPwdPnl2.Children.Add(newPwdLbl2);
            newPwdPnl2.Children.Add(newPwdBox2);
            panel.Children.Add(newPwdPnl2);
            Button changePwdBtn = new Button();
            changePwdBtn.Margin = new Thickness(5, 5, 5, 5);
            changePwdBtn.Width = 150;
            changePwdBtn.Content = "Change password";
            Object[] pwdChanges = new Object[] { newPwdBox1, newPwdBox2, resultLbl };
            changePwdBtn.Tag = pwdChanges;
            changePwdBtn.Click += changePwdBtn_Click;
            panel.Children.Add(changePwdBtn);
            panel.Children.Add(resultLbl);
            this.Children.Add(panel);

            DockPanel addPanel = new DockPanel();
            addPanel.Margin = new Thickness(10, 10, 10, 10);
            addPanel.Background = (Brush)bc.ConvertFrom("#41B1E1");
            Label addLbl = new Label();
            WrapPanel centeredPanel = new WrapPanel();
            addLbl.FontWeight = FontWeights.Bold;
            addLbl.FontFamily = new FontFamily("Open Sans");
            addLbl.Foreground = (Brush)bc.ConvertFrom("#FFFFFF");
            addLbl.FontSize = 34;
            addLbl.Margin = new Thickness(5, 5, 5, 5);
            addLbl.Content = "Add receptor";
            DockPanel.SetDock(addLbl, Dock.Top);
            addPanel.Children.Add(addLbl);
            System.Drawing.Bitmap bmpLight = (System.Drawing.Bitmap)Properties.Resources.ResourceManager.GetObject("lampPlus");
            Image imgLight = new Image();
            imgLight.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(bmpLight.GetHbitmap(),
                IntPtr.Zero,
                System.Windows.Int32Rect.Empty,
                BitmapSizeOptions.FromWidthAndHeight(100, 100));
            
            imgLight.MouseLeftButtonDown += addLight_MouseLeftButtonDown;

            imgLight.Width = 128;
            imgLight.Height = 128;
            imgLight.Margin = new Thickness(10, 10, 10, 10);
            imgLight.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            centeredPanel.Children.Add(imgLight);

            System.Drawing.Bitmap bmpPlug = (System.Drawing.Bitmap)Properties.Resources.ResourceManager.GetObject("plugPlus");
            Image imgPlug = new Image();
            imgPlug.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(bmpPlug.GetHbitmap(),
                IntPtr.Zero,
                System.Windows.Int32Rect.Empty,
                BitmapSizeOptions.FromWidthAndHeight(100, 100));
            imgPlug.MouseLeftButtonDown += addPlug_MouseLeftButtonDown;

            imgPlug.Width = 128;
            imgPlug.Height = 128;
            imgPlug.Margin = new Thickness(10, 10, 10, 10);
            imgPlug.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            imgPlug.Margin = new Thickness(150, 0, 0, 0);
            centeredPanel.Children.Add(imgPlug);
            centeredPanel.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            centeredPanel.VerticalAlignment = System.Windows.VerticalAlignment.Center;

            DockPanel.SetDock(centeredPanel, Dock.Bottom);
            addPanel.Children.Add(centeredPanel);


            DockPanel removePanel = new DockPanel();
            removePanel.Margin = new Thickness(10, 10, 10, 10);
            removePanel.Background = (Brush)bc.ConvertFrom("#41B1E1");

            Label rmvLabel = new Label();
            rmvLabel.FontFamily = new FontFamily("Open Sans");
            rmvLabel.FontWeight = FontWeights.Bold;
            rmvLabel.Foreground = (Brush)bc.ConvertFrom("#FFFFFF");
            rmvLabel.Background = (Brush)bc.ConvertFrom("#41B1E1");
            rmvLabel.FontSize = 34;
            rmvLabel.Content = "Remove receptor :";
            List<ComboBoxItem> cbL = new List<ComboBoxItem>();
            //ComboBoxItem cbi1 = new ComboBoxItem();
            List<Receptor> receptors = Receptor.getReceptors(client, new List<Room>());
            foreach (Receptor r in receptors)
            {
                ComboBoxItem cbi = new ComboBoxItem();
                cbi.FontFamily = new FontFamily("Open Sans");
                cbi.FontSize = 34;
                cbi.Content = r.Name + " in " +r.Room.Name;
                cbL.Add(cbi);
            }
            ComboBox cb = new ComboBox();
            cb.ItemsSource = cbL;
            cb.Margin = new Thickness(0, 10, 10, 10);
            cb.Width = 350;
            System.Drawing.Bitmap bmpDel = (System.Drawing.Bitmap)Properties.Resources.ResourceManager.GetObject("delete"); ;
            Image imgDel = new Image();
            imgDel.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(bmpDel.GetHbitmap(),
                IntPtr.Zero,
                System.Windows.Int32Rect.Empty,
                BitmapSizeOptions.FromWidthAndHeight(100, 100));
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
            removePanel.Children.Add(wp);
            this.Children.Add(addPanel);
            this.Children.Add(removePanel);
        }

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

        private void addPlug_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ReceptorAdder ra = new ReceptorAdder(client,"Plug");
            ra.ShowDialog();
        }

        private void addLight_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ReceptorAdder ra = new ReceptorAdder(client, "Light");
            ra.ShowDialog();
        }

        void changePwdBtn_Click(object sender, RoutedEventArgs e)
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
                    returnLbl.FontWeight = FontWeights.Bold;
                }
                else
                {
                    BrushConverter bc = new BrushConverter();
                    returnLbl.Foreground = (Brush)bc.ConvertFrom("#B64741");
                    returnLbl.Content = "Could not change the password";
                    returnLbl.FontWeight = FontWeights.Bold;
                }
                pwd1.Password = "";
                pwd2.Password = "";
            }
            else
            {
                BrushConverter bc = new BrushConverter();
                returnLbl.Foreground = (Brush)bc.ConvertFrom("#B64741");
                returnLbl.Content = "Could not change the password";
                returnLbl.FontWeight = FontWeights.Bold;
            }

        }
    }
}
