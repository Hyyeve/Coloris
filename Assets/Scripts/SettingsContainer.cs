
using UnityEngine;

public static class SettingsContainer
{
    public static BlockStyle blockStyle = BlockStyle.Buttons;

    public static Difficulty difficulty = Difficulty.Normal;

    public static bool randomBlockStyle = true;

    public static void TickRandomiser()
    {
        if (randomBlockStyle)
        {
            var style = RandomStyle();
            if (blockStyle == style) TickRandomiser();
            else blockStyle = style;
        }
    }


    private static BlockStyle RandomStyle()
    {
        var index = Random.Range(0, 3);
        switch (index)
        {
            case 0:
                return BlockStyle.Buttons;
            case 1:
                return BlockStyle.Flat;
            case 2:
                return BlockStyle.Puyos;
            default:
                return BlockStyle.Buttons;
        }
    }
}

public enum Difficulty
{
    Easy = 0,
    Normal = 1,
    Tricky = 2,
}

public enum BlockStyle
{
    Buttons = 0,
    Puyos = 1,
    Flat = 2,
    Cursed = 3,
}