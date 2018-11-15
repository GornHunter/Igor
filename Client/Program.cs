using Certificates;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        public static void Main(string[] args)
        {
            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

            X509Certificate2 srvCert = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, "Server");
            EndpointAddress address = new EndpointAddress(new Uri("net.tcp://localhost:9999/Service"), new X509CertificateEndpointIdentity(srvCert));

            using (ClientProxy proxy = new ClientProxy(binding, address))
            {
                string cltCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);

                X509Certificate2 x = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, cltCertCN);

                string ou = Formatter.ParseOU(x.Subject);

                string cn = Formatter.ParseCN(x.Subject);

                int num;
                string themeName = string.Empty;
                string oldThemeName = string.Empty;
                string newThemeName = string.Empty;
                string title = string.Empty;
                string content = string.Empty;
                string timeStamp = string.Empty;
                string error = string.Empty;
                string[] banned = null;
                string answer = string.Empty;

                while (true)
                {
                    Console.WriteLine("************MENU**************");
                    Console.WriteLine("******************************");
                    Console.WriteLine($"Welcome {cn} from group {ou}\n");
                    Console.WriteLine("1.Ban publisher");
                    Console.WriteLine("2.Check for publisher spam");
                    Console.WriteLine("3.Add theme");
                    Console.WriteLine("4.Modify theme");
                    Console.WriteLine("5.Remove theme");
                    Console.WriteLine("6.Write theme(publisher)");
                    Console.WriteLine("7.Read theme(publisher)");
                    Console.WriteLine("8.Send notifications");
                    Console.WriteLine("9.Subscribe to theme");
                    Console.WriteLine("10.Unsubscribe from theme");
                    Console.WriteLine("11.Read themes(subscriber)");
                    Console.WriteLine("12.Read incoming notifications");
                    Console.WriteLine("13.Update subscriber file");
                    Console.WriteLine("14.Logout");

                    Console.WriteLine("\nEnter number: ");
                    num = Int32.Parse(Console.ReadLine());

                    switch (num)
                    {
                        case 1:
                            if (ou != "Admin")
                            {
                                Console.WriteLine("\nYou don't have rights to access this operation!");
                                Console.ReadLine();
                                Console.Clear();
                                break;
                            }

                            foreach (var item in banned)
                            {
                                 jump:
                                 Console.WriteLine($"\nDo you want to ban {item}?");
                                 answer = Console.ReadLine();

                                 if (answer == "yes")
                                 {
                                    error = proxy.AdminBan(ou, item);
                                    Console.WriteLine(error);
                                    Console.ReadLine();
                                 }
                                 else if (answer == "no")
                                 {
                                    Console.WriteLine($"\nPublisher {item} won't be banned!");
                                    Console.ReadLine();
                                 }
                                 else
                                 {
                                    Console.WriteLine("\nYour option was invalid!");
                                    Console.ReadLine();
                                    goto jump;
                                 }
                            }

                            error = string.Empty;

                            Console.Clear();
                            break;
                        case 2:
                            if (ou != "Admin")
                            {
                                Console.WriteLine("\nYou don't have rights to access this operation!");
                                Console.ReadLine();
                                Console.Clear();
                                break;
                            }

                            banned = proxy.CheckPublisherSpam(ou);

                            if(banned == null)
                            {
                                Console.WriteLine("\nThere are no people for banning!");
                            }
                            else
                            {
                                Console.WriteLine("\nThere are people for banning!");
                            }

                            Console.ReadLine();
                            Console.Clear();
                            break;
                        case 3:
                            if (ou != "Publisher")
                            {
                                Console.WriteLine("\nYou don't have rights to access this operation!");
                                Console.ReadLine();
                                Console.Clear();
                                break;
                            }

                            Console.WriteLine("\nInsert theme you want to create: ");
                            themeName = Console.ReadLine();

                            error = proxy.PublisherAddTheme(ou, cn, themeName);
                            Console.WriteLine(error);
                            error = string.Empty;
                            themeName = string.Empty;

                            Console.ReadLine();
                            Console.Clear();
                            break;
                        case 4:
                            if (ou != "Publisher")
                            {
                                Console.WriteLine("\nYou don't have rights to access this operation!");
                                Console.ReadLine();
                                Console.Clear();
                                break;
                            }

                            Console.WriteLine("\nInsert theme you want to modify: ");
                            oldThemeName = Console.ReadLine();
                            Console.WriteLine("Insert new theme name: ");
                            newThemeName = Console.ReadLine();

                            error = proxy.PublisherModifyTheme(ou, cn, oldThemeName, newThemeName);
                            Console.WriteLine(error);
                            error = string.Empty;

                            Console.ReadLine();
                            Console.Clear();
                            break;
                        case 5:
                            if (ou != "Publisher")
                            {
                                Console.WriteLine("\nYou don't have rights to access this operation!");
                                Console.ReadLine();
                                Console.Clear();
                                break;
                            }

                            Console.WriteLine("\nInsert theme you want to remove: ");
                            themeName = Console.ReadLine();

                            error = proxy.PublisherRemoveTheme(ou, cn, themeName);
                            Console.WriteLine(error);
                            error = string.Empty;
                            themeName = string.Empty;

                            Console.ReadLine();
                            Console.Clear();
                            break;
                        case 6:
                            if (ou != "Publisher")
                            {
                                Console.WriteLine("\nYou don't have rights to access this operation!");
                                Console.ReadLine();
                                Console.Clear();
                                break;
                            }

                            error = proxy.PublisherWriteTheme(ou, cn);
                            Console.WriteLine(error);
                            error = string.Empty;

                            Console.ReadLine();
                            Console.Clear();
                            break;
                        case 7:
                            if (ou != "Publisher")
                            {
                                Console.WriteLine("\nYou don't have rights to access this operation!");
                                Console.ReadLine();
                                Console.Clear();
                                break;
                            }

                            Console.WriteLine("\nAll themes:");

                            error = proxy.PublisherReadTheme(ou, cn);
                            Console.WriteLine(error);
                            error = string.Empty;

                            Console.ReadLine();
                            Console.Clear();
                            break;
                        case 8:
                            if (ou != "Publisher")
                            {
                                Console.WriteLine("You don't have rights to access this operation!");
                                Console.ReadLine();
                                Console.Clear();
                                break;
                            }

                            Console.WriteLine("\nInsert theme you want to send your notifications to: ");
                            themeName = Console.ReadLine();
                            Console.WriteLine("Insert title of notification: ");
                            title = Console.ReadLine();
                            Console.WriteLine("Insert content of notification: ");
                            content = Console.ReadLine();
                            Console.WriteLine("Insert time stamp of notification: ");
                            timeStamp = Console.ReadLine();


                            error = proxy.PublisherSendNotifications(ou, cn, themeName, title, content, timeStamp);
                            Console.WriteLine(error);
                            error = string.Empty;
                            themeName = string.Empty;

                            Console.ReadLine();
                            Console.Clear();
                            break;
                        case 9:
                            if (ou != "Subscriber")
                            {
                                Console.WriteLine("You don't have rights to access this operation!");
                                Console.ReadLine();
                                Console.Clear();
                                break;
                            }

                            Console.WriteLine("\nInsert theme you want to subscribe to: ");
                            themeName = Console.ReadLine();

                            error = proxy.Subscribe(ou, cn, themeName);
                            Console.WriteLine(error);
                            error = string.Empty;
                            themeName = string.Empty;

                            Console.ReadLine();
                            Console.Clear();
                            break;
                        case 10:
                            if (ou != "Subscriber")
                            {
                                Console.WriteLine("You don't have rights to access this operation!");
                                Console.ReadLine();
                                Console.Clear();
                                break;
                            }

                            Console.WriteLine("Insert theme you want to unsubscribe from: ");
                            themeName = Console.ReadLine();

                            error = proxy.Unsubscribe(ou, cn, themeName);
                            Console.WriteLine(error);
                            error = string.Empty;
                            themeName = string.Empty;

                            Console.ReadLine();
                            Console.Clear();
                            break;
                        case 11:
                            if (ou != "Subscriber")
                            {
                                Console.WriteLine("You don't have rights to access this operation!");
                                Console.ReadLine();
                                Console.Clear();
                                break;
                            }

                            Console.WriteLine("\nThemes you can subscribe to: ");

                            error = proxy.SubscriberReadTheme(ou);
                            Console.WriteLine(error);
                            error = string.Empty;

                            Console.ReadLine();
                            Console.Clear();
                            break;
                        case 12:
                            if (ou != "Subscriber")
                            {
                                Console.WriteLine("You don't have rights to access this operation!");
                                Console.ReadLine();
                                Console.Clear();
                                break;
                            }

                            error = proxy.SubscriberReadNotifications(ou, cn);
                            Console.WriteLine(error);
                            error = string.Empty;

                            Console.ReadLine();
                            Console.Clear();
                            break;
                        case 13:
                            if (ou != "Subscriber")
                            {
                                Console.WriteLine("You don't have rights to access this operation!");
                                Console.ReadLine();
                                Console.Clear();
                                break;
                            }

                            error = proxy.SubscriberCheckUpdates(ou, cn);
                            Console.WriteLine(error);
                            error = string.Empty;

                            Console.ReadLine();
                            Console.Clear();
                            break;
                        case 14:
                            Environment.Exit(0);
                            break;
                        default:
                            Console.WriteLine("Your option was out of bounds!");
                            Console.ReadLine();
                            Console.Clear();
                            break;
                    }
                }
            }
        }
    }
}