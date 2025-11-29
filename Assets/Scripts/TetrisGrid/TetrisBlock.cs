using System.Linq;
using MoreLinq;
using UnityEngine;

namespace TetrisGrid
{
    public class TetrisBlock
    {
        public bool placed = true;
        private Vector2Int pivot;
        private Vector2Int[] offsets;
        private TetrisBlockState state;
        private TetrisGridController owner;
        private int rotation = 0;

        public bool TrySpawn(TetrisGridController controller, TetrisBlockState state, Vector2Int pivot)
        {
            this.placed = false;
            this.pivot = pivot;
            this.owner = controller;
            this.state = state;
            this.rotation = 0;
            this.offsets = TetrisBlockRotationTable.GetTableFor(state.type, 0);


            if (IsValidSpawnPosition(pivot, offsets))
            {
                ApplyState(pivot, offsets);
                return true;
            }
            else
            {
                return false;
            }

        }

        public void SpawnParticles(GameObject particles)
        {
            int lowestOff = 999;

            Vector2Int[] lowest = offsets.Where(predicate => !offsets.Contains(predicate + Vector2Int.down)).ToArray();
            
            foreach(Vector2Int vec in lowest)
            {
                if (!owner.IsValidCell(pivot + vec + Vector2Int.down) || owner.GetCell(pivot + vec + Vector2Int.down).GetState() != TetrisTileType.Empty)
                {
                    Vector2 position = owner.FromGrid(pivot + vec);
                    GameObject.Instantiate(particles, position, Quaternion.identity);
                }
            }
        }

        public void Place()
        {
            ApplyState(pivot, offsets);
            placed = true;
        }

        public TetrisBlockState GetState()
        {
            return state;
        }

        public void Remove()
        {
            offsets.ForEach(offset => owner.SetCellState(pivot + offset, TetrisTileType.Empty, TetrisTileColour.None));
            placed = true;
        }

        public bool Rotate(bool clockwise)
        {
            return TryRotateTo(TetrisBlockRotationTable.GetRotatedID(clockwise, rotation));
        }
        
        public bool TryRotateTo(int newRotation)
        {
            Vector2Int newPivot = pivot;
            Vector2Int[] newOffsets = TetrisBlockRotationTable.GetTableFor(state.type, newRotation);

            if (IsValidPosition(pivot, newOffsets))
            {
                rotation = newRotation;
                ApplyState(newPivot, newOffsets);
                return true;
            }
            else
            {
                Vector2Int[] kickTable = TetrisBlockRotationTable.GetRotationKickOffsetsFor(state.type, rotation, newRotation);

                foreach (Vector2Int offset in kickTable)
                {
                    Vector2Int kickedPivot = newPivot + offset;
                    if (!IsValidPosition(kickedPivot, newOffsets)) continue;
                    rotation = newRotation;
                    ApplyState(kickedPivot, newOffsets);
                    return true;
                }
            }
            return false;
        }

        public int GetRotation()
        {
            return rotation;
        }

        public bool IsValidWhenAt(Vector2Int change)
        {
            return IsValidPosition(pivot + change, offsets);
        }

        public bool TryMoveDistanceX(int distance)
        {
            int dir = (int)Mathf.Sign(distance);
            for(int i = 0; i < Mathf.Abs(distance); i++)
            {
                if(!TryMove(new Vector2Int(dir, 0))) return false;
            }
            return true;
        }

        public bool TryMoveDistanceY(int distance)
        {
            int dir = (int)Mathf.Sign(distance);
            for (int i = 0; i < Mathf.Abs(distance); i++)
            {
                if (!TryMove(new Vector2Int(0, dir))) return false;
            }
            return true;
        }

        public Vector2Int CalculateGhostPlacement()
        {
            Vector2Int position = this.pivot;
            for (int i = 0; i < 100; i++)
            {
                position += new Vector2Int(0, -1);
                if (IsValidPosition(position, offsets)) continue;
                return position + new Vector2Int(0, 1);
            }
            return position;
        }

        public bool TryMove(Vector2Int change)
        {
            if(IsValidPosition(pivot + change, offsets))
            {
                ApplyState(pivot + change, offsets);
                return true;
            }
            return false;
        }

        private bool IsValidPosition(Vector2Int newPivot, Vector2Int[] newOffsets)
        {
            if(!newOffsets.All(offset => owner.IsValidCell(newPivot + offset))) return false;

            var _tempState = state;
            offsets.ForEach(offset => owner.GetCell(pivot + offset).SetState(TetrisTileType.Empty, TetrisTileColour.None));
            var ret = newOffsets.All(offset => owner.GetCell(newPivot + offset).GetState() == TetrisTileType.Empty);
            ApplyState(pivot, offsets);
            return ret;
        }

        private bool IsValidSpawnPosition(Vector2Int newPivot, Vector2Int[] newOffsets)
        {
            if (!newOffsets.All(offset => owner.IsValidCell(newPivot + offset))) return false;
            return newOffsets.All(offset => owner.GetCell(newPivot + offset).GetState() == TetrisTileType.Empty);
        }

        private void ApplyState(Vector2Int newPivot, Vector2Int[] newOffsets)
        {
            offsets.ForEach(offset => owner.SetCellState(pivot + offset, TetrisTileType.Empty, TetrisTileColour.None));
            for (int i = 0; i < newOffsets.Length; i++)
            {
                TetrisTileColour colour = state.colours.Length > i ? state.colours[i] : state.colours[state.colours.Length - 1]; 
                owner.SetCellState(newPivot + newOffsets[i], state.type, colour);
            }
            pivot = newPivot;
            offsets = newOffsets;
        }
 
    }
}