using System;
using System.Windows.Forms;

namespace Digger
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        { 
            var game = Game.Create();
            Application.Run(new DiggerWindow(game));
        }
    }
}