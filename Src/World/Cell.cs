using System.Collections;
using Microsoft.Xna.Framework;

namespace CellularAutomata.World
{
    public interface ICell
    {
        public IEnumerator Update(GameWorld world, int thisX, int thisY);

        public Color CellColor();

        public ICell Clone();
    }
}