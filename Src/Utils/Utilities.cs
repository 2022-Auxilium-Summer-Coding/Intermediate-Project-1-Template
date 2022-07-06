using System;

namespace CellularAutomata.Utils;

public class Utilities
{
    public static bool Chance(float chance) {
        var rand = new Random();
        var randN = rand.NextDouble();
        return randN < chance;
    }
}