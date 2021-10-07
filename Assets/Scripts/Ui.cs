using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class Ui : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Button startButton;
    [SerializeField] Button resumebutton;
    [SerializeField] LeaderboardHandler leaderBoard;
    [SerializeField] Button howToPlayButton;
    [SerializeField] Button exitButton;
    [SerializeField] GameObject menuPanel;
    [SerializeField] GameObject pauseMenuPanel;
    public event Action OnStart;
    public event Action OnExit;
    public event Action OnResume;

    private void Start()
    {
        startButton = startButton.GetComponent<Button>();
        resumebutton = resumebutton.GetComponent<Button>();
        exitButton = exitButton.GetComponent<Button>();
        startButton.onClick.AddListener(started);
        exitButton.onClick.AddListener(exit);
        resumebutton.onClick.AddListener(resume);
    }

    public void EnableMenu()
    {
        menuPanel.SetActive(true);
        //leaderBoard.ConnectToPostgres();
    }

    public void DisableMenu()
    {
        menuPanel.SetActive(false);
    }
    public void EnablePauseMenu()
    {
        pauseMenuPanel.SetActive(true);
    }

    public void DisablePauseMenu()
    {
        pauseMenuPanel.SetActive(false);
    }

    void started()
    {
        OnStart();
    }

    void exit()
    {
        OnExit();
    }

    void resume()
    {
        OnResume();
    }
}
