using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;



/// <summary>
/// Summary description for EssentialInfo
/// </summary>

namespace System
{
    public static class EssentialInfo
    {
        private static string macAddress { get; set; }
        public static string MACADDRESS { get { return macAddress; } }

        public static void GetMacAddress()
        {
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                // Only consider Ethernet network interfaces
                if (nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet &&
                    nic.OperationalStatus == OperationalStatus.Up)
                {
                    macAddress = nic.GetPhysicalAddress().ToString();
                }
            }
        }
    }
}