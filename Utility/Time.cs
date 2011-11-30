using System;
using System.Text;

namespace robokins.Utility
{
    class Time
    {
        static readonly string[] Locations = new string[] { "UK", "Europe", "US Central", "US Eastern", "Brazil", "Japan", "Australia" };
        static readonly int[] LocationOffsets = new int[] { 0, 1, -6, -5, -3, 9, 10 };

        public static string WorldTime()
        {
            StringBuilder txt = new StringBuilder();
            txt.Append("Times: ");

            int count = Math.Min(Locations.Length, LocationOffsets.Length);

            for (int i = 0; i < count; i++)
            {
                txt.Append(Font.Bold);
                txt.Append(Locations[i]);
                txt.Append(Font.Bold);
                txt.Append(' ');

                DateTime now = DateTime.Now.ToUniversalTime();
                if (DateTime.Now.IsDaylightSavingTime())
                    now = now.AddHours(1);
                now = now.AddHours(LocationOffsets[i]);

                int hour = now.Hour % 12;
                hour = hour == 0 ? 12 : hour;

                txt.Append(hour.ToString());
                txt.Append(':');
                txt.Append(now.Minute.ToString().PadLeft(2, '0'));
                txt.Append(now.Hour > 12 ? 'p' : 'a');
                txt.Append('m');

                if (i + 1 < count)
                    txt.Append(" - ");
            }

            return txt.ToString();
        }
    }
}
