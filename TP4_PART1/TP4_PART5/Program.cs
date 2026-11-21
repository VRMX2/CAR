using System;
using System.Windows.Forms;

namespace TP4_PART5
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // ORA-01882 fix: must be set BEFORE any Oracle type is loaded.
            // Fixed UTC offset (Algeria = UTC+01:00) so the driver never sends the
            // Windows region name "W. Central Africa" that the Oracle server doesn't know.
            // If it still fails, try "Africa/Lagos" instead of "+01:00".
            Environment.SetEnvironmentVariable("ORA_SDTZ", "+01:00");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}