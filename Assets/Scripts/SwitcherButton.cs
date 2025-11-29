using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SwitcherButton : MonoBehaviour
{
    public TextMeshProUGUI button;

    public List<string> options;

    protected int currentIndex = 0;

    public virtual void Start()
    {
        int index;

        if ((index = options.IndexOf(button.text)) != -1)
        {
            currentIndex = index;
        }
    }

    public virtual void Click()
    {
        string current = button.text;

        int index;

        if((index = options.IndexOf(current)) != -1)
        {
            int next = index + 1;
            if (next >= options.Count) next = 0;
            button.text = options[next];
            currentIndex = next;
        }
        else if(options.Count > 0)
        {
            button.text = options[0];
            currentIndex = 0;
        }
    }

}
