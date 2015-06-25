using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Renci.SshNet;
using System.Collections;
using System.Diagnostics;
namespace AutomateMyHome

{
        public class Event
    {
        public String Minutes { get; set; }
        public String Hour { get; set; }
        public String Day { get; set; }
        public String Month { get; set; }
        public String DayOfTheWeek { get; set; }
        public String Scenario { get; set; }
        public bool isACreator { get; set; }
        
        public SshClient client { get; set; }
        private static String fileName = "EventScriptCrontab.txt";


        /// <summary>
        /// Event default constructor 
        /// </summary>

        public Event(){
            this.isACreator = true;
        }

        /// <summary>
        /// Event constructor for EventEditor
        /// </summary>
        public Event(SshClient client,ArrayList comboBoxContentList){
            this.isACreator = false;
            this.client = client;
            this.Scenario = (String)comboBoxContentList[0];
            this.Minutes = comboBoxContentList[1].ToString();
            this.Hour = comboBoxContentList[2].ToString();
            this.Day = comboBoxContentList[3].ToString();
            this.Month = comboBoxContentList[4].ToString();
            this.DayOfTheWeek = getNumberFromString(comboBoxContentList[5].ToString());
        }

        /// <summary>
        /// Event copy constructor 
        /// </summary>
        public Event(SshClient client, String minute, String hour,String day, String month, String dayOfTheWeek,String scenario){
            this.client = client;
            this.Scenario = scenario;
            this.Minutes = minute;
            this.Hour = hour;
            this.Day = day;
            this.Month = month;
            this.DayOfTheWeek = dayOfTheWeek;
            //this.client.RunCommand("crontab " + fileName);
        }


        /// <summary>
        /// Get the right name to display it
        /// </summary>
        public String getName()
        {
            String s = "";
            s += this.Scenario.Replace("-"," ") + " is executed";
            s = MinutePartOfName(s);
            s = DayPartOfName(s);
            return s;
        }
        /// <summary>
        /// Get the first half of the event name displayed 
        /// </summary>
        private string MinutePartOfName(String s)
        {
            if (this.Hour.Equals("*") && this.Minutes.Equals("*"))
            {
                s += " every minute";
            }
            else
            {
                if (this.Hour.Equals("*"))
                {
                    s += " every hour";
                    if (this.Minutes.Equals("*"))
                    {
                        s += " every minute";
                    }
                    else
                    {
                        s += " at " + this.Minutes + " minute";
                    }
                }
                else
                {
                    String rightHours = addNumberIfSingle(this.Hour);
                    String rightMinutes = addNumberIfSingle(this.Minutes);
                    s += " at ";
                    if (this.Minutes.Equals("*"))
                    {
                        s += this.Hour + " every minute";
                    }
                    else
                    {
                        s += rightHours + ":" + rightMinutes;
                    }
                }
            }
            return s;
        }
        /// <summary>
        /// Get the Second half of the event name displayed 
        /// </summary>
        private String DayPartOfName(String s)
        {
            String rightMonth = giveToMonthTheRightName(this.Month);
            String rightDayOfTheWeek = getStringFromNumberDayOfTheWeek(this.DayOfTheWeek);
            s += ", ";
            if (this.DayOfTheWeek.Equals("*") && this.Day.Equals("*") && this.Month.Equals("*"))
            {
                s += "every day";
            }
            else
            {
                if (!this.DayOfTheWeek.Equals("*"))
                {
                    if (this.Day.Equals("*"))
                    {
                        s += "every " + rightDayOfTheWeek;
                    }
                    else
                    {
                        s += "the " + rightDayOfTheWeek + " " + ChangeNumberInWord(this.Day);
                    }
                }
                else
                {
                    if (this.Day.Equals("*"))
                    {
                        s += "every day";
                    }
                    else
                    {
                        s += "the " + ChangeNumberInWord(this.Day);
                    }
                }
                if (this.Month.Equals("*"))
                {
                    s += " of every month";
                }
                else
                {
                    s += " of " + rightMonth;
                }
            }
            return s;
        }

