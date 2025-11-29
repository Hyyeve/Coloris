using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;


public class SpriteSwitcher : MonoBehaviour
{
    public Sprite Simple;
    public Sprite Alive;
    public Sprite Flat;

    private Sprite Randomised;

    public SpriteRenderer renderer;
    public TetrisPieceDisplayer displayer;
    public Image image;

    public bool alwaysCheck = false;

    public void Start()
    {
        var index = Random.Range(0, 3);
        switch (index)
        {
            case 0:
                Randomised = Simple;
                break;
            case 1:
                Randomised = Alive;
                break;
            case 2:
                Randomised = Flat;
                break;
            default:
                Randomised = Simple;
                break;
        }

        Check();
        
    }

    private void Check()
    {

        switch (SettingsContainer.blockStyle)
        {
            case BlockStyle.Buttons:
                if (renderer != null) renderer.sprite = Simple;
                if (displayer != null) displayer.UpdateSpriteOverride(Simple);
                if (image != null) image.sprite = Simple;
                break;
            case BlockStyle.Puyos:
                if (renderer != null) renderer.sprite = Alive;
                if (displayer != null) displayer.UpdateSpriteOverride(Alive);
                if (image != null) image.sprite = Alive;
                break;
            case BlockStyle.Flat:
                if (renderer != null) renderer.sprite = Flat;
                if (displayer != null) displayer.UpdateSpriteOverride(Flat);
                if (image != null) image.sprite = Flat;
                break;
            case BlockStyle.Cursed:
                if (renderer != null) renderer.sprite = Randomised;
                if (displayer != null) displayer.UpdateSpriteOverride(Randomised);
                if (image != null) image.sprite = Randomised;
                break;
        }
    }

    public void FixedUpdate()
    {
        if (alwaysCheck) Check();
    }
}
