using System;
using System.Collections;
using CellularAutomata.Cells;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CellularAutomata.World;

public class GameWorld
{
    private readonly Cell?[,] _cells;
    private readonly IEnumerator?[,] _cellsUpdater;
    private readonly int _width;
    private readonly int _height;
    private readonly int _cellSize;
    private readonly int _gridBorderSize;
        
    public readonly Texture2D CellTexture;
        
    public GameWorld(GraphicsDevice graphicsDevice, int width, int height, int cellSize, int gridBorderSize)
    {
        _cells = new Cell[width, height];
        _cellsUpdater = new IEnumerator[width, height];
        _width = width;
        _height = height;
        _cellSize = cellSize;
        _gridBorderSize = gridBorderSize;
        CellTexture = new Texture2D(graphicsDevice, cellSize, cellSize);
        var colors = new Color[cellSize * cellSize];
        Array.Fill(colors, Color.White);
        CellTexture.SetData(colors);
    }

    public void PerformCellUpdate()
    {
        for (var i = 0; i < _width; i++)
        {
            for (var j = 0; j < _height; j++)
            {
                var cell = _cells[i, j];
                if (cell is null)
                {
                    if (!(_cellsUpdater[i, j] is null))
                    {
                        _cellsUpdater[i, j] = null;
                    }
                    continue;
                }
                _cellsUpdater[i, j] ??= cell.Update(this, i, j);
                if (!_cellsUpdater[i, j]!.MoveNext())
                {
                    _cellsUpdater[i, j] = null;
                }
            }
        }
    }

    public void RenderWorld(SpriteBatch spriteBatch)
    {
        for (var i = 0; i < _width; i++)
        {
            for (var j = 0; j < _height; j++)
            {
                var cell = _cells[i, j];
                if (cell is null) continue;
                var x = i * _cellSize + _gridBorderSize * i;
                var y = j * _cellSize + _gridBorderSize * j;
                spriteBatch.Draw(CellTexture, new Vector2(x, y), cell.CellColor());
            }
        }
    }
        
    public void SetCell(int x, int y, Cell? cell)
    {
        if (!IsInBorder(x, y))
        {
            return;
        }
        _cells[x, y] = cell;
    }

    public bool TryMove(Cell cell, int currX, int currY, int x, int y)
    {
        if (!IsValidPosition(x, y)) return false;
        Move(cell, currX, currY, x, y);
        return true;
    }

    public void Move(Cell cell, int currX, int currY, int x, int y)
    {
        SetCell(currX, currY, null);
        SetCell(x, y, cell);
    }

    public Cell? GetCellAt(int x, int y)
    {
        return !IsInBorder(x, y) ? null : _cells[x, y];
    }

    private bool IsValidPosition(int x, int y)
    {
        return IsInBorder(x, y) && GetCellAt(x, y) is null;
    }

    private bool IsInBorder(int x, int y)
    {
        return x >= 0 && y >= 0 && x < _width && y < _height;
    }

    public void GetCellPosFromWorldPos(Vector2 vector2, out int x, out int y)
    {
        x = (int) (vector2.X / (_cellSize + _gridBorderSize));
        y = (int) (vector2.Y / (_cellSize + _gridBorderSize));
    }
}