        /// <summary>
        /// add to the String number given the right suffix 
        /// </summary>
        private String ChangeNumberInWord(String s)
        {
            String[] slits = s.Split();
            String rightStringNumber;
            if (slits.Count() > 1)
            {
                rightStringNumber = slits[1];
            }
            else
            {
                rightStringNumber = slits[0];
            }
            switch (rightStringNumber)
            {
                case "0":
                    s += "";
                    break;
                case "1":
                    s += "st";
                    break;
                case "2":
                    s += "nd";
                    break;
                case "3":
                    s += "rd";
                    break;
                default:
                    s += "th";
                    break;
            }
            if (s.Equals("0"))
            {
                s = "";
            }
            return s;
        }

        /// <summary>
        /// add a 0 in front of a number if their is only one digit
        /// </summary>
        public String addNumberIfSingle(String s)
        {
            if (s.Count() == 1)
            {
                s = "0" + s;
            }
            return s;
        }

        /// <summary>
        /// change the string number,from 0 to 6, in his Equivalent day of the week word
        /// </summary>
        private String getStringFromNumberDayOfTheWeek(string rightString)
        {

            switch (rightString)
            {
                case "1":
                    rightString = "Monday";
                    break;
                case "2":
                    rightString = "Tuesday";
                    break;
                case "3":
                    rightString = "Wednesday";
                    break;
                case "4":
                    rightString = "Thursday";
                    break;
                case "5":
                    rightString = "Friday";
                    break;
                case "6":
                    rightString = "Saturday";
                    break;
                case "0":
                    rightString = "Sunday";
                    break;
            }
            return rightString;
        }

        /// <summary>
        /// change the string number,from 1 to 12, in his Equivalent Month word
        /// </summary>
        private string giveToMonthTheRightName(string s)
        {
            switch (s)
            {
                case "1":
                    s = "January";
                    break;
                case "2":
                    s = "February";
                    break;
                case "3":
                    s = "March";
                    break;
                case "4":
                    s = "April";
                    break;
                case "5":
                    s = "May";
                    break;
                case "6":
                    s = "June";
                    break;
                case "7":
                    s = "July";
                    break;
                case "8":
                    s = "August";
                    break;
                case "9":
                    s = "September";
                    break;
                case "10":
                    s = "October";
                    break;
                case "11":
                    s = "November";
                    break;
                case "12":
                    s = "December";
                    break;
            }
            return s;
        }


        /// <summary>
        /// add an event to the contrab file 
        /// </summary>
        public void addToContrab(){
            try
            {
                this.client.RunCommand("echo \"" + this.Minutes + " " + this.Hour + " " + this.Day + " " + this.Month + " " + DayOfTheWeek + " HomeConnector/profils/" + Scenario.Replace(' ', '-') + ".sh" + "\" | tee -a " + fileName);
                this.client.RunCommand("crontab " + fileName);
            }
            catch
            {
                
                //Event client_ErrorOccurred
            }
        }

        /// <summary>
        /// suppress the corresponding line of this event in contrab file.
        /// </summary>
        public void removeFromContrab()
        {
            try
            {
                int line = getEventLineInContrabFile();
                this.client.RunCommand("sed -i '" + line + " d' " + fileName);
                this.client.RunCommand("crontab " + fileName);
            }
            catch
            {
                //Event client_ErrorOccurred
            }
        }


        /// <summary>
        /// Suppress the script file and recreate it 
        /// </summary>
        public static void recreateScript(SshClient client){
            client.RunCommand("rm " + fileName);
            client.RunCommand("touch " + fileName);
            
        }


        /// <summary>
        /// get the List of all Event saved in contrab file
        /// </summary>
        public static List<Event> getAllEvent(SshClient client){
            List<Event> eventList = new List<Event>();
            SshCommand cmd = client.RunCommand("cat "+fileName);
            String[] list = cmd.Result.Split('\n');
            List<String> stringList = list.ToList<String>();
            String[] list2;
            String[] list3;
            String[] list4;
            if (stringList.Last<string>() == "")
            {
                stringList.RemoveAt(list.Length - 1);
            }
            foreach (String s in stringList)
            {   
                list2 = s.Split(new char[] { '/' });
                list3 = list2[0].Split(' ');
                list4 = list2.Last<String>().Split('.');
                Event ev = new Event(client,list3[0],list3[1],list3[2],list3[3],list3[4],list4[list4.Length - 2]);
                eventList.Add(ev);
             }
            return eventList;
        }


