using System.Collections;
using Microsoft.Xna.Framework;

namespace CellularAutomata.Cells
{
    // ReSharper disable once InconsistentNaming
    public class SandCell : ICell
    {
        public IEnumerator Update(GameWorld world, int thisX, int thisY)
        {
            world.TryMove(this, thisX, thisY, thisX, thisY + 1);
            yield return null;
            yield return null;
            yield return null;
            yield return null;
        }

        public Color CellColor()
        {
            return Color.Yellow;
        }
    }
}