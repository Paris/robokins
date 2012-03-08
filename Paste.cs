using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Timers;
using System.Xml;
using robokins.IRC;

namespace robokins
{
    partial class Bot
    {
        public static string PasteSync { get; set; }
        public static string PasteURL { get; set; }

        List<string> pasteIds;

        [Conditional("PASTE")]
        void PasteSetup(Client client)
        {
            if (string.IsNullOrEmpty(PasteURL))
                return;

            pasteIds = new List<string>();
            var timer = new Timer(PasteFreq);
            var local = !string.IsNullOrEmpty(PasteSync) && Directory.Exists(PasteSync);

            timer.Elapsed += new ElapsedEventHandler(delegate(object sender, ElapsedEventArgs e)
            {
                if (local)
                {
                    foreach (var file in new DirectoryInfo(PasteSync).GetFiles())
                    {
                        string id = file.Name, nick = File.ReadAllText(file.FullName), info = string.Empty, item = PasteURL + id;

                        if (pasteIds.Contains(item))
                            continue;

                        int z = nick.IndexOf(' ');
                        if (z != -1)
                        {
                            info = nick.Substring(z + 1);
                            nick = nick.Substring(0, z);
                        }

                        pasteMessage(client, nick, PasteURL + id, info);
                        pasteIds.Add(item);
                        file.Delete();
                    }
                }
                else
                {
                    var list = pasteFetch();

                    foreach (var item in list.Keys)
                    {
                        if (pasteIds.Contains(item))
                            continue;

                        pasteMessage(client, list[item][0], item, list[item][1]);
                        pasteIds.Add(item);
                    }
                }
            });

            if (!local)
            {
                foreach (var item in pasteFetch().Keys)
                    pasteIds.Add(item);
            }

            client.Quitting += new System.EventHandler(delegate(object sender, EventArgs e)
            {
                if (timer.Enabled)
                    timer.Stop();
            });

            timer.Start();
        }

        internal static Dictionary<string, string[]> pasteFetch(int count = 5)
        {
            var src = PasteURL + "feed/?full=0&limit=" + count.ToString();
            var reader = XmlReader.Create(src, new XmlReaderSettings { ProhibitDtd = false });
            var list = new Dictionary<string, string[]>();
            bool entry = false;
            string id = null, author = null, summary = null;

            while (reader.Read())
            {
                if (reader.NodeType != XmlNodeType.Element)
                    continue;

                switch (reader.LocalName)
                {
                    case "id": id = reader.ReadElementContentAsString(); break;
                    case "name": author = reader.ReadElementContentAsString(); break;
                    case "summary": summary = reader.ReadElementContentAsString(); break;
                    case "entry":
                        if (id != null && entry)
                            list.Add(id, new[] { author, summary });
                        id = author = summary = null;
                        entry = true;
                        break;
                }
            }

            reader.Close();

            if (id != null)
                list.Add(id, new[] { author, summary });

            return list;
        }

        void pasteMessage(Client client, string nick, string id, string info)
        {
            if (!string.IsNullOrEmpty(info))
                info = " - " + info;
            client.Private(Channel, string.Format("{0} pasted {1}{2}", new string[] { nick, id, info }));
        }
    }
}
