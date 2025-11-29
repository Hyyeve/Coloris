using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shaker : MonoBehaviour
{
    public float dX = 0.1f;
    public float dY = 0.1f;
    public float shakeFrames = 60;
    public float repeatTimes = 5;



    public bool triggerShake;

    void Update()
    {

        if (Keyboard.current.endKey.isPressed) triggerShake = true;

        if (triggerShake)
        {
            Shake();
            triggerShake = false;
        }
    }

    public void Shake()
    {
        StartCoroutine(DoShake());

    }

    private IEnumerator DoShake()
    {
        Vector3 initialPos = transform.position;

        for (int j = 0; j < repeatTimes; j++)
        {

            for (float i = 0; i < 1; i += 4f / shakeFrames)
            {
                float oX = dX * i;
                float oY = dY * i;

                transform.position = initialPos + new Vector3(oX, oY);

                yield return new WaitForEndOfFrame();
            }

            for (float i = 1; i > -1; i -= 2f / shakeFrames)
            {
                float oX = dX * i;
                float oY = dY * i;

                transform.position = initialPos + new Vector3(oX, oY);

                yield return new WaitForEndOfFrame();
            }

            for (float i = -1; i < 0; i += 4f / shakeFrames)
            {
                float oX = dX * i;
                float oY = dY * i;

                transform.position = initialPos + new Vector3(oX, oY);

                yield return new WaitForEndOfFrame();
            }

        }
    }
}
