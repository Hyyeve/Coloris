using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DemoModeHandler : MonoBehaviour
{

    public string SceneToLoad;
    void Update()
    {
        if (UnityEngine.InputSystem.Keyboard.current.f12Key.wasPressedThisFrame && UnityEngine.InputSystem.Keyboard.current
            .f1Key.isPressed)
        {
            DemoModeChecker.DemoMode = !DemoModeChecker.DemoMode;
        }



        if (SceneManager.GetActiveScene().name == SceneToLoad)
        {
            DemoModeChecker.ResetDemoTimer();

        } 
        else if (DemoModeChecker.CheckDemoTimer())
        {
            SceneManager.LoadScene(SceneToLoad);
        }
            
    }
}
