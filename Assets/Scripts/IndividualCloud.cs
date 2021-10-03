using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndividualCloud : MonoBehaviour
{
    Vector3 startPos;
    [SerializeField] float frequency = 5f;
    [SerializeField] float magnitude = 5f;
    [SerializeField] float offset = 0f;
    void Start()
    {
        startPos = transform.localPosition;
    }

    public void HandleUpdate()
    {
        transform.localPosition = startPos + transform.up * Mathf.Sin(Time.time * frequency + offset) * magnitude;
    }
}
