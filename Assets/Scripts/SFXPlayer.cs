using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SFXPlayer : MonoBehaviour
{
    public AudioClip sound;

    private AudioSource player;

    void Start()
    {
        player = GameObject.FindWithTag("SFXPlayer").GetComponent<AudioSource>();
    }

    public void PlaySound()
    {
        player.PlayOneShot(sound, 1.3f);
    }
}
