using CellularAutomata.Cells;
using CellularAutomata.Utils;
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
        
        public CellularAutomata()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _camera = new Camera();
            _gameWorld = new GameWorld(GraphicsDevice, 64, 64, 12, 0);
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
                _gameWorld.SetCell(x, y, new SandCell());
            } 
            else if (ms.RightButton == ButtonState.Pressed)
            {
                var mp = _camera.ScreenToWorldPos(ms.Position.ToVector2());
                _gameWorld.GetCellPosFromWorldPos(mp, out var x, out var y);
                _gameWorld.SetCell(x, y, null);
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
