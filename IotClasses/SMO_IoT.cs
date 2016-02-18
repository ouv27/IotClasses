using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Principal;
using System.Net;
using System.Management;
using System.Diagnostics;

namespace SMO_IoT
{

    public class Iot_Constants
    {
        public string sFirstTopicArgument = "SMOIOT/";
        public string[] sLocalisation1 = new string[3]
        {"SMOHOME","ACCORHOTELS","OTHER"};
        public string[] sLocalisation2SMOHOME = new string[4]
        { "/BUREAU","/SALON","/SAS","/GARAGE"};
        public string[] sLocalisation2ACCORHOTELS = new string[4]
        { "/ODYSSEY","/EVRY","/AVIA","/OTHER"};

    }

    /// <summary>
    /// /SMO_IoT/MachineOrObjet/DateTime/Unit/Localisation1/Localisation2/... 
    /// MachineOrObjet si avec IP : NomMachine(IP)
    /// ex : /SMO_IoT/
    /// On Error : /SMO_IoT/Error/exception
    /// </summary>
    public class IoT_Topics
    {
        public DateTime CreationDate;
        public string sMachineOrObjet;
        public string sUnit;
        public string sLocalisation;
        public bool bInit;
        public string sTopics;
        public IoT_Topics()
        {
            sMachineOrObjet = sUnit = sLocalisation = "";
            bInit = false;
            sTopics = "";
        }
        public string Get_Topics(string MachineOrObjet, string Unit, string Localisation)
        {
            sTopics = "";
            Iot_Constants IOTCONST = new Iot_Constants();

            try
            {
                sMachineOrObjet = MachineOrObjet;
                sUnit = Unit;
                sLocalisation = Localisation;
                CreationDate = DateTime.UtcNow;
                sTopics = IOTCONST.sFirstTopicArgument + sMachineOrObjet + "/" + CreationDate + "/" + sUnit + "/" + Localisation;
                return sTopics;
            }
            catch (Exception e)
            {
                sTopics = "/SMOIOT/Error/Exception" + e.Message;
                return sTopics;
            }
        }
        public string Get_Topics(string Unit, string Localisation)
        {
            sTopics = "";
            Iot_Constants IOTCONST = new Iot_Constants();
            try
            {
                Iot_Machine mach = new Iot_Machine();
                sMachineOrObjet = mach.GetMachineName();
                sUnit = Unit;
                sLocalisation = Localisation;
                CreationDate = DateTime.UtcNow;
                Iot_Machine iotmachine = new Iot_Machine();

                sTopics = IOTCONST.sFirstTopicArgument + iotmachine.GetMachineName() + "/" + CreationDate + "/" + sUnit + "/" + Localisation;
                return sTopics;
            }
            catch (Exception e)
            {
                sTopics = "/SMO_IoT/Error/Exception" + e.Message;
                return sTopics;
            }
        }
    }

    public class Iot_Machine
    {
        public string GetMachineName()
        {
            string sMachine = Environment.MachineName;

            return sMachine;
        }
        public string GetNbProc()
        {
            string sNbProc = Environment.ProcessorCount.ToString();
            return sNbProc;
        }

        public string getAvailableRAM()
        {
                PerformanceCounter ramCounter = new PerformanceCounter("Memory", "Available MBytes");
                return ramCounter.NextValue() + "Mb";
        }


    }

}
