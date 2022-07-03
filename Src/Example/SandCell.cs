using System.Collections;
using CellularAutomata.World;
using Microsoft.Xna.Framework;

namespace CellularAutomata.Example
{
    // ReSharper disable once InconsistentNaming
    public class SandCell : ICell
    {
        public IEnumerator Update(GameWorld world, int thisX, int thisY)
        {
            if (!world.TryMove(this, thisX, thisY, thisX, thisY + 1))
            {
                // if can not move directly down
                if (!world.TryMove(this, thisX, thisY, thisX - 1, thisY + 1))
                {
                    // if can not move bottom left
                    if (!world.TryMove(this, thisX, thisY, thisX + 1, thisY + 1))
                    {
                    }
                }
            }
            
            // wait for 4 frames before the next update
            for (var i = 0; i < 4; i++)
            {
                yield return null;
            }
        }

        public Color CellColor()
        {
            return Color.Yellow;
        }
    }
}