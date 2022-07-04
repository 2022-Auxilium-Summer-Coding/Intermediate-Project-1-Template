using System;
using System.Collections.Generic;
using CellularAutomata.Cells;

namespace CellularAutomata.Events;

public class CellEvents
{
    public static event Action<List<Cell>>? RegisterCellTypes;

    public static void OnRegisterCellTypes(List<Cell> registry)
    {
        RegisterCellTypes?.Invoke(registry);
    }
}