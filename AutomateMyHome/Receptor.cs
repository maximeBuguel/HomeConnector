using Newtonsoft.Json;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomateMyHome
{
    public class Receptor
    {
        public String Name { get; set; }
        public Room Room { get; set; }
        public Boolean twoFrequencies { get; set; }
        public String commandeOn { get; set; }
        public String commandeOff { get; set; }
        public SshClient client { get; set; }
        public String Type { get; set; }


        public Receptor(String name, Room room, Boolean twoFreq, String cd1, String cd2, SshClient c, String type)
        {
            this.Name = name;
            this.Room = room;
            this.twoFrequencies = twoFreq;
            this.commandeOn = cd1;
            this.commandeOff = cd2;
            this.client = c;
            this.Type = type;
        }

        public void sendCode1()
        {
            client.RunCommand("sudo ./HomeConnector/codesend " + commandeOn);
        }

        public void sendCode2()
        {
            if (twoFrequencies)
            {
                client.RunCommand("sudo ./HomeConnector/codesend " + commandeOff);
            }
            else
            {
                this.sendCode1();
            }
        }

        public static List<Receptor> getReceptors(SshClient c, List<Room> rooms) {
            List<Receptor> receptors = new List<Receptor>();
            SshCommand cmd = c.RunCommand("cat HomeConnector/Receptors.json ");
            List<JSONReceptor> lR = JsonConvert.DeserializeObject<List<JSONReceptor>>(cmd.Result);
            Dictionary<String, String>[] rcptrs = JsonConvert.DeserializeObject<Dictionary<String,String>[]>(cmd.Result);
            int size =  rcptrs.Length;
            for (int i = 0; i < size; i++)
            {
                String r = rcptrs[i]["room"];
                Room room = Room.exist(r, rooms);
                if(room == null){
                    room = new Room(r);
                    rooms.Add(room);
                }
                Boolean b = Convert.ToBoolean( rcptrs[i]["doubleCommande"]);
                Receptor newRecp = new Receptor(rcptrs[i]["name"],room, b, rcptrs[i]["commandeOn"], rcptrs[i]["commandeOff"],c, rcptrs[i]["type"]);
                receptors.Add(newRecp);
                room.addReceptor(newRecp);
            }




                return receptors;
        
        }

        public System.Drawing.Bitmap getIcon()
        {
            if (this.Type == "Light")
            {
                System.Drawing.Bitmap bmp = (System.Drawing.Bitmap)Properties.Resources.ResourceManager.GetObject("lamp");
                return bmp;
            }
            else {
                System.Drawing.Bitmap bmp = (System.Drawing.Bitmap)Properties.Resources.ResourceManager.GetObject("plug");
                return bmp;
            }
        }

      


        public static void refresh(SshClient c ,List<Receptor> receps)
        {
            List<JSONReceptor> JSONRecep = new List<JSONReceptor>();
            foreach(Receptor r in receps){
                JSONRecep.Add(new JSONReceptor(r));
            }
            String output = JsonConvert.SerializeObject(JSONRecep);
            c.RunCommand("echo \"" + output.Replace("\"","\\\"") + "\" > HomeConnector/Receptors.json");
        }
    }

}

