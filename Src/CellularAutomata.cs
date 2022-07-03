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
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch = null!;
        private Camera _camera = null!;
        private GameWorld _gameWorld = null!;

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
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _gameWorld = new GameWorld(GraphicsDevice, _worldWidth, _worldHeight, _cellSize, _gridBorderSize);
            _camera = new Camera();
        }

        protected override void Update(GameTime gameTime)
        {
            if (!_paused)
            {
                _gameWorld.PerformCellUpdate();
            }
            _camera.Controls();

            var ms = Mouse.GetState();
            if (ms.LeftButton == ButtonState.Pressed)
            {
                var mp = _camera.ScreenToWorldPos(ms.Position.ToVector2());
                _gameWorld.GetCellPosFromWorldPos(mp, out var x, out var y);
                WorldEvents.OnRequestSpawnCell(x, y, _gameWorld);
            } 
            else if (ms.RightButton == ButtonState.Pressed)
            {
                var mp = _camera.ScreenToWorldPos(ms.Position.ToVector2());
                _gameWorld.GetCellPosFromWorldPos(mp, out var x, out var y);
                WorldEvents.OnRequestRemoveCell(x, y, _gameWorld);
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
            _spriteBatch.Begin(transformMatrix: _camera.GetTransform());
            _gameWorld.RenderWorld(_spriteBatch);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
