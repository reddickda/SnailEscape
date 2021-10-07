using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public enum GameState { play, stop, pause, resume }

public class GameController : MonoBehaviour
{

    GameState state;

    [SerializeField] PlayerController playerController;

    [SerializeField] Ui ui;

    [SerializeField] GameObject player;

    [SerializeField] LoopingBackground loopingBackground;

    [SerializeField] ObstacleHandler handler;

    [SerializeField] LeaderboardHandler leaderboard;

    [SerializeField] GameObject startPanel;
    bool nameEntered = false;

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
        startPanel.gameObject.SetActive(true);
        var button = startPanel.GetComponentInChildren<Button>();
        button.onClick.AddListener(Enter);
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
            handler.HandleUpdate();
            foreach (GameObject background in Backgrounds)
            {
               background.GetComponent<Background>().HandleUpdate();
            }
            foreach (GameObject cloud in Clouds)
            {
                cloud.GetComponent<Cloud>().HandleUpdate();

            }
            //foreach (Coin c in coins)
            //{
            //    c.HandleUpdate();
            //}
        }
        if(state == GameState.stop)
        {
            if(!nameEntered)
            {

            }
            else
            {
                ui.EnableMenu();
                startPanel.SetActive(false);
            }
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
        //leaderboard.ConnectToPostgres();
        state = GameState.stop;
        playerController.anim.SetFloat("Speed", 0f);
        playerController.bCollider2d.enabled = false;
        DestroyAll();
    }

    IEnumerator WaitAfterGameOver()
    {
        yield return new WaitForSeconds(.2f);
    }

    void StartGame()
    {
        StartCoroutine(WaitToStart());
        state = GameState.play;
        playerController.bCollider2d.enabled = true;
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

    void DestroyAll()
    {
        GameObject[] obstacles = FindObjectsOfType<GameObject>().Where(obj => obj.tag == "obstacle").ToArray();
        GameObject[] coins = FindObjectsOfType<GameObject>().Where(obj => obj.tag == "coin").ToArray();
        if (obstacles.Length > 0)
        {
            foreach (GameObject obj in obstacles)
            {
                Destroy(obj);
            }
            foreach (GameObject coin in coins)
            {
                Destroy(coin);
            }
        }
    }

    void ExitGame()
    {
        Application.Quit();
    }

    void Enter()
    {
        string name = startPanel.GetComponentInChildren<InputField>().text;
        if (name.Length > 10 || name.Length < 1)
        {
            //show message enter valid name
            StartCoroutine(ValidName());
        }
        else
        {
            nameEntered = true;
        }
    }

    IEnumerator ValidName()
    {
        var text = startPanel.transform.Find("InvalidNameText");
        text.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        text.gameObject.SetActive(false);
    }
}
