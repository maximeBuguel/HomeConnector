using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutomateMyHome
{
    public class Room
    {
        public String Name { get; set; }

        public List<Receptor> receps { get; set; }

        public Room(String name)
        {
            this.Name = name;
            this.receps = new List<Receptor>();
        }

        public void addReceptor(Receptor r) {
            this.receps.Add(r);
        }

        

        public static Room exist(String r, List<Room> rooms) {
            Room exist = null;
            foreach(Room room in rooms){
                if(room.Name == r){
                    exist = room;
                }
            }
            return exist;

        }

    }
}
