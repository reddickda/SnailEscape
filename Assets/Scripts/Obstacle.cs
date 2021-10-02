using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Obstacle : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    public void HandleUpdate ()
    {

    }
}
