using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using robokins.Properties;

namespace robokins.Utility
{
    class Manual
    {
        const string Website = "http://www.autohotkey.com/";

        static Dictionary<string, string> index = null;

        public static string Lookup(string item)
        {
            CheckIndex();
            string key = item = NormaliseKey(item);
            return index.ContainsKey(key) ? Website + index[key] : null;
        }

        static string NormaliseKey(string term)
        {
            string key = term;
            key = key.ToLowerInvariant();
            key = Regex.Replace(key, "[^a-z ]", string.Empty);
            key = key.Trim();
            return key;
        }

        static void CheckIndex()
        {
            if (index != null)
                return;

            index = new Dictionary<string, string>();

            var reader = new StringReader((string)Resources.Manual);
            string line;
            string key = null;

            while ((line = reader.ReadLine()) != null)
            {
                string part;
                line = line.Trim();

                const string name = "<param name=\"Name\" value=\"";
                const string value = "<param name=\"Local\" value=\"";
                const string end = "\">";

                if (line.StartsWith(name))
                {
                    part = line.Substring(name.Length);
                    part = part.Substring(0, part.Length - end.Length);
                    key = part;
                }
                else if (line.StartsWith(value))
                {
                    part = line.Substring(value.Length);
                    part = part.Substring(0, part.Length - end.Length);

                    if (key != null)
                    {
                        key = NormaliseKey(key);
                        if (key.Length != 0 && !index.ContainsKey(key))
                            index.Add(key, part.Trim());
                    }

                    key = null;
                }
            }

            Resources.ResourceManager.ReleaseAllResources();
        }
    }
}
