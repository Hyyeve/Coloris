using UnityEngine;
using TetrisGrid;
using System.Collections.Generic;

public static class TetrisRandomiserSystem
{
    private static List<TetrisTileType> bag7 = new List<TetrisTileType>();

    private static List<TetrisTileType> pool35 = new List<TetrisTileType>();
    private static List<TetrisTileType> pool35history = new List<TetrisTileType>();
    private static List<TetrisTileType> pool35order = new List<TetrisTileType>();

    private static List<TetrisTileColour> colourBiasedRandomHistory = new List<TetrisTileColour>();

    private static List<TetrisTileColour> colourPool15 = new List<TetrisTileColour>();

    public static void ResetRandomisers()
    {
        bag7.Clear();
        pool35.Clear();
        pool35history.Clear();
        colourBiasedRandomHistory.Clear();
        colourPool15.Clear();
    }

    public static TetrisTileType Get7BagRandom()
    {
        if(bag7.Count == 0)
        {
            FillBagOnce(bag7);
        }

        int random = Random.Range(0, bag7.Count);

        TetrisTileType type = bag7[random];
        bag7.RemoveAt(random);

        return type;
    }

    public static TetrisTileType Get35WeightedPoolRandom()
    {
        if (pool35history.Count == 0)
        {
            TetrisTileType p = new[] { TetrisTileType.I, TetrisTileType.J, TetrisTileType.L, TetrisTileType.T }[Random.Range(0, 4)];
            pool35history.AddRange(new[] { TetrisTileType.S, TetrisTileType.Z, TetrisTileType.S, TetrisTileType.Z, p });
            return p;
        }

        if(pool35.Count < 35)
        {
            FillBag5x(pool35);
        }

        TetrisTileType piece = TetrisTileType.Empty;
        int random = 0;

        for (int roll = 0; roll < 5; roll++)
        {
            random = Random.Range(0, 35);
            piece = pool35[random];
            if (!pool35history.Contains(piece) || roll >= 5) break;
            if (pool35order.Count > 0) pool35[random] = pool35order[0];
        }

        if (pool35order.Contains(piece)) pool35order.Remove(piece);
        pool35order.Add(piece);

        pool35[random] = pool35order[0];

        pool35history.RemoveAt(0);
        pool35history.Add(piece);

        return piece;

    }

    public static TetrisTileColour GetBaisedRandomColour()
    {
        TetrisTileColour colour = TetrisTileColour.None;

        for(int i = 0; i < 3; i++)
        {
            colour = new[]
            {
                TetrisTileColour.Blue,
                TetrisTileColour.Cyan,
                TetrisTileColour.Orange,
                TetrisTileColour.Yellow,
                TetrisTileColour.Green,
                TetrisTileColour.Red,
                TetrisTileColour.Purple
            }[Random.Range(0, 7)];

            if (colourBiasedRandomHistory.Contains(colour) && colourBiasedRandomHistory.FindAll(col => col == colour).Count < 3) break;
        }

        colourBiasedRandomHistory.Insert(0, colour);
        if (colourBiasedRandomHistory.Count >= 7) colourBiasedRandomHistory.RemoveAt(6);

        return colour;
    }

    public static LineClearObjective GenerateObjective(List<TetrisBlockState> upcomingBlocks, int complexity)
    {
        List<TetrisTileColour> options = new List<TetrisTileColour>();
        List<TetrisTileColour> results = new List<TetrisTileColour>();
        foreach (TetrisBlockState block in upcomingBlocks) if(block.colours[0] != TetrisTileColour.Rainbow) options.AddRange(block.colours);
        for(int i = 0; i < complexity; i++)
        {
            if (options.Count == 0) break;
            int rand = Random.Range(0, options.Count);
            TetrisTileColour genr = options[rand];
            options.RemoveAt(rand);
            if(!results.Contains(genr)) results.Add(genr);
        }

        return new LineClearObjective(results);
    }

    private static void FillBagOnce(List<TetrisTileType> bag)
    {
        bag.AddRange(new[] { TetrisTileType.I, TetrisTileType.J, TetrisTileType.L, TetrisTileType.O, TetrisTileType.S, TetrisTileType.Z, TetrisTileType.T });
    }

    private static void FillBag5x(List<TetrisTileType> bag)
    {
        FillBagOnce(bag);
        FillBagOnce(bag);
        FillBagOnce(bag);
        FillBagOnce(bag);
        FillBagOnce(bag);
    }
}
