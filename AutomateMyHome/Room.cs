using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutomateMyHome
{
    /// <summary>
    /// Class representing a Room
    /// </summary>
    public class Room
    {
        public String Name { get; set; }

        public List<Receptor> receps { get; set; }

        /// <summary>
        /// Room constructor
        /// </summary>
        /// <param name="name"> name of the room</param>
        public Room(String name)
        {
            this.Name = name;
            this.receps = new List<Receptor>();
        }

        /// <summary>
        /// Add a receptor to the list of receptors of the room
        /// </summary>
        /// <param name="r"> Receptor to add</param>
        public void addReceptor(Receptor r) {
            this.receps.Add(r);
        }


        /// <summary>
        /// check if a room already exist in a list
        /// </summary>
        /// <param name="r"> name of the room </param>
        /// <param name="rooms"> list of rooms </param>
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
