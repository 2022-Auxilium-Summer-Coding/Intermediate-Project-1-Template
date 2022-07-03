using System;

namespace CellularAutomata
{
    public static class Program
    {
        [STAThread]
        private static void Main()
        {
            using var game = new CellularAutomata();
            game.Run();
        }
    }
}
