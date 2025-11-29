using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMover : MonoBehaviour
{
    public Transform transformToMove;

    public Canvas rootCanvas;

    public Vector2 moveOffset;

    public int moveFrames;

    private bool moved = false;

    private bool moving;

    public void TriggerMove()
    {
        if (moving) return;
        if (moved) StartCoroutine(DoMove(-1));
        else StartCoroutine(DoMove(1));
    }

    private IEnumerator DoMove(int dir)
    {
        moving = true;

        TAnim anim = new TAnim(TTransition.InverseCurve, moveFrames);
        anim.Forward();

        var localMoveOffset = moveOffset * (rootCanvas != null ? rootCanvas.scaleFactor : 1f);
        
        Vector2 initialPosition = transformToMove.position;
        for (int i = 0; i < moveFrames; i++)
        {
            transformToMove.position = initialPosition + localMoveOffset * (float)anim.Get() * dir;
            yield return new WaitForFixedUpdate();
        }

        transformToMove.position = initialPosition + localMoveOffset * dir;
        moving = false;
        moved = !moved;
    }
}
