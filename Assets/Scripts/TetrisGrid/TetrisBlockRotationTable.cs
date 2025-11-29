using System;
using UnityEngine;

namespace TetrisGrid
{
    public static class TetrisBlockRotationTable
    {

        public static int GetRotatedID(bool clockwise, int from)
        {
            int ret;
            if (clockwise)
            {
                if (from == 2) ret = -1;
                else ret = from + 1;
            }
            else
            {
                if (from == -1) ret = 2;
                else ret = from - 1;
            }
            return ret;
        }

        public static int GetRotatedID180(int from)
        {
            return @from switch
            {
                1 => -1,
                2 => 0,
                _ => @from + 2
            };
        }

        public static Vector2 GetCentreOffset(TetrisTileType state)
        {
            switch (state)
            {
                case TetrisTileType.Special:
                case TetrisTileType.Empty:
                case TetrisTileType.Garbage:
                case TetrisTileType.O:
                    return Vector2.zero;
                case TetrisTileType.J:
                case TetrisTileType.L:
                case TetrisTileType.S:
                case TetrisTileType.Z:
                case TetrisTileType.T:
                    return new Vector2(0.5f, 0f);
                case TetrisTileType.I:
                    return new Vector2(0f, 0.5f);
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        public static Vector2Int[] GetRotationKickOffsetsFor(TetrisTileType state, int rotFrom, int rotTo)
        {
            if (state == TetrisTileType.I) return GetRotationKickOffsetsForI(rotFrom, rotTo);
            
            switch (rotFrom)
            {
                case 0 when rotTo == -1:
                    return new[]
                    {
                        new Vector2Int(1, 0),
                        new Vector2Int(1, 1),
                        new Vector2Int(0, -2),
                        new Vector2Int(1, -2),
                    };
                case 0 when rotTo == 1:
                    return new[]
                    {
                        new Vector2Int(-1, 0),
                        new Vector2Int(-1, 1),
                        new Vector2Int(0, -2),
                        new Vector2Int(-1, -2),
                    };
                case 1 when rotTo == 0:
                    return new[]
                    {
                        new Vector2Int(1, 0),
                        new Vector2Int(1, -1),
                        new Vector2Int(0, 2),
                        new Vector2Int(1, 2),
                    };
                case 1 when rotTo == 2:
                    return new[]
                    {
                        new Vector2Int(1, 0),
                        new Vector2Int(1, -1),
                        new Vector2Int(0, 2),
                        new Vector2Int(1, 2),
                    };
                case 2 when rotTo == 1:
                    return new[]
                    {
                        new Vector2Int(-1, 0),
                        new Vector2Int(-1, 1),
                        new Vector2Int(0, -2),
                        new Vector2Int(-1, -2),
                    };
                case 2 when rotTo == -1:
                    return new[]
                    {
                        new Vector2Int(1, 0),
                        new Vector2Int(1, 1),
                        new Vector2Int(0, -2),
                        new Vector2Int(1, -2),
                    };
                case -1 when rotTo == 2:
                    return new[]
                    {
                        new Vector2Int(-1, 0),
                        new Vector2Int(-1, -1),
                        new Vector2Int(0, 2),
                        new Vector2Int(-1, 2),
                    };
                case -1 when rotTo == 0:
                    return new[]
                    {
                        new Vector2Int(-1, 0),
                        new Vector2Int(-1, -1),
                        new Vector2Int(0, 2),
                        new Vector2Int(-1, 2),
                    };
            }

            return new Vector2Int[] { };
        }

        private static Vector2Int[] GetRotationKickOffsetsForI(int rotFrom, int rotTo)
        {
            switch (rotFrom)
            {
                case 0 when rotTo == -1:
                    return new[]
                    {
                        new Vector2Int(-1, 0),
                        new Vector2Int(2, 0),
                        new Vector2Int(2, -1),
                        new Vector2Int(-1, 2),
                    };
                case 0 when rotTo == 1:
                    return new[]
                    {
                        new Vector2Int(1, 0),
                        new Vector2Int(-2, 0),
                        new Vector2Int(-2, -1),
                        new Vector2Int(1, 2),
                    };
                case 1 when rotTo == 0:
                    return new[]
                    {
                        new Vector2Int(-1, 0),
                        new Vector2Int(2, 0),
                        new Vector2Int(-1, -2),
                        new Vector2Int(2, 1), 
                    };
                case 1 when rotTo == 2:
                    return new[]
                    {
                        new Vector2Int(-1, 0),
                        new Vector2Int(2, 0),
                        new Vector2Int(-1, 2),
                        new Vector2Int(2, -1),
                    };
                case 2 when rotTo == 1:
                    return new[]
                    {
                        new Vector2Int(-2, 0),
                        new Vector2Int(1, 0),
                        new Vector2Int(-2, 1),
                        new Vector2Int(1, -2),
                    };
                case 2 when rotTo == -1:
                    return new[]
                    {
                        new Vector2Int(2, 0),
                        new Vector2Int(-1, 0),
                        new Vector2Int(2, 1),
                        new Vector2Int(-1, -2),
                    };
                case -1 when rotTo == 2:
                    return new[]
                    {
                        new Vector2Int(1, 0),
                        new Vector2Int(-2, 0),
                        new Vector2Int(1, 2),
                        new Vector2Int(-2, -1),
                    };
                case -1 when rotTo == 0:
                    return new[]
                    {
                        new Vector2Int(1, 0),
                        new Vector2Int(-2, 0),
                        new Vector2Int(1, -2),
                        new Vector2Int(-2, 1), 
                    };
            }

            return new Vector2Int[] { };
        }
        
        public static Vector2Int[] GetTableFor(TetrisTileType state, int rotation)
        {
            switch (state)
            {
                case TetrisTileType.Special:
                    return SingleTileTable();
                case TetrisTileType.Empty:
                    break;
                case TetrisTileType.I:
                    return GetITable(rotation);
                case TetrisTileType.J:
                    return GetJTable(rotation);
                case TetrisTileType.L:
                    return GetLTable(rotation);
                case TetrisTileType.O:
                    return GetOTable(rotation);
                case TetrisTileType.S:
                    return GetSTable(rotation);
                case TetrisTileType.Z:
                    return GetZTable(rotation);
                case TetrisTileType.T:
                    return GetTTable(rotation);
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }

            throw new ArgumentOutOfRangeException();
        }

        private static Vector2Int[] SingleTileTable()
        {
            return new[]
            {
               new Vector2Int(0,0),
            };
        }

        private static Vector2Int[] GetITable(int rotation)
        {
            switch (rotation)
            {
                case -1:
                    return new[]
                    {
                        new Vector2Int(-1, -1),
                        new Vector2Int(-1, 0),
                        new Vector2Int(-1, 1),
                        new Vector2Int(-1, 2)
                    };
                case 2:
                    return new[]
                    {
                        new Vector2Int(-2, 0),
                        new Vector2Int(-1, 0),
                        new Vector2Int(0, 0),
                        new Vector2Int(1, 0)
                    };
                case 1:
                    return new[]
                    {
                        new Vector2Int(0, -1),
                        new Vector2Int(0, 0),
                        new Vector2Int(0, 1),
                        new Vector2Int(0, 2)
                    };
                case 0:
                    return new[]
                    {
                        new Vector2Int(-2, 1),
                        new Vector2Int(-1, 1),
                        new Vector2Int(0, 1),
                        new Vector2Int(1, 1)
                    };
            }

            throw new ArgumentOutOfRangeException();
        }

        private static Vector2Int[] GetJTable(int rotation)
        {
            switch (rotation)
            {
                case -1:
                    return new[]
                    {
                        new Vector2Int(0, -1),
                        new Vector2Int(0, 0),
                        new Vector2Int(0, 1),
                        new Vector2Int(-1, -1)
                    };
                case 2:
                    return new[]
                    {
                        new Vector2Int(1, -1),
                        new Vector2Int(-1, 0),
                        new Vector2Int(0, 0),
                        new Vector2Int(1, 0)
                    };
                case 1:
                    return new[]
                    {
                        new Vector2Int(0, -1),
                        new Vector2Int(0, 0),
                        new Vector2Int(0, 1),
                        new Vector2Int(1, 1)
                    };
                case 0:
                    return new[]
                    {
                        new Vector2Int(-1, 1),
                        new Vector2Int(-1, 0),
                        new Vector2Int(0, 0),
                        new Vector2Int(1, 0)
                    };
            }

            throw new ArgumentOutOfRangeException();
        }

        private static Vector2Int[] GetLTable(int rotation)
        {
            switch (rotation)
            {
                case 1:
                    return new[]
                    {
                        new Vector2Int(0, -1),
                        new Vector2Int(0, 0),
                        new Vector2Int(0, 1),
                        new Vector2Int(1, -1)
                    };
                case 0:
                    return new[]
                    {
                        new Vector2Int(1, 1),
                        new Vector2Int(-1, 0),
                        new Vector2Int(0, 0),
                        new Vector2Int(1, 0)
                    };
                case -1:
                    return new[]
                    {
                        new Vector2Int(0, -1),
                        new Vector2Int(0, 0),
                        new Vector2Int(0, 1),
                        new Vector2Int(-1, 1)
                    };
                case 2:
                    return new[]
                    {
                        new Vector2Int(-1, -1),
                        new Vector2Int(-1, 0),
                        new Vector2Int(0, 0),
                        new Vector2Int(1, 0)
                    };
            }

            throw new ArgumentOutOfRangeException();
        }

        private static Vector2Int[] GetOTable(int rotation)
        {
            return new[]
            {
                new Vector2Int(-1, 0),
                new Vector2Int(0, 0),
                new Vector2Int(-1, 1),
                new Vector2Int(0, 1)
            };
        }

        private static Vector2Int[] GetZTable(int rotation)
        {
            switch (rotation)
            {
                case -1:
                    return new[]
                    {
                        new Vector2Int(-1, 0),
                        new Vector2Int(0, 0),
                        new Vector2Int(0, 1),
                        new Vector2Int(-1, -1)
                    };
                case 2:
                    return new[]
                    {
                        new Vector2Int(1, -1),
                        new Vector2Int(0, -1),
                        new Vector2Int(0, 0),
                        new Vector2Int(-1, 0)
                    };
                case 1:
                    return new[]
                    {
                        new Vector2Int(0, -1),
                        new Vector2Int(0, 0),
                        new Vector2Int(1, 0),
                        new Vector2Int(1, 1)
                    };
                case 0:
                    return new[]
                    {
                        new Vector2Int(1, 0),
                        new Vector2Int(0, 0),
                        new Vector2Int(0, 1),
                        new Vector2Int(-1, 1)
                    };
            }

            throw new ArgumentOutOfRangeException();
        }

        private static Vector2Int[] GetSTable(int rotation)
        {
            switch (rotation)
            {
                case -1:
                    return new[]
                    {
                        new Vector2Int(0, -1),
                        new Vector2Int(0, 0),
                        new Vector2Int(-1, 1),
                        new Vector2Int(-1, 0)
                    };
                case 2:
                    return new[]
                    {
                        new Vector2Int(1, 0),
                        new Vector2Int(0, -1),
                        new Vector2Int(0, 0),
                        new Vector2Int(-1, -1)
                    };
                case 1:
                    return new[]
                    {
                        new Vector2Int(1, -1),
                        new Vector2Int(0, 0),
                        new Vector2Int(1, 0),
                        new Vector2Int(0, 1)
                    };
                case 0:
                    return new[]
                    {
                        new Vector2Int(1, 1),
                        new Vector2Int(0, 0),
                        new Vector2Int(0, 1),
                        new Vector2Int(-1, 0)
                    };
            }

            throw new ArgumentOutOfRangeException();
        }

        private static Vector2Int[] GetTTable(int rotation)
        {
            switch (rotation)
            {
                case -1:
                    return new[]
                    {
                        new Vector2Int(0, -1),
                        new Vector2Int(0, 0),
                        new Vector2Int(0, 1),
                        new Vector2Int(-1, 0)
                    };
                case 2:
                    return new[]
                    {
                        new Vector2Int(0, -1),
                        new Vector2Int(0, 0),
                        new Vector2Int(1, 0),
                        new Vector2Int(-1, 0)
                    };
                case 1:
                    return new[]
                    {
                        new Vector2Int(0, -1),
                        new Vector2Int(0, 0),
                        new Vector2Int(1, 0),
                        new Vector2Int(0, 1)
                    };
                case 0:
                    return new[]
                    {
                        new Vector2Int(1, 0),
                        new Vector2Int(0, 0),
                        new Vector2Int(-1, 0),
                        new Vector2Int(0, 1)
                    };
            }

            throw new ArgumentOutOfRangeException();
        }
    }
}