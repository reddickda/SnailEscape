using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
  

    Vector3 respawnPos = new Vector3(30.3f, 2.651025f, 0f);

    Background[] backgrounds;

    void Start()
    {
        backgrounds = FindObjectsOfType<Background>();
    }

    // Update is called once per frame
    public void HandleUpdate()
    {

    }
}
