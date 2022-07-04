using System;
using System.Collections.Generic;
using CellularAutomata.Cells;
using CellularAutomata.Events;
using CellularAutomata.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Color = Microsoft.Xna.Framework.Color;

namespace CellularAutomata.Example;

public class CellHandler
{
    private List<Cell> _cells;
    private int _selectedCell;

    private readonly Texture2D _slotTexture;
    private readonly Texture2D _cellTexture;

    public CellHandler(GraphicsDevice graphicsDevice)
    {
        _cells = new List<Cell>();
        _selectedCell = 0;

        CellEvents.OnRegisterCellTypes(_cells);
        
        var slotDimension = 48;
        var colors = new Color[slotDimension * slotDimension];

        _slotTexture = new Texture2D(graphicsDevice, slotDimension, slotDimension);
        
        for (var j = 0; j < slotDimension; j++)
        {
            for (var i = 0; i < slotDimension; i++)
            {
                if (j == 0 || j == slotDimension - 1)
                {
                    if (i == 0 || i == slotDimension - 1) continue;
                    colors[slotDimension * j + i] = Color.White;
                }
                else
                {
                    if (i == 0 || i == slotDimension - 1)
                    {
                        colors[slotDimension * j + i] = Color.White;
                        continue;
                    }
                    colors[slotDimension * j + i] = Color.Gray;
                }
            }
        }
        _slotTexture.SetData(colors);

        var size = (int) (slotDimension * 0.7);
        _cellTexture = new Texture2D(graphicsDevice, size, size);
        var cellColors = new Color[size * size];
        Array.Fill(cellColors, Color.White);
        _cellTexture.SetData(cellColors);
        
        WorldEvents.RemoveDefaultHandlers();
        WorldEvents.RequestRemoveCell += OnRemoveCell;
        WorldEvents.RequestSpawnCell += OnSpawnCell;
    }

    public void DrawHud(SpriteBatch batch)
    {
        batch.Begin();
        
        var x = 10f;
        var y = 10;
        var slotWidth = _slotTexture.Width;
        var cellBorder = slotWidth * 0.3f / 2;
        
        for (var i = 0; i < _cells.Count; i++)
        {
            batch.Draw(_slotTexture, new Vector2(x, y), Color.White);
            batch.Draw(_cellTexture, new Vector2(x + cellBorder, y + cellBorder), _cells[i].CellColor());

            var ms = Mouse.GetState();
            var mp = ms.Position.ToVector2();
            if (mp.X > x && mp.X < x + slotWidth && mp.Y > y && mp.Y < y + slotWidth)
            {
                if (ms.LeftButton == ButtonState.Pressed)
                {
                    _selectedCell = i;
                }
            }
            
            x += slotWidth * 1.4f;
        }

        batch.End();
    }

    private static void OnRemoveCell(int x, int y, GameWorld world)
    {
        world.RemoveCell(x, y);
    }

    private void OnSpawnCell(int x, int y, GameWorld world)
    {
        var rand = new Random();
        
        for (var i = -9; i < 9; i++)
        {
            for (var j = -9; j < 9; j++)
            {
                if (rand.Next() % 2 == 0)
                {
                    world.SetCell(x + i, y + j, _cells[_selectedCell].Clone());
                }
            }
        }
    }
}