using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StyleSwitcherButton : SwitcherButton
{

    public override void Start()
    {
        switch (SettingsContainer.blockStyle)
        {
            case BlockStyle.Buttons:
                currentIndex = 0;
                button.text = options[0];
                break;
            case BlockStyle.Puyos:
                currentIndex = 1;
                button.text = options[1];
                break;
            case BlockStyle.Flat:
                currentIndex = 2;
                button.text = options[2];
                break;
            case BlockStyle.Cursed:
                currentIndex = 3;
                button.text = options[3];
                break;
        }

        if (SettingsContainer.randomBlockStyle)
        {
            currentIndex = 4;
            button.text = options[4];
        }
    }

    public override void Click()
    {
        base.Click();

        switch (this.currentIndex)
        {
            case 0:
                SettingsContainer.blockStyle = BlockStyle.Buttons;
                SettingsContainer.randomBlockStyle = false;
                break;
            case 1:
                SettingsContainer.blockStyle = BlockStyle.Puyos;
                SettingsContainer.randomBlockStyle = false;
                break;
            case 2:
                SettingsContainer.blockStyle = BlockStyle.Flat;
                SettingsContainer.randomBlockStyle = false;
                break;
            case 4:
                SettingsContainer.randomBlockStyle = true;
                SettingsContainer.TickRandomiser();
                break;
            case 3:
                SettingsContainer.blockStyle = BlockStyle.Cursed;
                SettingsContainer.randomBlockStyle = false;
                break;
        }
    }


    

}
