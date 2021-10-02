using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    Vector3 respawnPos = new Vector3(30.3f, 2.651025f, 0f);

    float coinProbability = 50;
    float obstancleProbablity = 50;

    public Obstacle[] allObstacles;
    public Coin[] allCoins;

    private void Start()
    {
        allObstacles = GetComponentsInChildren<Obstacle>(true);
        allCoins = GetComponentsInChildren<Coin>(true);
    }

    public void HandleUpdate()
    {
        if (transform.position.x > -23.57)
        {
            transform.Translate(new Vector3(-3*Time.deltaTime, 0, 0));
        }
        else
        {
            transform.position = respawnPos;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "spawncollider")
        {
            SetObstaclesAndCoins();
        }
    }
    public void SetObstaclesAndCoins()
    {
        for(int i = 0; i < allObstacles.Length; i++)
        {
            if (i == 0)
            {
                allObstacles[i].gameObject.SetActive(true);
            }
            else
            {
                if (Random.Range(0, 100) < obstancleProbablity)
                {
                    allObstacles[i].gameObject.SetActive(true);
                }
                else
                {
                    allObstacles[i].gameObject.SetActive(false);
                }
            }
        }

        foreach (Coin c in allCoins)
        {
            if (Random.Range(0, 100) < coinProbability)
            {
                c.gameObject.SetActive(true);
            }
            else
            {
                c.gameObject.SetActive(false);
            }
        }
    }
}
