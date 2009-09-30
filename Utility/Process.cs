using Diagnostics = System.Diagnostics;

namespace robokins.Utility
{
    class Process
    {
        public static string Output(string Command) { return Output(Command, null); }

        public static string Output(string Command, string Arguments)
        {
            var prc = new Diagnostics.Process();
            prc.StartInfo.UseShellExecute = false;
            prc.StartInfo.RedirectStandardOutput = true;
            prc.StartInfo.FileName = Command;
            if (!string.IsNullOrEmpty(Arguments))
                prc.StartInfo.Arguments = Arguments;
            prc.Start();
            string output = prc.StandardOutput.ReadToEnd();
            prc.WaitForExit();
            prc.Close();
            return output;
        }
    }
}
