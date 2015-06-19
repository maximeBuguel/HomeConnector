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

        public void InitialyseHomePanel() {
            this.Children.Clear();
            foreach (Room room in Rooms)
            {
                Label roomlabel = new Label();
                roomlabel.FontFamily = new FontFamily("Open Sans");
                roomlabel.FontWeight = FontWeights.Bold;
                roomlabel.Content = room.Name + " :";
                roomlabel.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                roomlabel.FontSize = 24;
                roomlabel.Foreground = Utils.getColor(Utils.white);
                this.Children.Add(roomlabel);
                this.Background = Utils.getColor(Utils.darkBlue); ;

                WrapPanel items = new WrapPanel();

                foreach (Receptor recp in room.receps)
                {

                    System.Drawing.Bitmap bmp = recp.getIcon();
                    Image img = new Image();
                    img.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(bmp.GetHbitmap(),
                        IntPtr.Zero,
                        System.Windows.Int32Rect.Empty,
                        BitmapSizeOptions.FromWidthAndHeight(100, 100));

                    StackPanel panel = new StackPanel();
                    panel.Height = 200;
                    img.Width = 80;
                    img.Height = 80;
                    img.Margin = new Thickness(0, 5, 0, 0);
                    panel.Width = 150;
                    panel.Height = 150;
                    panel.Background = Utils.getColor(Utils.lightBlue);
                    panel.Margin = new Thickness(10, 5, 0, 5);
                    Label name = new Label();
                    name.FontWeight = FontWeights.Bold;
                    name.FontFamily = new FontFamily("Open Sans");
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
                        System.Drawing.Bitmap bmpI = (System.Drawing.Bitmap)Properties.Resources.ResourceManager.GetObject("I"); ;
                        Image imgI = new Image();
                        imgI.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(bmpI.GetHbitmap(),
                            IntPtr.Zero,
                            System.Windows.Int32Rect.Empty,
                            BitmapSizeOptions.FromWidthAndHeight(100, 100));
                        imgI.Width = 24;
                        imgI.Height = 24;
                        imgI.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                        imgI.Tag = recp;
                        imgI.MouseLeftButtonDown += imgI_MouseLeftButtonDown;
                        btnPanel.Children.Add(imgI);
                        System.Drawing.Bitmap bmpO = (System.Drawing.Bitmap)Properties.Resources.ResourceManager.GetObject("O"); ;
                        Image imgO = new Image();
                        imgO.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(bmpO.GetHbitmap(),
                            IntPtr.Zero,
                            System.Windows.Int32Rect.Empty,
                            BitmapSizeOptions.FromWidthAndHeight(100, 100));
                        imgO.Width = 24;
                        imgO.Height = 24;
                        imgO.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                        imgO.Tag = recp;
                        imgO.MouseLeftButtonDown += imgO_MouseLeftButtonDown;
                        imgI.Margin = new Thickness(5, 0, 0, 0);
                        btnPanel.Children.Add(imgO);

                    }
                    else
                    {
                        System.Drawing.Bitmap bmpIO = (System.Drawing.Bitmap)Properties.Resources.ResourceManager.GetObject("IO"); ;
                        Image imgIO = new Image();
                        imgIO.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(bmpIO.GetHbitmap(),
                            IntPtr.Zero,
                            System.Windows.Int32Rect.Empty,
                            BitmapSizeOptions.FromWidthAndHeight(200, 100));
                        imgIO.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                        imgIO.Width = 48;
                        imgIO.Height = 24;
                        imgIO.Tag = recp;
                        imgIO.MouseLeftButtonDown += imgI_MouseLeftButtonDown;
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

        void imgO_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Receptor rep = (Receptor)((Image)sender).Tag;
            rep.sendCode2();
        }

        void imgI_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Receptor rep = (Receptor)((Image)sender).Tag;
            rep.sendCode1();
        }
    }
}
