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
    public class HomePanel : StackPanel
    {
        public List<Room> Rooms { get; set; }

        public HomePanel(List<Room> rooms)
        {
            this.Rooms = rooms;
            InitialyseHomePanel();
        }

        public void InitialyseHomePanel()
        {
            this.Children.Clear();
            foreach (Room room in Rooms)
            {
                Label roomlabel = new Label();
                roomlabel.FontFamily = Utils.appFont;
                roomlabel.FontWeight = Utils.weightFont;
                roomlabel.Content = room.Name + " :";
                roomlabel.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                roomlabel.FontSize = 24;
                roomlabel.Foreground = Utils.getColor(Utils.white);
                this.Children.Add(roomlabel);
                this.Background = Utils.getColor(Utils.darkBlue); ;

                WrapPanel items = new WrapPanel();

                foreach (Receptor recp in room.receps)
                {
                    Image img = new Image();
                    img.Source = Utils.getImageSource(recp.getIcon());

                    StackPanel panel = new StackPanel();
                    img.Width = 80;
                    img.Height = 80;
                    img.Margin = new Thickness(0, 5, 0, 0);
                    panel.Width = 150;
                    panel.Background = Utils.getColor(Utils.lightBlue);
                    panel.Margin = new Thickness(10, 5, 0, 5);
                    Label name = new Label();
                    name.FontWeight = Utils.weightFont;
                    name.FontFamily = Utils.appFont;
                    name.Foreground = Utils.getColor(Utils.white);
                    name.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                    panel.Children.Add(img);
                    name.Content = recp.Name;
                    name.FontSize = 18;
                    panel.Children.Add(name);
                    WrapPanel btnPanel = new WrapPanel();
                    btnPanel.Height = 25;
                    btnPanel.Cursor = Cursors.Hand;
                    if (recp.twoFrequencies)
                    {
                        Button imgI = new Button();
                        Image imgIb = new Image();
                        imgIb.Source = Utils.getImageSource(Properties.Resources.I);
                        //imgIb.Width =24 ;
                        //imgIb.Height = 24;
                        imgI.Content = imgIb;
                        imgI.Background = Utils.getColor(Utils.green);
                        imgI.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                        imgI.Tag = recp;
                        imgI.Click += imgI_Click;
                        btnPanel.Children.Add(imgI);
                        Button imgO = new Button();
                        Image imgOb = new Image();
                        imgOb.Source = Utils.getImageSource(Properties.Resources.O);
                        //imgOb.Width = 24;
                        //imgOb.Height = 24;
                        imgOb.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                        imgOb.Tag = recp;
                        imgO.Click += imgO_Click;
                        imgO.Content = imgOb;
                        imgO.Tag = recp;
                        imgO.Background = Utils.getColor(Utils.red);
                        imgI.Margin = new Thickness(5, 0, 0, 0);
                        btnPanel.Children.Add(imgO);

                    }
                    else
                    {
                        Button imgIO = new Button();
                        Image imgIOb = new Image();
                        imgIOb.Source = Utils.getImageSource(Properties.Resources.IO);
                        imgIOb.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                        imgIOb.Width = 48;
                        imgIOb.Height = 24;
                        imgIO.Tag = recp;
                        imgIO.Click += imgI_Click;
                        imgIO.Content = imgIOb;

                        imgIO.Background = Utils.getColor(Utils.purple);
                        btnPanel.Children.Add(imgIO);

                    }
                    btnPanel.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                    btnPanel.Margin = new Thickness(0, 0, 0, 10);
                    panel.Children.Add(btnPanel);
                    items.Children.Add(panel);
                }
                this.Children.Add(items);

            }
        }

        void imgO_Click(object sender, RoutedEventArgs e)
        {
            Receptor rep = (Receptor)((Button)sender).Tag;
            rep.sendCode2();
        }

        void imgI_Click(object sender, RoutedEventArgs e)
        {
            Receptor rep = (Receptor)((Button)sender).Tag;
            rep.sendCode1();
        }

        void imgI_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Receptor rep = (Receptor)((Image)sender).Tag;
            rep.sendCode1();
        }
    }
}
