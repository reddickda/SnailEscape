using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObstacleHandler : MonoBehaviour
{
    [SerializeField] GameObject obstacle;
    [SerializeField] GameObject coin;
    [SerializeField] Transform spawnLocation;
    [SerializeField] Transform coinSpawnLocation;
    [SerializeField] Transform lowCoinSpawnLocation;

    float coinProbability = 75;

    GameObject lastSpawned;
    private float timer;
    public float timeBetweenSpawns = 2;
    public float timeSinceLastSpawn;

    Coin[] coins;
    Obstacle[] obstacles;
    void Start()
    {
        timer = 0f;
    }

    // have some logic for each type of mushroom having a coin spawn or two... if 4 seconds between down spawn high, if two seconds spawn high
    public void HandleUpdate()
    {
        coins = FindObjectsOfType<Coin>().Where(obj => obj.tag == "coin").ToArray();
        obstacles = FindObjectsOfType<Obstacle>().Where(obj => obj.tag == "obstacle").ToArray();
        timer += Time.deltaTime;
        if (timer > timeBetweenSpawns)
        {
            timer = 0f;
            Instantiate(obstacle, spawnLocation.position, Quaternion.identity, GameObject.Find("Spawnpoint").transform);
            if (Random.Range(0, 100) < coinProbability) 
            {
                if (timeBetweenSpawns == 4)
                {
                    Instantiate(coin, coinSpawnLocation.position, Quaternion.identity, GameObject.Find("CoinSpawnPoint").transform);
                }
                else
                {
                    Instantiate(coin, lowCoinSpawnLocation.position, Quaternion.identity, GameObject.Find("CoinSpawnPoint").transform);
                }

            }
            timeBetweenSpawns = Random.Range(2, 4);

        }
        foreach(Coin coin in coins)
        {
            coin.HandleUpdate();
        }foreach(Obstacle obs in obstacles)
        {
            obs.HandleUpdate();
        }
    }
}
