using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
namespace AutomateMyHome
{   
    public class SpecialEventToSendEvAndScenario
    {
        public List<String> scenarList { get; set; }
        public Event ev { get; set; }

        /// <summary>
        ///  create an object to fill the tag of the icon linked to the creation of an event
        /// </summary>
        public SpecialEventToSendEvAndScenario(List<String> scenarList, Event ev)
        {
            this.scenarList = scenarList;
            this.ev = ev;
        }
    }
}
