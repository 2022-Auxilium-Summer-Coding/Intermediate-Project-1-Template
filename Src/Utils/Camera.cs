using System;
using System.Numerics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace CellularAutomata.Utils;

public class Camera
{
    private Matrix _transform;
    private float _zoom;
    private Vector2 _position;

    public Camera()
    {
        _transform = Matrix.Identity;
        _zoom = 1;
        _position = Vector2.Zero;
        UpdateTransform();
    }

    public void Controls()
    {
        var state = Keyboard.GetState();
        var direction = Vector2.Zero;
            
        if (state.IsKeyDown(Keys.W))
        {
            direction += new Vector2(0, 1);
        }
            
        if (state.IsKeyDown(Keys.S))
        {
            direction += new Vector2(0, -1);
        }
            
        if (state.IsKeyDown(Keys.D))
        {
            direction += new Vector2(-1, 0);
        }
            
        if (state.IsKeyDown(Keys.A))
        {
            direction += new Vector2(1, 0);
        }
            
        if (direction != Vector2.Zero)
        {
            direction.Normalize();
            if (state.IsKeyDown(Keys.LeftShift))
            {
                direction *= 40;
            }
            direction *= 4 - _zoom;
            Move(direction);
        }
            
        var zoomLevel = 0f;
            
        if (state.IsKeyDown(Keys.Q))
        {
            zoomLevel += 0.01f;
        }

        if (state.IsKeyDown(Keys.E))
        {
            zoomLevel -= 0.01f;
        }

        if (zoomLevel != 0)
        {
            Zoom(zoomLevel);
        }
    }
        
    private void Zoom(float zoom)
    {
        _zoom += zoom;
        _zoom = Math.Clamp(_zoom, 0.05f, 4f);
        UpdateTransform();
    }

    private void Move(Vector2 direction)
    {
        _position += direction;
        UpdateTransform();
    }

    private void UpdateTransform()
    {
        _transform = Matrix.CreateTranslation(_position.X, _position.Y, 1) *
                     Matrix.CreateScale(_zoom);
    }

    public Vector2 ScreenToWorldPos(Vector2 vector2)
    {
        var transform = Matrix.Invert(_transform);
        return Vector2.Transform(vector2, transform);
    }
        
    public Matrix GetTransform()
    {
        return _transform;
    }
}