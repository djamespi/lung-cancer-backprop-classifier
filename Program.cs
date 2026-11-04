using System;
using System.Windows.Forms;
using BackProp.HyperparameterTuning;

namespace BackProp
{
    internal static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length == 2 && args[0] == "--tune")
            {
                try
                {
                    AutoTuner.RunGridSearch(args[1]);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex);
                    Environment.ExitCode = 1;
                }
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new Form1());
        }
    }
}