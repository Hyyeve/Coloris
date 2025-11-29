using UnityEngine;
using System.Collections;
using Input;

public class ControlsDemoController : GameController
{
    public override IEnumerator HandleMoveBlock()
    {
        updateInProgress = true;
        if (input.Pressed(TetrisInput.HardDrop))
        {
            currentBlock.TryMoveDistanceY(-100);
            yield return StartCoroutine(LockBlock());
            updateInProgress = false;
            yield break;
        }

        if (input.Pressed(TetrisInput.Hold) && !hasHeldBlock)
        {
            SwapHoldBlock();
            hasHeldBlock = true;
            updateInProgress = false;
            yield break;
        }

        if (input.Pressed(TetrisInput.RotateR))
        {
            currentBlock.Rotate(true);
            updateInProgress = false;
            yield break;
        }
        else if (input.Pressed(TetrisInput.RotateL))
        {
            currentBlock.Rotate(false);
            updateInProgress = false;
            yield break;
        }

        if (input.Down(TetrisInput.SoftDrop))
        {
           sdsCount++;
           if (sdsCount >= SDS)
           {
              currentBlock.TryMoveDistanceY(-1);
              sdsCount = 0;
           }
        }
        else
        {
            sdsCount = 0;
        }

        if (!input.Down(TetrisInput.Right) && !input.Down(TetrisInput.Left) || input.Down(TetrisInput.Right) && input.Down(TetrisInput.Left))
        {
            dasCount = 0;
        }

        if (input.Pressed(TetrisInput.Right) || input.Down(TetrisInput.Right) && !input.Down(TetrisInput.Left))
        {
            if (input.Pressed(TetrisInput.Right))
            {
                dasCount = 0;
                currentBlock.TryMoveDistanceX(1);
            }
            else
            {
                dasCount++;
                if (dasCount >= DAS)
                {
                    if (ARR == 0)
                    {
                        currentBlock.TryMoveDistanceX(100);
                    }
                    else
                    {
                        arrCount++;
                        if (arrCount > ARR)
                        {
                            currentBlock.TryMoveDistanceX(1);
                            arrCount = 0;

                        }
                    }
                }
                else
                {
                    arrCount = 0;
                }
            }
        }


        if (input.Pressed(TetrisInput.Left) || input.Down(TetrisInput.Left) && !input.Down(TetrisInput.Right))
        {
            if (input.Pressed(TetrisInput.Left))
            {
                dasCount = 0;
                currentBlock.TryMoveDistanceX(-1);
            }
            else
            {
                dasCount++;
                if (dasCount >= DAS)
                {
                    if (ARR == 0)
                    {
                        currentBlock.TryMoveDistanceX(-100);
                    }
                    else
                    {
                        arrCount++;
                        if (arrCount > ARR)
                        {
                            currentBlock.TryMoveDistanceX(-1);
                            arrCount = 0;
                        }
                    }
                }
                else
                {
                    arrCount = 0;
                }
            }
        }
        updateInProgress = false;
        yield break;
    }

    public override void SpawnBlockEffect()
    {

    }

    public override IEnumerator LockBlock()
    {
        currentBlock.Place();
        yield return new WaitForSeconds(0.5f);
        currentBlock.Remove();
        yield break;
    }
}
