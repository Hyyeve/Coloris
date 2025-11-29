using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class videofader : MonoBehaviour
{

    private VideoPlayer player;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<VideoPlayer>();
        StartCoroutine(fade());
    }


    IEnumerator fade()
    {
        while (!player.isPrepared) yield return new WaitForEndOfFrame();

        var initialAlpha = player.targetCameraAlpha;
        for(float i = 0; i < 1; i+= 1f/120f)
        {
            player.targetCameraAlpha = initialAlpha * i;
            yield return new WaitForEndOfFrame();
        }


        yield break;
    }
}
