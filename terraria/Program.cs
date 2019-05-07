using System;
using System.Windows.Forms;

namespace terraria
{
    class MainForm
    {
        public static void Main()
        {
            var form = new Form();
            Application.Run(form);

            var game = new Game();
        }
    }
}
