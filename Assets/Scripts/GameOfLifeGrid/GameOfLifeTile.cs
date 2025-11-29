using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOfLifeTile : GridTile
{

    private SpriteRenderer render;

    [System.NonSerialized]
    public bool isAlive = true;
    private bool nextIsAlive = true;

    public override void InitTile(GridController controller, Vector2Int index, Vector2 position)
    {
        base.InitTile(controller, index, position);
	    render = GetComponent<SpriteRenderer>();
    }

    public override void CalcState()
    {
		int aliveCount = -1;

        for(int i = -1; i <= 1; i++)
		{
			for(int j = -1; j <= 1; j++)
			{
				GridTile neighbour = getNeighbour(new Vector2Int(i, j));

				if (neighbour != null)
				{
					if(((GameOfLifeTile)neighbour).isAlive) aliveCount++;
				}
			}
		}

		if (aliveCount < 2 || aliveCount > 3) nextIsAlive = false;
		if(aliveCount == 3 || (aliveCount > 0 && Random.Range(0f, 1f) > 0.85)) nextIsAlive = true;


	}

    public override void ApplyState()
    {
		isAlive = nextIsAlive;
		render.enabled = isAlive;
    }
}
