#if PASTE

using System.IO;
using System.Timers;

namespace robokins
{
    partial class Bot
    {
        Timer paste;
        DirectoryInfo pasteDir;

        void PasteCheck(object sender, ElapsedEventArgs e)
        {
            foreach (FileInfo file in pasteDir.GetFiles())
            {
                string id = file.Name, nick = File.ReadAllText(file.FullName), info = string.Empty;

                int z = nick.IndexOf(' ');
                if (z != -1)
                {
                    info = string.Concat(" - ", nick.Substring(z + 1));
                    nick = nick.Substring(0, z);
                }

                Message(Channel, string.Format("{0} pasted {1}{2}{3}", new string[] { nick, PasteURI, id, info }));
                file.Delete();
            }
        }
    }
}

#endif
