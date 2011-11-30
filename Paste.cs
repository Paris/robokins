using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Timers;
using System.Xml;

namespace robokins
{
    partial class Bot
    {
        List<string> pasteIds;

        [Conditional("PASTE")]
        void PasteSetup()
        {
            pasteIds = new List<string>();
            var timer = new Timer(PasteFreq);
            var local = Directory.Exists(PasteSync);
            timer.Elapsed += local ? new ElapsedEventHandler(pasteDirectoryCheck) : new ElapsedEventHandler(pasteFeedCheck);

            if (!local)
            {
                foreach (var item in pasteFetch().Keys)
                    pasteIds.Add(item);
            }

            Quitting += new System.EventHandler(delegate(object sender, EventArgs e)
            {
                if (timer.Enabled)
                    timer.Stop();
            });

            timer.Start();
        }

        void pasteDirectoryCheck(object sender, ElapsedEventArgs e)
        {
            foreach (var file in Directory.GetFiles(PasteSync, "*", SearchOption.TopDirectoryOnly))
            {
                string path = Path.Combine(PasteSync, file), id = file, nick = File.ReadAllText(path), info = string.Empty;

                int z = nick.IndexOf(' ');
                if (z != -1)
                {
                    info = nick.Substring(z + 1);
                    nick = nick.Substring(0, z);
                }

                pasteMessage(nick, PasteURL + id, info);
                File.Delete(path);
            }
        }

        void pasteFeedCheck(object sender, ElapsedEventArgs e)
        {
            var list = pasteFetch();

            foreach (var item in list.Keys)
            {
                if (pasteIds.Contains(item))
                    continue;

                pasteMessage(list[item][0], item, list[item][1]);
                pasteIds.Add(item);
            }
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

        void pasteMessage(string nick, string id, string info)
        {
            if (!string.IsNullOrEmpty(info))
                info = " - " + info;
            Message(Client, Channel, string.Format("{0} pasted {1}{2}", new string[] { nick, id, info }));
        }
    }
}
