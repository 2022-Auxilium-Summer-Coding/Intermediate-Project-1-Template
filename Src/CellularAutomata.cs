using CellularAutomata.Events;
using CellularAutomata.Utils;
using CellularAutomata.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CellularAutomata
{
    public class CellularAutomata : Game
    {
        protected GraphicsDeviceManager Graphics;
        protected SpriteBatch SpriteBatch = null!;
        protected Camera Camera = null!;
        protected GameWorld GameWorld = null!;

        private bool _paused = true;

        private readonly int _worldWidth;
        private readonly int _worldHeight;
        private readonly int _cellSize;
        private readonly int _gridBorderSize;

        public CellularAutomata(int worldWidth, int worldHeight, int cellSize, int gridBorderSize = 0)
        {
            _worldWidth = worldWidth;
            _worldHeight = worldHeight;
            _cellSize = cellSize;
            _gridBorderSize = gridBorderSize;
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            GameWorld = new GameWorld(GraphicsDevice, _worldWidth, _worldHeight, _cellSize, _gridBorderSize);
            Camera = new Camera();
        }

        protected override void Update(GameTime gameTime)
        {
            if (!_paused)
            {
                GameWorld.PerformCellUpdate();
            }
            Camera.Controls();

            var ms = Mouse.GetState();
            if (ms.LeftButton == ButtonState.Pressed)
            {
                var mp = Camera.ScreenToWorldPos(ms.Position.ToVector2());
                GameWorld.GetCellPosFromWorldPos(mp, out var x, out var y);
                WorldEvents.OnRequestSpawnCell(x, y, GameWorld);
            } 
            else if (ms.RightButton == ButtonState.Pressed)
            {
                var mp = Camera.ScreenToWorldPos(ms.Position.ToVector2());
                GameWorld.GetCellPosFromWorldPos(mp, out var x, out var y);
                WorldEvents.OnRequestRemoveCell(x, y, GameWorld);
            }

            var ks = Keyboard.GetState();
            if (ks.IsKeyDown(Keys.Space))
            {
                _paused = !_paused;
            }
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(25, 25, 25, 255));
            SpriteBatch.Begin(transformMatrix: Camera.GetTransform());
            GameWorld.RenderWorld(SpriteBatch);
            SpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
