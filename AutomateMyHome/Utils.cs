using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace AutomateMyHome
{
    class Utils
    {
        public const String lightBlue = "#41B1E1";
        public const String darkBlue = "#2F4F4F";
        public const String white = "#FFFFFF";
        public const String green = "#00A11A";
        public const String red = "#BD1D49";
        public const String red = "#5E3AB8";
        
        public static Brush getColor(String s) {
            BrushConverter bc = new BrushConverter();
            Brush brush = (Brush)bc.ConvertFrom(s);
            return brush;
        }

        
    }
}
