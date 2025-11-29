using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public string InstantSwitch = "";

    public void Start()
    {
        if (InstantSwitch != "")
        {
            SceneManager.LoadScene(InstantSwitch);
        }
    }

    public void SwitchTo(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
