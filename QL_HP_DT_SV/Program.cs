using System;
using System.Windows.Forms;


namespace QL_HP_DT_SV
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Login.LoginForm());
        }
    }
}
