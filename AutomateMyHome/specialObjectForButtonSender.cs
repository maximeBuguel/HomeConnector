using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Renci.SshNet;
using System.Windows.Controls;

namespace AutomateMyHome
{
    public class specialObjectForButtonSender
    {
        public SshClient Client { get; set; }
        public List<ComboBox> Combolist { get; set; }
        public Event Ev { get; set; }

        /// <summary>
        ///  create an object to fill the tag of the icon linked to the modification of an existing event
        /// </summary>
        public specialObjectForButtonSender(SshClient client, List<ComboBox> combolist,Event ev)
        {   this.Ev = ev;
            this.Combolist= combolist;
            this.Client = client;
        }
    }
}
