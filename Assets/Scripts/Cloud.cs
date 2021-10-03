using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    Vector3 respawnPos = new Vector3(-7.23f, -1.4f, -76f);
    IndividualCloud[] Clouds;
    // Start is called before the first frame update
    void Start()
    {
        Clouds = GetComponentsInChildren<IndividualCloud>();
    }

    // Update is called once per frame
    public void HandleUpdate()
    {
        foreach (IndividualCloud cloud in Clouds)
        {
            cloud.HandleUpdate();
        }
        if (transform.position.x < 31.45)
        {
            transform.Translate(new Vector3(.5f * Time.deltaTime, 0, 0));
        }
        else
        {
            transform.position = respawnPos;
        }
    }
}
