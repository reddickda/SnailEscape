using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Obstacle : MonoBehaviour
{
    public float speed = 2.3f;

    public void HandleUpdate()
    {
        Vector3 left = Vector3.left;
        Vector3 translation = left * Time.deltaTime * speed;
        transform.Translate(translation);
        if(transform.position.x < -100)
        {
            Destroy(gameObject);
        }
    }
}
