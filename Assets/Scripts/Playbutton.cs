using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playbutton : MonoBehaviour
{
    [SerializeField] AudioSource gameLoopMusic;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        gameLoopMusic.mute = false;
    }
}
