using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomateMyHome
{
    /// <summary>
    /// This class is use to parse easily the list of receptors
    /// </summary
    class JSONReceptor
    {
        public String name { get; set; }
        public String doubleCommande { get; set; }
        public String commandeOn { get; set; }
        public String commandeOff { get; set; }
        public String type { get; set; }
        public String room { get; set; }

/// <summary>
/// Cast a Receptor into a JSONRECEPTOR
/// </summary>
/// <param name="r"> Recepector to copy</param>
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
