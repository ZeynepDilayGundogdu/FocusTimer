using System;
using System.Windows.Forms;

namespace Productivity
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Önce LoginForm göster
            LoginForm login = new LoginForm();

            if (login.ShowDialog() == DialogResult.OK)
            {
                // Eğer kullanıcı doğru login yaptıysa Form1 aç
                Application.Run(new Form1());
            }
            else
            {
                // Yanlışsa direkt çık
                Application.Exit();
            }
        }
    }
}
