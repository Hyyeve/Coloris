using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TetrisGrid
{
    public class TetrisTile : MonoBehaviour
    {
        
        public Sprite[] SimpleTileSpritesEGCBYOGRP;
        public Sprite[] AliveTileSpritesEGCBYOGRP;
        public Sprite[] FlatTileSpritesEGCBYOGRP;

        private Sprite[] tileSpritesEGCBYOGRP;

        private TetrisTileType _state;
        private TetrisTileColour _colour;
        private SpriteRenderer _renderer;

        public void InitTile(Vector2 position)
        {

            switch (SettingsContainer.blockStyle)
            {
                case BlockStyle.Buttons:
                    tileSpritesEGCBYOGRP = SimpleTileSpritesEGCBYOGRP;
                    break;
                case BlockStyle.Puyos:
                    tileSpritesEGCBYOGRP = AliveTileSpritesEGCBYOGRP;
                    break;
                case BlockStyle.Flat:
                    tileSpritesEGCBYOGRP = FlatTileSpritesEGCBYOGRP;
                    break;
                case BlockStyle.Cursed:
                    var index = Random.Range(0, 3);
                    Sprite[] sprite;
                    switch (index)
                    {
                        case 0:
                            sprite = SimpleTileSpritesEGCBYOGRP;
                            break;
                        case 1:
                            sprite = AliveTileSpritesEGCBYOGRP;
                            break;
                        case 2:
                            sprite = FlatTileSpritesEGCBYOGRP;
                            break;
                        default:
                            sprite = SimpleTileSpritesEGCBYOGRP;
                            break;
                    }

                    tileSpritesEGCBYOGRP = sprite;
                    break;
            }

            transform.localPosition = position;
            _renderer = GetComponent<SpriteRenderer>();
            SetState(TetrisTileType.Empty, TetrisTileColour.None);

        }

        public void RunBurst(bool side)
        {
            StartCoroutine(BurstRoutine(side));
        }

        private IEnumerator BurstRoutine(bool side)
        {
            for(int i = 0; i < 15; i++)
            {
                if(side) _renderer.material.SetFloat("_Burst_Percent_A", (float)i / 15);
                else _renderer.material.SetFloat("_Burst_Percent_B", (float)i / 15);


                yield return new WaitForFixedUpdate();
            }
            _renderer.material.SetFloat("_Burst_Percent_A", 0f);
            _renderer.material.SetFloat("_Burst_Percent_B", 0f);
        }

        public void SetState(TetrisTileType type, TetrisTileColour colour)
        {
            _state = type;
            _colour = colour;
            _renderer.sprite = GetSpriteFor(colour);
            if(colour == TetrisTileColour.Rainbow)
            {
                _renderer.material.SetInt("Rainbowify", 1);
            }
            else
            {
                _renderer.material.SetInt("Rainbowify", 0);
            }
        }

        public void SpriteOverride(Sprite sprite)
        {
            _renderer.sprite = sprite;
        }

        public TetrisTileType GetState()
        {
            return _state;
        }

        public TetrisTileColour GetColour()
        {
            return _colour;
        }

        private Sprite GetSpriteFor(TetrisTileColour state)
        {
            switch (state)
            {
                case TetrisTileColour.None:
                    return tileSpritesEGCBYOGRP[0];
                case TetrisTileColour.Garbage:
                    return tileSpritesEGCBYOGRP[1];
                case TetrisTileColour.Blue:
                    return tileSpritesEGCBYOGRP[2];
                case TetrisTileColour.Cyan:
                    return tileSpritesEGCBYOGRP[3];
                case TetrisTileColour.Yellow:
                    return tileSpritesEGCBYOGRP[4];
                case TetrisTileColour.Orange:
                    return tileSpritesEGCBYOGRP[5];
                case TetrisTileColour.Green:
                    return tileSpritesEGCBYOGRP[6];
                case TetrisTileColour.Red:
                    return tileSpritesEGCBYOGRP[7];
                case TetrisTileColour.Purple:
                    return tileSpritesEGCBYOGRP[8];
                case TetrisTileColour.Rainbow:
                    return tileSpritesEGCBYOGRP[1];
            }

            throw new IndexOutOfRangeException("Tetris tile state for tile at " + transform.position + " invalid!");
        }


    }
}