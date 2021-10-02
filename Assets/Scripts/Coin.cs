using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Coin : MonoBehaviour
{
    float originalY;

    void Start()
    {
        originalY = this.transform.position.y;
    }

    public void HandleUpdate()
    {
        transform.position = new Vector3(transform.position.x, Mathf.Lerp(originalY, originalY+1, Mathf.PingPong(Time.time * 1, 1.0f)), transform.position.z);
    }
}
