using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Certificates
{
    public class Formatter
    {
        public static string ParseName(string winLogonName)
        {
            string[] parts = new string[] { };

            if (winLogonName.Contains("@"))
            {
                ///UPN format
                parts = winLogonName.Split('@');
                return parts[0];
            }
            else if (winLogonName.Contains("\\"))
            {
                /// SPN format
                parts = winLogonName.Split('\\');
                return parts[1];
            }
            else
            {
                return winLogonName;
            }
        }

        public static string ParseOU(string certificate)
        {
            
            string name = certificate.Split('=')[2];

            return name;

            /*string[] parts = new string[] { };
            string parts1;
            string[] parts2 = new string[] { };

            if (certificate.Contains(" "))
            {
                parts = certificate.Split(' ');
                parts1 = parts[1];
                parts2 = parts1.Split('=');
                return parts2[1];
            }
            else
            {
                return certificate;
            }*/
        }

        public static string ParseCN(string certificate)
        {
            string name = certificate.Split('=')[1];

            string name1 = name.Split(',')[0];

            return name1;
        }
    }
}
