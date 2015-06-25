using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutomateMyHome
{
    public class Scenario
    {
        public String name { get; set; }
        public List<String> codes { get; set; }

        /// <summary>
        /// Scenario default constructor constructor
        /// </summary>
        public Scenario()
        {
            codes = new List<string>();
            name = "";

        }

        /// <summary>
        /// create an instance of an exixting secnario
        /// </summary>
        public Scenario(String name, List<String> codes)
        {
            this.codes = codes;
            this.name = name;
        }

        /// <summary>
        /// Set the name of the scenario to the first name available begining with "New scenario"
        /// </summary>
        public void initName(SshClient c)
        {
            String beginName = "New-scenario";
            name = beginName;
            int i = 0;
            while (!canCreate(c, name + ".sh"))
            {
                i++;
                name = beginName + i;
            }

        }

        /// <summary>
        /// Return the list of scenarios of the Home Box
        /// </summary>
        public static List<Scenario> getScenarios(SshClient c)
        {
            try
            {
                List<Scenario> ans = new List<Scenario>();
                SshCommand cmd = c.RunCommand("ls HomeConnector/profils ");
                string[] profils = cmd.Result.Split('\n');
                int size = profils.Length;
                for (int i = 0; i < size - 1; i++)
                {
                    string profil = profils[i];
                    string[] name = profil.Split('.');
                    ans.Add(new Scenario(name[0], getCodes(c, profil)));
                }
                return ans;
            }
            catch {
                //Event client_ErrorOccurred
                return null;
            }
        }

        /// <summary>
        /// Return the list of scenario's names of the Home Box
        /// </summary>
        public static List<String> getScenariosNames(SshClient c)
        {
            try
            {
                List<String> ans = new List<String>();
                SshCommand cmd = c.RunCommand("ls HomeConnector/profils ");
                string[] profils = cmd.Result.Split('\n');
                int size = profils.Length;
                for (int i = 0; i < size - 1; i++)
                {
                    string profil = profils[i];
                    string[] name = profil.Split('.');
                    ans.Add(name[0]);
                }
                return ans;
            }
            catch
            {
                //Event client_ErrorOccurred
                return null;
            }
        }

        /// <summary>
        /// Return the list of the codes ( signals ) of a scenario
        /// </summary>
        public static List<String> getCodes(SshClient c, string fileName)
        {
            try
            {
                List<String> ans = new List<String>();
                SshCommand cmd = c.RunCommand("cat HomeConnector/profils/" + fileName);
                string[] lines = cmd.Result.Split('\n');
                List<String> li = lines.ToList<string>();
                if (li.Last<string>() == "")
                {
                    li.RemoveAt(lines.Length - 1);

                }
                foreach (string line in li)
                {

                    string[] splittedLines = line.Split(' ');

                    ans.Add(splittedLines[splittedLines.Length - 1]);
                }

                return ans;
            }
            catch
            {
                //Event client_ErrorOccurred
                return null;
            }
        }

        /// <summary>
        /// Check if a Name is available for a scenario
        /// </summary>
        public static bool canCreate(SshClient c, string fileName)
        {
            SshCommand cmd = c.RunCommand("ls HomeConnector/profils ");
            string[] profils = cmd.Result.Split('\n');

            return !profils.Contains<String>(fileName);
        }

        /// <summary>
        /// Launch the scenario
        /// </summary>
        public void launch(SshClient c)
        {
            SshCommand command = c.CreateCommand("sudo ./HomeConnector/profils/" + name + ".sh");
            command.BeginExecute();

        }

        /// <summary>
        /// Remove all activation codes of a Receptor in a list of scenario codes
        /// </summary>
        public static void removeReceptorFromScenarios(SshClient c, Receptor r)
        {
            try
            {
                List<Scenario> scenarios = Scenario.getScenarios(c);
                foreach (Scenario sc in scenarios)
                {
                    Boolean changed = false;
                    List<String> newCodes = new List<string>();
                    foreach (string code in sc.codes)
                    {

                        if (code != r.commandeOn && code != r.commandeOff)
                        {
                            
                            newCodes.Add(code);
                        }
                        else
                        {
                            changed = true;
                        }
                    }
                    if (changed == true)
                    {
                        c.RunCommand("rm HomeConnector/profils/" + sc.name + ".sh");
                        c.RunCommand("touch HomeConnector/profils/" + sc.name + ".sh");
                        foreach (String s in newCodes)
                        {
                            c.RunCommand("echo \"sudo ./HomeConnector/codesend " + s + "\" | tee -a HomeConnector/profils/" + sc.name + ".sh");
                        }
                        c.RunCommand("chmod +x HomeConnector/profils/" + sc.name + ".sh");
                        //recreate scenario
                    }
                }
            }
            catch
            {
                //Event client_ErrorOccurred
            }
        }
        }
}
