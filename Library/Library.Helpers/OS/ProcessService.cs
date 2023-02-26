using System.Diagnostics;
using System.IO;

namespace Library.Helpers.OS
{
    public class ProcessService
    {
        public object Run(string fileName, string arguments)
        {
            try
            {
                var process = new Process();
                process.StartInfo = new ProcessStartInfo
                {
                    FileName = fileName,
                    Arguments = arguments
                };

                process.StartInfo.RedirectStandardOutput = true;
                process.Start();
                process.WaitForExit();
                StreamReader reader = process.StandardOutput;
                var outputMessage = reader.ReadToEnd();

                return outputMessage;
            }
            catch (System.Exception ex)
            {
                return ex.Message;
            }
        }
    }

}
