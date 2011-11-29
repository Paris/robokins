using System.Collections.Generic;
using System.IO;

namespace robokins.IRC
{
    partial class Client
    {
        string Concat(IEnumerable<string> value, string seperator = ",", TextWriter output = null)
        {
            bool returns = output == null;

            if (returns)
                output = new StringWriter();

            bool first = true;

            foreach (var part in value)
            {
                if (first)
                    first = false;
                else
                    output.Write(seperator);
                output.Write(part);
            }

            return returns ? output.ToString() : null;
        }
    }
}
