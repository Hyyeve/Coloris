using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTile : MonoBehaviour
{
	private GridController owner;
	private Vector2Int gridPosition;

    public virtual void InitTile(GridController controller, Vector2Int index, Vector2 position)
	{
		owner = controller;
		gridPosition = index;
		transform.position = position;
	}

	public virtual void CalcState()
	{

	}

	public virtual void ApplyState()
	{

	}

    public GridTile getNeighbour(Vector2Int offset)
	{
		Vector2Int cell = gridPosition + offset;
		if (cell.x < 0 || cell.x >= owner.gridCells.x || cell.y < 0 || cell.y >= owner.gridCells.y) return null;
		return owner.GetCell(cell);
	}

}
