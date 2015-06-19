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
        public Scenario()
        {
            codes = new List<string>();
            name = "";

        }
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
        public Scenario(String name, List<String> codes)
        {
            this.codes = codes;
            this.name = name;
        }

        public static List<Scenario> getScenarios(SshClient c)
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
        public static List<String> getCodes(SshClient c, string fileName)
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

        public static bool canCreate(SshClient c, string fileName)
        {
            SshCommand cmd = c.RunCommand("ls HomeConnector/profils ");
            string[] profils = cmd.Result.Split('\n');

            return !profils.Contains<String>(fileName);
        }
        public void launch(SshClient c)
        {
            SshCommand command = c.CreateCommand("sudo ./HomeConnector/profils/" + name + ".sh");
            command.BeginExecute();

        }

        public static Scenario getScenario(SshClient c , String s) {
            String[] splitted = s.Split('/');
            String scName = splitted.Last().Replace(".sh","");
            List<Scenario> scenarios = getScenarios(c);
            foreach (Scenario sce in scenarios) {
                if (sce.name == scName) {
                    return sce;
                }
            }
            return null;
        }

        public static void removeReceptorFromScenarios(SshClient c ,Receptor r) {
            List<Scenario> scenarios = Scenario.getScenarios(c);
            foreach (Scenario sc in scenarios) {
                Boolean changed = false;
                List<String> newCodes = new List<string>();
                foreach (string code in sc.codes)
                {

                    if (code != r.commandeOn && code != r.commandeOff)
                    {
                        //sc.codes.Remove(code);
                        newCodes.Add(code);
                    }
                    else {
                        changed = true;
                    }
                }
                if (changed == true)
                {
                    c.RunCommand("rm HomeConnector/profils/"+sc.name+".sh");
                    c.RunCommand("touch HomeConnector/profils/" + sc.name + ".sh");
                    foreach (String s in newCodes) {
                        c.RunCommand("echo \"sudo ./HomeConnector/codesend " + s + "\" | tee -a HomeConnector/profils/" + sc.name + ".sh");
                    }
                    c.RunCommand("chmod +x HomeConnector/profils/" + sc.name + ".sh");
                    //recreate scenario
                }
            }
        }
    }
}
