using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{

	public bool reset = false;

    public Vector2 cellSize;
    public Vector2Int gridCells;

    public GridTile gridTileObject;

	public bool autoUpdateGrid;
	public float autoUpdateUPS;


    private GridTile[,] gridTiles;

    public void Start()
    {
        gridTiles = new GridTile[gridCells.x, gridCells.y];
        for(int i = 0; i < gridCells.x; i++)
        {
            for(int j = 0; j < gridCells.y; j++)
            {
                GridTile newTile = Instantiate(gridTileObject, this.transform);

				Vector2Int index = new Vector2Int(i, j);
				Vector2 position = new Vector2(index.x, index.y) * cellSize;

				newTile.InitTile(this, index, position);

				gridTiles[i, j] = newTile;

            }
        }

		if (autoUpdateGrid) StartCoroutine("AutoUpdateGrid");
    }

    public void Update()
	{
        if(reset)
		{
			reset = false;
			StopCoroutine("AutoUpdateGrid");
			foreach (GridTile tile in gridTiles) Destroy(tile.gameObject);
			Start();
		}
	}

    public IEnumerator AutoUpdateGrid()
	{
        while(true)
		{
			yield return new WaitForSeconds(1f / autoUpdateUPS);

			RunGridUpdate();
		}
	}

    public void RunGridUpdate()
	{
		foreach (GridTile tile in gridTiles)tile.CalcState();
		foreach (GridTile tile in gridTiles)tile.ApplyState();
	}

    public GridTile GetCell(Vector2Int index)
	{
		return gridTiles[index.x, index.y];
	}

}
