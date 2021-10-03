using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuteButton : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] AudioSource gameLoopMusic;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        gameLoopMusic.mute = true;
    }
}
