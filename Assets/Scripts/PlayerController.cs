using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
    [SerializeField] Text scoreText;
    [SerializeField] Text highScoreText;
    public Animator anim;
    ParticleSystem rocketFire;
    int score = 0;
    int highScore = 0;
    public event Action OnDead;
    public event Action OnPause;
    public bool paused;
    AudioSource scoreSound;

    private float timer;
    Text timerText;

    void Start()
    {
        timerText = GameObject.Find("Timer").GetComponent<Text>();
        timerText.text = "";
        scoreText.text = "Score: 0";
        highScore = PlayerPrefs.GetInt("highscore", highScore);
        highScoreText.text = "Highscore: " + highScore.ToString(); ;
        anim = GetComponent<Animator>();
        //anim.enabled = false;
        rocketFire = GetComponentInChildren<ParticleSystem>();
        rocketFire.gameObject.SetActive(false);
        scoreSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    //Mathf.FloorToInt(Time.timeSinceLevelLoad)
    public void HandleUpdate()
    {
        if (!paused)
        {
            DOTween.Play(transform);
            timer += Time.deltaTime;
            timerText.text = Mathf.Round(timer * 1f).ToString();
            score += Mathf.RoundToInt(timer);
            scoreText.text = "Score " + score;
            if (transform.position.y > 0.1)
            {
                anim.SetBool("Grounded", false);
            }
            else
            {
                anim.SetBool("Grounded", true);
            }
            //anim.enabled = true;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (transform.position.y > 0)
                {
                    Debug.Log("cant jump twice in a row");
                    anim.SetBool("Grounded", false);
                }
                else
                {
                    StartCoroutine(PlayFire());
                    transform.DOLocalJump(transform.position, 2f, 1, 1.5f);
                    anim.SetTrigger("Jump");
                    anim.SetBool("Grounded", true);
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            OnPause();
            paused = true;
            DOTween.Pause(transform);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("collision");
        if(collision.gameObject.tag == "obstacle")
        {
            //anim.enabled = false;
            anim.SetBool("isDead", true);
            OnDead();

            //set gamestate off
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("trigger");
        if (collision.gameObject.tag == "obstacle" || collision.gameObject.tag == "bird")
        {
            //anim.enabled = false;

            OnDead();
            scoreText.text = "Score: " + 0;
            if (score > highScore)
            {
                highScore = score;
                highScoreText.text = "highscore: " + highScore;
                PlayerPrefs.SetInt("highscore", highScore);
            }
            score = 0;
            timer = 0;
            anim.SetTrigger("died");
            anim.SetBool("isDead", true);


            //set gamestate off
        }
        else if(collision.gameObject.tag == "scorecollider")
        {
            score += 1000;
            scoreText.text = "Score: " + score;
            scoreSound.Play();
            collision.gameObject.SetActive(false);
        }
    }

    IEnumerator PlayFire()
    {
        rocketFire.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        rocketFire.gameObject.SetActive(false);
    }
}
