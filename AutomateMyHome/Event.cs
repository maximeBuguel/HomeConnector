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
        
        

        public Event(){
            this.isACreator = true;
        }

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

        public Event(SshClient client, String minute, String hour,String day, String month, String dayOfTheWeek,String scenario){
            this.client = client;
            this.Scenario = scenario;
            this.Minutes = minute;
            this.Hour = hour;
            this.Day = day;
            this.Month = month;
            this.DayOfTheWeek = dayOfTheWeek;
            this.client.RunCommand("crontab " + fileName);
        }

        

        public String  getName()
        {
            return this.Scenario.Replace('-',' ')+" : min :" + this.Minutes +" hour :" + this.Hour + " day :" +this.Day + " month :" + this.Month + " ";
        }

        public void addToContrab(){
            this.client.RunCommand("echo \"" + this.Minutes + " " + this.Hour + " " + this.Day + " " + this.Month + " " + DayOfTheWeek + " HomeConnector/profils/" + Scenario.Replace(' ', '-') + ".sh" + "\" | tee -a "+ fileName);
        }

        public void removeFromContrab()
        {
            int line = getEventLineInContrabFile();
            this.client.RunCommand("sed -i '"+ line +" d' "+ fileName);
            this.client.RunCommand("crontab " + fileName);
        }

        public static void recreateScript(SshClient client){
            client.RunCommand("rm " + fileName);
            client.RunCommand("touch " + fileName);
            
        }
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
            recreateScript(client);
            addAllEventToCrontab(eventList);
            return eventList;
        }

        public static void addAllEventToCrontab(List<Event> eventList)
        {
            foreach (Event e in eventList)
            {
                e.addToContrab();
            }
        }

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

        public static List<String> getAllLineFromScriptFile(SshClient client)
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
