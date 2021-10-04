using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Coin : MonoBehaviour
{
    float originalY;
    public float speed = 2.3f;
    [SerializeField] GameObject coinSprite;

    Vector3 startPos;
    [SerializeField] float frequency = 5f;
    [SerializeField] float magnitude = .5f;
    [SerializeField] float offset = 0f;

    void Start()
    {
        originalY = this.transform.position.y;
        startPos = coinSprite.transform.localPosition;

    }

    public void HandleUpdate()
    {
        Vector3 left = Vector3.left;
        Vector3 translation = left * Time.deltaTime * speed;


        transform.Translate(translation);
        if (transform.position.x < -100)
        {
            Destroy(gameObject);
        }

        coinSprite.transform.localPosition = startPos + transform.up * Mathf.Sin(Time.time * frequency + offset) * magnitude;
    }
}
