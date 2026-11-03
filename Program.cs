using System;
using System.Windows.Forms;
using BackProp.HyperparameterTuning;

namespace BackProp
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // To run the automated tuning, UNCOMMENT the next two lines:
            // AutoTuner.RunGridSearch(); 
            // return;

            // Otherwise, it runs the standard UI
            Application.Run(new Form1());
        }
    }
}