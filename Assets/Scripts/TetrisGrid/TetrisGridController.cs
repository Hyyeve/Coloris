using System.Collections;
using System.Collections.Generic;
using TetrisGrid;
using UnityEngine;

public class TetrisGridController : MonoBehaviour
{
	public Vector2 cellSize;
    public Vector2Int gridCells;

    public TetrisTile gridTileObject;
    
    private TetrisTile[,] gridTiles;

    public bool warped;
    public int warps;

    public void Start()
    {
        gridTiles = new TetrisTile[gridCells.x, gridCells.y];
        for(int i = 0; i < gridCells.x; i++)
        {
            for(int j = 0; j < gridCells.y; j++)
            {
                TetrisTile newTile = Instantiate(gridTileObject, this.transform);

				Vector2Int index = new Vector2Int(i, j);
				Vector2 position = new Vector2(index.x, index.y) * cellSize;

				newTile.InitTile(position);

				gridTiles[i, j] = newTile;

            }
        }
        if (warped) WarpGrid();
    }

    public void WarpGrid()
    {
        for(int i = 0; i < warps; i++)
        {
            int r1 = Random.Range(0, gridCells.x);
            int r11 = Random.Range(0, gridCells.y);
            int r2 = Random.Range(0, gridCells.x);
            int r22 = Random.Range(0, gridCells.y);

            Vector3 temp = ((TetrisTile)(gridTiles[r1,r11])).transform.position;

            ((TetrisTile)(gridTiles[r1, r11])).transform.position = ((TetrisTile)(gridTiles[r2, r22])).transform.position;

            ((TetrisTile)(gridTiles[r2, r22])).transform.position = temp;

        }
    }

    public bool IsValidCell(Vector2Int index)
    {
	    return index.x >= 0 && index.x < gridCells.x && index.y >= 0 && index.y < gridCells.y;
    }
    
    public TetrisTile GetCell(Vector2Int index)
	{
		return gridTiles[index.x, index.y];
	}

    public void SetCellState(Vector2Int index, TetrisTileType type, TetrisTileColour colour)
    {
	    gridTiles[index.x, index.y].SetState(type, colour);
    }

    public void ClearRow(int row)
    {
        if(!IsValidCell(new Vector2Int(0, row))) return;
        for(int i = 0; i < gridCells.x; i++)
        {
            SetCellState(new Vector2Int(i, row), TetrisTileType.Empty, TetrisTileColour.None);
        }
    }

    public void FillGarbageRow(int row)
    {
        if (!IsValidCell(new Vector2Int(0, row))) return;
        for (int i = 0; i < gridCells.x; i++)
        {
            SetCellState(new Vector2Int(i, row), TetrisTileType.Garbage, TetrisTileColour.Garbage);
        }
    }

    public void FillRandomGarbageRow(int row, int maxCount)
    {
        if (!IsValidCell(new Vector2Int(0, row))) return;

        ClearRow(row);

        for (int i = 0; i < maxCount; i++)
        {
            int rand = Random.Range(0, gridCells.x);
            SetCellState(new Vector2Int(rand, row), TetrisTileType.Garbage, TetrisTileColour.Garbage);
        }
    }

    public void ShiftDownFrom(int row)
    {
        if (!IsValidCell(new Vector2Int(0, row))) return;

        for(int y = row; y < gridCells.y; y++)
        {
            for(int x = 0; x < gridCells.x; x++)
            {
                Vector2Int index = new Vector2Int(x, y);
                Vector2Int offset = Vector2Int.up;
                if (IsValidCell(index + offset))
                {
                    TetrisTile tile = GetCell(index + offset);
                    SetCellState(index, tile.GetState(), tile.GetColour());
                }
                else
                {
                    SetCellState(index, TetrisTileType.Empty, TetrisTileColour.None);
                }
            }
        }
    }

    public void ShiftUpTo(int row)
    {
        if (!IsValidCell(new Vector2Int(0, row))) return;

        for (int y = row; y > 0; y--)
        {
            for (int x = 0; x < gridCells.x; x++)
            {
                Vector2Int index = new Vector2Int(x, y);
                Vector2Int offset = Vector2Int.down;
                if (IsValidCell(index + offset))
                {
                    TetrisTile tile = GetCell(index + offset);
                    SetCellState(index, tile.GetState(), tile.GetColour());
                }
                else
                {
                    SetCellState(index, TetrisTileType.Empty, TetrisTileColour.None);
                }
            }
        }
    }

    public IEnumerator BurstLine(int row, bool side)
    {
        if (!IsValidCell(new Vector2Int(0, row))) yield break;
        for(int i = 0; i < gridCells.x; i++)
        {
            GetCell(new Vector2Int(i, row)).RunBurst(side);
        }
        yield return new WaitForSeconds(0.25f);
    }

    public void ClearColumn(int col)
    {
        if (!IsValidCell(new Vector2Int(col, 0))) return;
        for (int i = 0; i < gridCells.y; i++)
        {
            SetCellState(new Vector2Int(col, i), TetrisTileType.Empty, TetrisTileColour.None);
        }
    }

    public Vector2 FromGrid(Vector2Int gridPosition)
    {
        return (Vector2)(transform.position) + cellSize * gridPosition;
    }

}
