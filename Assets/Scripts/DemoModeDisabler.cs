using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class DemoModeDisabler : MonoBehaviour
{
    public GameObject GameObject;
    
    void Update()
    {
        GameObject.SetActive(!DemoModeChecker.DemoMode);
    }
}
