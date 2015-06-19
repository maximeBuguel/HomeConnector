using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomateMyHome
{
    class JSONReceptor
    {
        public String name { get; set; }
        public String doubleCommande { get; set; }
        public String commandeOn { get; set; }
        public String commandeOff { get; set; }
        public String type { get; set; }
        public String room { get; set; }

        public JSONReceptor(Receptor r)
        {
            this.name = r.Name;
            this.doubleCommande = r.twoFrequencies.ToString();
            this.commandeOn = r.commandeOn;
            this.commandeOff = r.commandeOff;
            this.type = r.Type;
            this.room = r.Room.Name;
        }

        public JSONReceptor()
        {

        }
    }
}
