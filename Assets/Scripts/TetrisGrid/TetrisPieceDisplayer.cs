using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TetrisGrid;
using UnityEngine;

public class TetrisPieceDisplayer : MonoBehaviour
{
    public TetrisTile tileRendererObject;
    public Vector2 cellSize;
    public bool overrideSprite;
    public Sprite spriteOverride;
    public bool shouldCentreTiles = false;

    private List<TetrisTile> renderers = new List<TetrisTile>();

    private TetrisBlockState cstate;
    private int crot;

    public void UpdateSpriteOverride(Sprite sprite)
    {
        spriteOverride = sprite;
        ApplyNewPieceState(cstate, crot);
    }

    public void SetPosition(Vector3 pos)
    {
        this.transform.position = new Vector3(pos.x, pos.y, this.transform.position.z);
    }

    public void ApplyNewPieceState(TetrisBlockState state, int rotation)
    {
        cstate = state;
        crot = rotation;

        ApplyEmptyState();

        Vector2Int[] offsets = TetrisBlockRotationTable.GetTableFor(state.type, rotation);
        
        Vector2 centringOffset = Vector2.zero;


        if (shouldCentreTiles)
        {
            centringOffset = TetrisBlockRotationTable.GetCentreOffset(state.type);
        }

        for(int i = 0; i < offsets.Length; i++)
        {
            TetrisTile tile = Instantiate(tileRendererObject, this.transform);
            tile.InitTile((Vector2)(offsets[i] * cellSize) - centringOffset);
            tile.SetState(state.type, state.colours[i > state.colours.Length - 1 ? state.colours.Length - 1 : i]);
            if(overrideSprite)
            {
                tile.SpriteOverride(spriteOverride);
            }
            renderers.Add(tile);
        }
    }

    public void ApplyEmptyState()
    {
        for(int i = 0; i < renderers.Count; i++)
        {
            Destroy(renderers[i].gameObject);
        }

        renderers.Clear();
    }
}
