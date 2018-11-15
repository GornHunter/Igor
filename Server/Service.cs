using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Service : IService
    {
        public static Dictionary<string, Notification> themes = new Dictionary<string, Notification>();

        public static List<string> list = new List<string>();

        public string AdminBan(string ou, string publisher)
        {
            if(ou != "Admin")
            {
                return "You don't have rights to access this operation!";
            }

            if (!File.Exists("banned.txt"))
            {
                using (StreamWriter w = File.CreateText("banned.txt"))
                {
                    w.WriteLine(publisher);

                    w.Close();
                }
            }
            else
            {
                string file = File.ReadAllText("banned.txt");

                using (StreamWriter w = File.AppendText("banned.txt"))
                {
                    if (file.Contains(publisher))
                    {
                        return $"\nPublisher {publisher} is already banned!";
                    }
                    else
                    {
                        w.WriteLine(publisher);

                        w.Close();
                    }
                }
            }

            return "";
        }

        public string[] CheckPublisherSpam(string ou)
        {
            if (ou != "Admin")
            {
                string[] temp = new string[] { "You don't have rights to access this operation!" };
                return temp;
            }

            string path = Path.GetFullPath(@"..");
            string[] filePaths = Directory.GetFiles(path, "*.txt", SearchOption.AllDirectories);

            string[] clientFile = File.ReadAllLines("themes.txt");
            string res = string.Empty;
            string title1 = string.Empty;
            string content1 = string.Empty;
            string title2 = string.Empty;
            string content2 = string.Empty;
            string title3 = string.Empty;
            string content3 = string.Empty;
            clientFile.ToList();

            List<string> names = new List<string>();
            List<string> names1 = new List<string>();

            foreach (var item in filePaths)
            {
                names.Add(item.Split('\\')[9]);
            }

            foreach (var item in names)
            {
                names1.Add(item.Split('.')[0]);
            }

            List<string> banned = new List<string>();
            string[] retBanned = new string[] { };

            foreach (string file in names1)
            {
                if (clientFile.Contains(file))
                {
                    string[] newFile = File.ReadAllLines($"{file}.txt");

                    title1 = newFile[1];
                    content1 = newFile[2];

                    title2 = newFile[5];
                    content2 = newFile[6];

                    title3 = newFile[9];
                    content3 = newFile[10];

                    if (title1 == title2 && title1 == title3 && content1 == content2 && content1 == content3)
                    {
                        Console.WriteLine($"{newFile[0]} is spamming in {file}.txt!");

                        if (!banned.Contains(newFile[0]))
                        {
                            banned.Add(newFile[0]);
                        }
                    }
                }
            }

            retBanned = banned.ToArray();

            return retBanned;
        }

        public string PublisherAddTheme(string ou, string cn, string themeName)
        {
            if (ou != "Publisher")
            {
                return "You don't have rights to access this operation!";
            }

            if(CheckIfPublisherBanned(cn) == true)
            {
                return "You are banned!";
            }

            if (!themes.ContainsKey(themeName))
            {
                themes.Add(themeName, new Notification(null, null, null));

                using (StreamWriter w = File.AppendText($"{themeName}.txt"))
                {
                    w.WriteLine($"{cn}");

                    w.Close();
                }
            }
            else
            {
                return "Theme already exists!";
            }

            return "Success!";
        }

        public string PublisherModifyTheme(string ou, string cn, string oldThemeName, string newThemeName)
        {
            if (ou != "Publisher")
            {
                return "You don't have rights to access this operation!";
            }

            if (CheckIfPublisherBanned(cn) == true)
            {
                return "You are banned!";
            }

            if (File.Exists($"{oldThemeName}.txt"))
            {
                string[] linesTemp = File.ReadAllLines($"{oldThemeName}.txt");

                if (linesTemp[0] != cn)
                {
                    return "You can't modify theme that other publisher created!";
                }
            }
            else
            {
                return "File doesn't exist!";
            }

            if (themes.ContainsKey(oldThemeName))
            {
                if (File.Exists("themes.txt"))
                {
                    themes.Remove(oldThemeName);
                    themes.Add(newThemeName, new Notification(null, null, null));
                    File.Move($"{oldThemeName}.txt", $"{newThemeName}.txt");

                    string[] lines = File.ReadAllLines("themes.txt");
                    List<string> newLines = new List<string>();

                    foreach (var item in lines)
                    {
                        if (item == oldThemeName)
                        {
                            // preskace se ta linija               
                        }
                        else
                        {
                            newLines.Add(item);
                        }
                    }

                    using (StreamWriter w = new StreamWriter("themes.txt", false))
                    {
                        foreach(var item in newLines)
                        {
                            w.WriteLine(item);
                        }

                        w.WriteLine(newThemeName);

                        w.Close();
                    }
                }
                else
                {
                    return "File themes.txt doesn't exist!";
                }
            }
            else
            {
                return "Theme doesn't exist!";
            }

            return "Success!";
        }

        public string PublisherReadTheme(string ou, string cn)
        {
            if (ou != "Publisher")
            {
                return "You don't have rights to access this operation!";
            }

            if (CheckIfPublisherBanned(cn) == true)
            {
                return "You are banned!";
            }

            if (File.Exists("themes.txt"))
            {
                return File.ReadAllText("themes.txt");
            }
            else
            {
                return "File themes.txt doesn't exist!";
            }
        }

        public string PublisherWriteTheme(string ou, string cn)
        {
            if (ou != "Publisher")
            {
                return "You don't have rights to access this operation!";
            }

            if (CheckIfPublisherBanned(cn) == true)
            {
                return "You are banned!";
            }

            using (StreamWriter w = File.CreateText("themes.txt"))
            {
                foreach(var item in themes.Keys)
                {
                    w.WriteLine(item);
                }

                w.Close();
            }

            return "Success!";
        }

        public string PublisherRemoveTheme(string ou, string cn, string themeName)
        {
            if (ou != "Publisher")
            {
                return "You don't have rights to access this operation!";
            }

            if (CheckIfPublisherBanned(cn) == true)
            {
                return "You are banned!";
            }

            if (File.Exists($"{themeName}.txt"))
            {
                string[] linesTemp = File.ReadAllLines($"{themeName}.txt");

                if(linesTemp[0] != cn)
                {
                    return "You can't remove theme that other publisher created!";
                }
            }
            else
            {
                return "File doesn't exist!";
            }

            if (themes.ContainsKey(themeName))
            {
                themes.Remove(themeName);

                File.Delete($"{themeName}.txt");

                if (File.Exists("themes.txt"))
                {
                    string[] lines = File.ReadAllLines("themes.txt");
                    List<string> newLines = new List<string>();

                    foreach (var item in lines)
                    {
                        if (item == themeName)
                        {
                            // preskace se ta linija               
                        }
                        else
                        {
                            newLines.Add(item);
                        }
                    }

                    using (StreamWriter w = new StreamWriter("themes.txt", false))
                    {
                        foreach (var item in newLines)
                        {
                            w.WriteLine(item);
                        }

                        w.Close();
                    }
                }
                else
                {
                    return "File themes.txt doesn't exist!";
                }
            }
            else
            {
                return "Theme doesn't exist!";
            }

            return "Success!";
        }

        public string PublisherSendNotifications(string ou, string cn, string themeName, string title, string content, string timeStamp)
        {
            if (ou != "Publisher")
            {
                return "You don't have rights to access this operation!";
            }

            if (CheckIfPublisherBanned(cn) == true)
            {
                return "You are banned!";
            }

            if (File.Exists($"{themeName}.txt"))
            {
                string[] linesTemp = File.ReadAllLines($"{themeName}.txt");

                if (linesTemp[0] != cn)
                {
                    return "You can't send notifications to theme that other publisher created!";
                }
            }
            else
            {
                return "File doesn't exist!";
            }

            if (themes.ContainsKey(themeName))
            {
                themes[themeName].Title = title;
                themes[themeName].Content = content;
                themes[themeName].TimeStamp = timeStamp;

                using (StreamWriter w = File.AppendText($"{themeName}.txt"))
                {
                    w.WriteLine($"Title: {title}");
                    w.WriteLine($"Content: {content}");
                    w.WriteLine($"Time stamp: {timeStamp}");
                    w.WriteLine("-----------------------------------");

                    w.Close();
                }
            }
            else
            {
                return "Theme doesn't exist!";
            }

            return "Success!";
        }

        public string Subscribe(string ou, string cn, string themeName)
        {
            if (ou != "Subscriber")
            {
                return "You don't have rights to access this operation!";
            }

            if (list.Contains(themeName))
            {
                if (File.Exists($"{cn}.txt"))
                {
                    string[] lines = File.ReadAllLines($"{cn}.txt");

                    foreach (var item in lines)
                    {
                        if (item == themeName)
                        {
                            return "You are already subscribed to that theme!";
                        }
                    }
                }
            }
            else
            {
                return "Theme doesn't exist!";
            }

            if (list.Contains(themeName))
            {
                if (!File.Exists($"{cn}.txt"))
                {
                    using (StreamWriter w = File.CreateText($"{cn}.txt"))
                    {
                        w.WriteLine(themeName);

                        w.Close();
                    }
                }
                else
                {
                     using (StreamWriter w = File.AppendText($"{cn}.txt"))
                     {
                         w.WriteLine(themeName);
                    
                         w.Close();
                     }
                }
            }

            return "Success!";
        }

        public string SubscriberReadNotifications(string ou, string cn)
        {
            if (ou != "Subscriber")
            {
                return "You don't have rights to access this operation!";
            }

            string path = Path.GetFullPath(@"..");
            string[] filePaths = Directory.GetFiles(path, "*.txt", SearchOption.AllDirectories);

            string[] clientFile = File.ReadAllLines($"{cn}.txt");
            string res = string.Empty;
            clientFile.ToList();

            List<string> names = new List<string>();
            List<string> names1 = new List<string>();

            foreach (var item in filePaths)
            {
                names.Add(item.Split('\\')[9]);
            }

            foreach (var item in names)
            {
                names1.Add(item.Split('.')[0]);
            }

            foreach (string file in names1)
            {
                if (clientFile.Contains(file))
                {
                    string read = File.ReadAllText($"{file}.txt");
                    res += read;
                    res += "\n\n";
                }
            }

            return res;
        }

        public string SubscriberReadTheme(string ou)
        {
            if (ou != "Subscriber")
            {
                return "You don't have rights to access this operation!";
            }

            foreach(var item in themes.Keys)
            {
                list.Add(item);
            }

            if (File.Exists("themes.txt"))
            {
                return File.ReadAllText("themes.txt");
            }
            else
            {
                return "File themes.txt doesn't exist!";
            }
        }

        public string Unsubscribe(string ou, string cn, string themeName)
        {
            if (ou != "Subscriber")
            {
                return "You don't have rights to access this operation!";
            }

            if (list.Contains(themeName))
            {
                if (File.Exists($"{cn}.txt"))
                {
                    string[] lines = File.ReadAllLines($"{cn}.txt");
                    List<string> newLines = new List<string>();
                
                    foreach (var item in lines)
                    {
                        if (item == themeName)
                        {
                            // preskace se ta linija               
                        }
                        else
                        {
                            newLines.Add(item);
                        }
                    }

                    using (StreamWriter w = new StreamWriter($"{cn}.txt", false))
                    {
                        foreach (var item in newLines)
                        {
                            w.WriteLine(item);
                        }

                        w.Close();
                    }
                }
                else
                {
                    return $"File {cn}.txt doesn't exist!";
                }
            }
            else
            {
                return "Theme doesn't exist!";
            }

            return "";
        }

        public string SubscriberCheckUpdates(string ou, string cn)
        {
            if (ou != "Subscriber")
            {
                return "You don't have rights to access this operation!";
            }

            if (File.Exists("themes.txt") && File.Exists($"{cn}.txt"))
            {
                string[] name = File.ReadAllLines($"{cn}.txt");
                List<string> nameList = name.ToList();

                string[] name1 = File.ReadAllLines("themes.txt");
                List<string> nameList1 = name1.ToList();

                List<string> newList = new List<string>();

                foreach(var item in nameList1)
                {
                    if (!nameList.Contains(item))
                    {
                        //preskace se linija koja ne postoji u themes.txt
                    }
                    else
                    {
                        newList.Add(item);
                    }
                }

                using (StreamWriter w = new StreamWriter($"{cn}.txt", false))
                {
                    foreach (var item in newList)
                    {
                        w.WriteLine(item);
                    }

                    w.Close();
                }
            }
            else
            {
                return $"Files {cn}.txt i themes.txt don't exist!";
            }

            return "Success!";
        }

        public bool CheckIfPublisherBanned(string cn)
        {
            string fileBanned = string.Empty;

            if (File.Exists("banned.txt"))
            {
                fileBanned = File.ReadAllText("banned.txt");
            }
            else
            {
                return false;
            }

            if (fileBanned.Contains(cn))
            {
                return true;
            }

            return false;
        }
    }
}