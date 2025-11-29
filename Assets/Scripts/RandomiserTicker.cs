using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomiserTicker : MonoBehaviour
{
    public void Tick()
    {
        SettingsContainer.TickRandomiser();
    }
}
