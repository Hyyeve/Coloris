using System.Collections;
using System.Collections.Generic;
using TetrisGrid;
using UnityEngine;

public class MultiTetrisPieceDisplayer : MonoBehaviour
{
    public Vector2 pieceOffset;

    public TetrisPieceDisplayer prefdisplayer;

    public int maxCount;

    public bool shouldCentreTiles = false;

    private List<TetrisPieceDisplayer> pieceDisplayers = new List<TetrisPieceDisplayer>();

    public void ApplyNewPieceState(List<TetrisBlockState> states)
    {
        ApplyEmptyState();

        for(int i = 0; i < (maxCount > states.Count ? states.Count : maxCount); i ++)
        {
            TetrisPieceDisplayer displayer = Instantiate(prefdisplayer, this.transform);
            displayer.shouldCentreTiles = shouldCentreTiles;
            displayer.transform.position = transform.position + (Vector3)(pieceOffset * i);
            displayer.ApplyNewPieceState(states[i], 0);
            pieceDisplayers.Add(displayer);
        }

    }

    public void ApplySpecialPieceState(List<TetrisTileColour> colours)
    {
        List<TetrisBlockState> states = new List<TetrisBlockState>();
        foreach(TetrisTileColour col in colours)
        {
            states.Add(new TetrisBlockState(TetrisTileType.Special, new[] { col }));
        }
        ApplyNewPieceState(states);
    }

    public void ApplyEmptyState()
    {
        for (int i = 0; i < pieceDisplayers.Count; i++)
        {
            pieceDisplayers[i].ApplyEmptyState();
            Destroy(pieceDisplayers[i]);
        }

        pieceDisplayers.Clear();
    }
}
