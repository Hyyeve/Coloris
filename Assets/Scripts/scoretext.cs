using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class scoretext : MonoBehaviour
{
    public TextMeshProUGUI texttoset;

    public GameController controller;

    void Update()
    {
        texttoset.text = string.Format("{0,6:000000}", controller.score);
    }
}
