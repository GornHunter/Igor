using Certificates;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class ClientProxy : ChannelFactory<IService>, IService, IDisposable
    {
        IService factory;

        public ClientProxy(NetTcpBinding binding, EndpointAddress address) : base(binding, address)
        {
            /// cltCertCN.SubjectName should be set to the client's username. .NET WindowsIdentity class provides information about Windows user running the given process
            string cltCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);

            this.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

            /// Set appropriate client's certificate on the channel. Use CertManager class to obtain the certificate based on the "cltCertCN"
            this.Credentials.ClientCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, cltCertCN);

            factory = this.CreateChannel();
        }

        public string AdminBan(string ou, string publisher)
        {
            string name = string.Empty;

            try
            {
                name = factory.AdminBan(ou, publisher);
                return name;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
                return name;
            }
        }

        public string[] CheckPublisherSpam(string ou)
        {
            string[] name = new string[] { };

            try
            {
                name = factory.CheckPublisherSpam(ou);
                return name;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
                return name;
            }
        }

        public string PublisherAddTheme(string ou, string cn, string themeName)
        {
            string name = string.Empty;

            try
            {
                name = factory.PublisherAddTheme(ou, cn, themeName);
                return name;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
                return name;
            }
        }

        public string PublisherModifyTheme(string ou, string cn, string oldThemeName, string newThemeName)
        {
            string name = string.Empty;

            try
            {
                name = factory.PublisherModifyTheme(ou, cn, oldThemeName, newThemeName);
                return name;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
                return name;
            }
        }

        public string PublisherReadTheme(string ou, string cn)
        {
            string name = string.Empty;

            try
            {
                name = factory.PublisherReadTheme(ou, cn);
                return name;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
                return name;
            }
        }

        public string PublisherWriteTheme(string ou, string cn)
        {
            string name = string.Empty;

            try
            {
                name = factory.PublisherWriteTheme(ou, cn);
                return name;
            }
            catch(Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
                return name;
            }
        }

        public string PublisherRemoveTheme(string ou, string cn, string themeName)
        {
            string name = string.Empty;

            try
            {
                name = factory.PublisherRemoveTheme(ou, cn, themeName);
                return name;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
                return name;
            }
        }

        public string PublisherSendNotifications(string ou, string cn, string themeName, string title, string content, string timeStamp)
        {
            string name = string.Empty;

            try
            {
                name = factory.PublisherSendNotifications(ou, cn, themeName, title, content, timeStamp);
                return name;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
                return name;
            }
        }

        public string Subscribe(string ou, string cn, string themeName)
        {
            string name = string.Empty;

            try
            {
                name = factory.Subscribe(ou, cn, themeName);
                return name;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
                return name;
            }
        }

        public string SubscriberReadNotifications(string ou, string cn)
        {
            string name = string.Empty;

            try
            {
                name = factory.SubscriberReadNotifications(ou, cn);
                return name;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
                return name;
            }
        }

        public string SubscriberReadTheme(string ou)
        {
            string name = string.Empty;

            try
            {
                name = factory.SubscriberReadTheme(ou);
                return name;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
                return name;
            }
        }

        public string Unsubscribe(string ou, string cn, string themeName)
        {
            string name = string.Empty;

            try
            {
                name = factory.Unsubscribe(ou, cn, themeName);
                return name;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
                return name;
            }
        }

        public string SubscriberCheckUpdates(string ou, string cn)
        {
            string name = string.Empty;

            try
            {
                name = factory.SubscriberCheckUpdates(ou, cn);
                return name;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
                return name;
            }
        }

        public bool CheckIfPublisherBanned(string cn)
        {
            bool name = false;

            try
            {
                name = factory.CheckIfPublisherBanned(cn);
                return name;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
                return name;
            }
        }
    }
}