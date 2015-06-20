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

        public SpecialEventToSendEvAndScenario(List<String> scenarList, Event ev)
        {
            this.scenarList = scenarList;
            this.ev = ev;
        }
    }
}
