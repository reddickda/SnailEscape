using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum GameState { play, stop, pause, resume }

public class GameController : MonoBehaviour
{

    GameState state;

    [SerializeField] PlayerController playerController;

    [SerializeField] Ui ui;

    [SerializeField] GameObject player;

    [SerializeField] LoopingBackground loopingBackground; 

    GameObject[] Backgrounds;
    GameObject[] Clouds;



    Coin[] coins;

    void Start()
    {
        DisableAll();
        player.SetActive(true);
        state = GameState.stop;
        playerController.OnDead += GameOver;
        playerController.OnPause += PauseGame;
        ui.OnResume += ResumeGame;
        ui.OnStart += StartGame;
        ui.OnExit += ExitGame;
        ui.EnableMenu();
        Backgrounds = GameObject.FindGameObjectsWithTag("background");
        Clouds = GameObject.FindGameObjectsWithTag("cloud");
        coins = Resources.FindObjectsOfTypeAll<Coin>();
    }

    // Update is called once per frame
    void Update()
    {
        if(state == GameState.play)
        {
            playerController.anim.SetFloat("Speed", 1f);
            loopingBackground.HandleUpdate();
            ui.DisablePauseMenu();
            ui.DisableMenu();
            playerController.HandleUpdate();
            //obstacleHandler.HandleUpdate();
            foreach (GameObject background in Backgrounds)
            {
               background.GetComponent<Background>().HandleUpdate();
            }
            foreach (GameObject cloud in Clouds)
            {
                cloud.GetComponent<Cloud>().HandleUpdate();

            }
            foreach (Coin c in coins)
            {
                c.HandleUpdate();
            }
        }
        if(state == GameState.stop)
        {
            ui.EnableMenu();
        }
        if(state == GameState.pause)
        {
            ui.EnablePauseMenu();
        }
    }
    private void FixedUpdate()
    {
        if (state == GameState.play)
        {
            //foreach(Obstacle obstacle in obstacleHandler.obstacles)
            //{
            //    obstacle.HandleUpdate();
            //}
        }
    }

    void GameOver()
    {
        //StartCoroutine(WaitAfterGameOver());
        state = GameState.stop;
        playerController.anim.SetFloat("Speed", 0f);
    }

    IEnumerator WaitAfterGameOver()
    {
        state = GameState.stop;
        yield return new WaitForSeconds(2f);
        playerController.anim.SetFloat("Speed", 0f);
    }

    void StartGame()
    {
        StartCoroutine(WaitToStart());
        state = GameState.play;
        playerController.anim.SetFloat("Speed", 1f);
        playerController.anim.SetBool("isDead", false);
    }

    void PauseGame()
    {
        state = GameState.pause;
        playerController.anim.SetFloat("Speed", 0f);
    }

    void ResumeGame()
    {
        state = GameState.play;
        playerController.paused = false;
        playerController.anim.SetFloat("Speed", 1f);
        playerController.anim.SetBool("isDead", false);
    }

    IEnumerator WaitToStart()
    {
        DisableAll();
        yield return new WaitForSeconds(1f);
    }

    void DisableAll()
    {
        GameObject[] obstacles = FindObjectsOfType<GameObject>().Where(obj => obj.tag == "obstacle").ToArray();
        GameObject[] coins = FindObjectsOfType<GameObject>().Where(obj => obj.tag == "scorecollider").ToArray();
        if (obstacles.Length > 0)
        {
            foreach (GameObject obj in obstacles)
            {
                obj.SetActive(false);
            }
            foreach (GameObject coin in coins)
            {
                coin.SetActive(false);
            }
        }
    }


    void ExitGame()
    {
        Application.Quit();
    }
}
