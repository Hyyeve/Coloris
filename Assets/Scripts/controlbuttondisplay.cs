using System.Collections;
using System.Collections.Generic;
using Input;
using UnityEngine;

public class controlbuttondisplay : MonoBehaviour
{

    public Color tintU;
    public Color tintP;

    public TetrisInput input;


    private InputComponent inputComp;
    private SpriteRenderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        inputComp = GetComponent<InputComponent>();
    }

    // Update is called once per frame
    void Update()
    {
        inputComp.PushInputs();
        inputComp.FinaliseInputs();
        if(inputComp.Down(input))
        {
            renderer.color = tintP;
        }
        else
        {
            renderer.color = tintU;
        }
        inputComp.PopInputs();
    }
}
