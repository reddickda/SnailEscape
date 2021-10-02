using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObstacleHandler : MonoBehaviour
{
    // Start is called before the first frame update
    // TODO handle a list of obstacles on the screen and space them out a bit
    // spawn when game starts despawn when game ends

    public Obstacle[] obstacles;
    float lastX;
    [SerializeField] GameObject obstacle;
    GameObject lastSpawned;
    void Start()
    {
        lastX = 11f;
    }

    public void HandleUpdate()
    {
        if(lastSpawned == null)
        {
            lastSpawned = GameObject.Find("Obstacle(Clone)");
        }
        int xSpawnRand = Random.Range(5, 10);
        obstacles = FindObjectsOfType<Obstacle>().Where(obj => obj.name == "Obstacle(Clone)").ToArray();
        if(obstacles.Length < 6)
        {
            Vector3 spawnPos = new Vector3(lastX + xSpawnRand, obstacle.transform.position.y, obstacle.transform.position.z);
            if (spawnPos.x - lastSpawned.transform.position.x > 5)
            {
                lastSpawned = Instantiate(obstacle, spawnPos, Quaternion.identity, transform);
                lastX = spawnPos.x;
            }
        }
        else
        {
            lastX = 11f;
        }
    }
}
