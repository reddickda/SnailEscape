using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Background : MonoBehaviour
{
    Vector3 respawnPos = new Vector3(30.3f, 2.651025f, 0f);

    float coinProbability = 50;
    float obstancleProbablity = 60;

    float timer = 0;

    public Obstacle[] allObstacles;
    public Coin[] allCoins;
    public BirdScript[] allBirds;

    private void Start()
    {
        allObstacles = GetComponentsInChildren<Obstacle>(true);
        allCoins = GetComponentsInChildren<Coin>(true);
        allBirds = Resources.FindObjectsOfTypeAll<BirdScript>();
    }

    public void HandleUpdate()
    {
        timer += Time.deltaTime;
        if (transform.position.x > -23.57)
        {
            transform.Translate(new Vector3(-3*Time.deltaTime, 0, 0));
        }
        else
        {
            transform.position = respawnPos;
        }
        if(timer > 10)
        {
            //for(int i = 0; i < allBirds.Length; i++)
            //{
            //    allBirds[i].gameObject.SetActive(true);
            //}
            allBirds[0].gameObject.SetActive(true);
        }
        if(timer > 20)
        {
            allBirds[1].gameObject.SetActive(true);
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
