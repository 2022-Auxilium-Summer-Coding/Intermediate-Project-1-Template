using System;

namespace CellularAutomata;

public class Application
{
    [STAThread]
    public static void Main()
    {
        using var game = new CellularAutomata(128, 128, 6, 2);
        game.Run();
    }
}