using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using CellularAutomata.Cells;
using CellularAutomata.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CellularAutomata.World;

public class GameWorld
{
    private readonly Cell?[,] _cells;
    private readonly IEnumerator?[,] _cellsUpdater;
    private readonly Queue<(int, int)> _cellsToRemove;
    
    private readonly int _width;
    private readonly int _height;
    private readonly int _cellSize;
    private readonly int _gridBorderSize;
        
    public readonly Texture2D CellTexture;
    
    public GameWorld(GraphicsDevice graphicsDevice, int width, int height, int cellSize, int gridBorderSize)
    {
        _cells = new Cell[width, height];
        _cellsUpdater = new IEnumerator[width, height];
        _cellsToRemove = new Queue<(int, int)>();
        
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
                if (cell is null) continue;
                _cellsUpdater[i, j] ??= cell.Update(this, i, j);
                if (!_cellsUpdater[i, j]!.MoveNext())
                {
                    _cellsUpdater[i, j] = null;
                }
            }
        }

        for (var i = 0; i < _cellsToRemove.Count; i++)
        {
            var (x, y) = _cellsToRemove.Dequeue();
            _cells[x, y] = null;
            _cellsUpdater[x, y] = null;
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

    public void RemoveCell(int x, int y)
    {
        if (!IsInBorder(x, y)) return;
        if (GetCellAt(x, y) is null) return;
        _cellsToRemove.Enqueue((x, y));
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

    public bool TryMoveToward(Cell cell, int currX, int currY, int dirX, int dirY, int maxTravelDistance)
    {
        throw new NotImplementedException();

        // dirX = Math.Clamp(dirX, -1, 1);
        // dirY = Math.Clamp(dirY, -1, 1);
        //
        // var destX = currX;
        // var destY = currY;
        //
        // for (var i = 1; i <= maxTravelDistance; i++)
        // {
        //     var x = currX + dirX * i;
        //     var y = currY + dirY * i;
        //     if (!IsValidPosition(x, y)) break;
        //     destX = x;
        //     destY = y;
        // }
        //
        // if (destX == currX || destY == currY) return false;
        //
        // Move(cell, currX, currY, destX, destY);
        // return true;
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