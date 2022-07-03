using System.Collections;
using Microsoft.Xna.Framework;

namespace CellularAutomata.World
{
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
    }
}