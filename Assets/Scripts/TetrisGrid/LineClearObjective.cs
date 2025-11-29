using System;
using System.Collections.Generic;
using TetrisGrid;
public class LineClearObjective
{
    public List<TetrisTileColour> colourRequirements;

    public LineClearObjective(List<TetrisTileColour> requirements)
    {
        colourRequirements = requirements;
    }

    public bool CheckObjective(List<TetrisTileColour> blocks)
    {
        foreach(TetrisTileColour col in colourRequirements)
        {
            if (!blocks.Contains(col)) return false;
        }
        return true;
    }
}