        /// <summary>
        /// add all event from the the list in contrab file
        /// </summary>
        public static void addAllEventToCrontab(List<Event> eventList)
        {
            foreach (Event e in eventList)
            {
                e.addToContrab();
            }
        }


        /// <summary>
        /// get event corresponding line from contrab file
        /// </summary>
        public int getEventLineInContrabFile()
        {
            String[] list2;
            String[] list3;
            String[] list4;
            int lineNumber = 1;
            List<String> stringList = getAllLineFromScriptFile(this.client);
            foreach (String s in stringList)
            {
                list2 = s.Split(new char[] { '/' });
                list3 = list2[0].Split(' ');
                list4 = list2.Last<String>().Split('.');
                if (list3[0].Equals(this.Minutes) && list3[1].Equals(this.Hour) && list3[2].Equals(this.Day) && list3[3].Equals(this.Month) && list3[4].Equals(this.DayOfTheWeek) && list4[list4.Length - 2].Equals(this.Scenario))
                {
                    return lineNumber;
                }
                lineNumber++;
            }
            return -1;

        }
        /// <summary>
        /// get all event line from script file
        /// </summary>
        public static List<String> getAllLineFromScriptFile(SshClient client)
        {
            try
            {
                SshCommand cmd = client.RunCommand("cat " + fileName);
                String[] list = cmd.Result.Split('\n');
                List<String> stringList = list.ToList<String>();
                if (stringList.Last<string>() == "")
                {
                    stringList.RemoveAt(list.Length - 1);
                }
                return stringList;
            }
            catch
            {
                //Event client_ErrorOccurred
                return null;
            }

        }

        /// <summary>
        /// get all Event corresponding line that contains this specific scenario name
        /// </summary>
        public static List<int> getAllLineWhoContainsName(String name, SshClient client)
        {
            String[] list;
            String[] list1;
            List<int> lineNumberList = new List<int>();
            int lineNumber = 1;
            List<String> stringList = getAllLineFromScriptFile(client);
            foreach (String s in stringList)
            {
                list = s.Split(new char[] { '/' });
                list1 = list.Last<String>().Split('.');
                String rightName = list1[0].Replace(' ', '-');
                if (rightName.Equals(name))
                {
                    lineNumberList.Add(lineNumber);
                }
                lineNumber++;
            }
            return lineNumberList;
        }
        /// <summary>
        /// Delete all Event that contains a specific scenario name from contrab file
        /// </summary>
        public static void deleteAllEventWhoContainsThatScenario(String name, SshClient client)
        {
            List<int> LineNumberList = getAllLineWhoContainsName(name, client);
            int tmp;
            for (int index = 0; index < LineNumberList.Count; index++)
            {

                if (index != 0)
                {
                    tmp = LineNumberList[index] - index;
                }
                else
                {
                    tmp = LineNumberList[index];
                }
                client.RunCommand("sed -i '" + tmp + " d' " + fileName);
            }


            client.RunCommand("crontab " + fileName);

        }
        /// <summary>
        /// Change string word in string number corresponding to the day of the week
        /// </summary>
        private String getNumberFromString(string dayOfTheWeek)
        {
            String rightString = "*";
            switch (dayOfTheWeek)
            {
                case "Monday":
                    rightString = "1";
                    break;
                case "Tuesday":
                    rightString = "2";
                    break;
                case "Wednesday":
                    rightString = "3";
                    break;
                case "Thursday":
                    rightString = "4";
                    break;
                case "Friday":
                    rightString = "5";
                    break;
                case "Saturday":
                    rightString = "6";
                    break;
                case "Sunday":
                    rightString = "0";
                    break;
            }
            return rightString;
        }

    }
}
