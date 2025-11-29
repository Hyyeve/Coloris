namespace TetrisGrid
{
    public enum TetrisTileType
    {
        Special, Empty, Garbage, I, J, L, O, S, Z, T
    }

    public enum TetrisTileColour
    {
        None, Garbage, Cyan, Blue, Orange, Yellow, Green, Red, Purple, Rainbow
    }
    public class TetrisBlockState 
    {
        public TetrisTileType type;
        public TetrisTileColour[] colours;

        public TetrisBlockState(TetrisTileType type, TetrisTileColour[] colours)
        {
            this.type = type;
            this.colours = colours;
        }

        public void Rainbowify()
        {
            colours = new[]
            {
                TetrisTileColour.Rainbow,
            };
        }
    }
}