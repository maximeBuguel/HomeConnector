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
        public List<Scenario> scenarList { get; set; }
        public Event ev { get; set; }

        public SpecialEventToSendEvAndScenario(List<Scenario> scenarList,Event ev)
        {
            this.scenarList = scenarList;
            this.ev = ev;
        }
    }
}
