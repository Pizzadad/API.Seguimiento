using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Logger
{
    public static class LocalExceptionLogger
    {
        public static void EscribirLog(Exception ex)
        {
            if (ex == null) return;
            try
            {
                var sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFile.txt", true);
                sw.WriteLine(string.Format("{0}: {1}; {2}", "-Ocurrió una excepcion-", DateTime.Now, ex));
                sw.Flush();
                sw.Close();
            }
            catch
            {
                // ignored
            }
        }
    }
}
