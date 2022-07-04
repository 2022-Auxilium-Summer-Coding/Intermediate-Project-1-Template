using System.Collections;
using CellularAutomata.World;
using Microsoft.Xna.Framework;

namespace CellularAutomata.Cells;

public abstract class Cell
{
    public virtual IEnumerator Update(GameWorld world, int thisX, int thisY)
    {
        // wait for 1 frames before the next update
        for (var i = 0; i < CellularAutomata.UpdateDelay; i++)
        {
            yield return null;
        }
    }

    public abstract Color CellColor();

    public abstract Cell Clone();

    public static bool operator ==(Cell a, Cell b)
    {
        return a.GetType() == b.GetType();
    }

    public static bool operator !=(Cell a, Cell b)
    {
        return !(a == b);
    }
    
    protected bool Equals(Cell other)
    {
        return GetType() == other.GetType();
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Cell) obj);
    }

    public override int GetHashCode()
    {
        return GetType().GetHashCode();
    }
}