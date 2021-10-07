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
    int deadBoolHash;
    [SerializeField] AudioSource scoreSound;
    [SerializeField] AudioSource rocketPack;
    [SerializeField] AudioSource hitSound;
    bool isJumping;
    public BoxCollider2D bCollider2d;
    [SerializeField] LeaderboardHandler leaderboard;
    // add input field for name
    [SerializeField] InputField playerName;

    private float timer;
    Text timerText;

    void Start()
    {
        timerText = GameObject.Find("Timer").GetComponent<Text>();
        timerText.text = "";
        scoreText.text = "Score: 0";
       // highScore = PlayerPrefs.GetInt("highscore", 0);
        highScoreText.text = "Highscore: " + highScore.ToString(); ;
        anim = GetComponent<Animator>();
        //anim.enabled = false;
        rocketFire = GetComponentInChildren<ParticleSystem>();
        rocketFire.gameObject.SetActive(false);
        scoreSound = GetComponent<AudioSource>();
        isJumping = false;
        bCollider2d = GetComponent<BoxCollider2D>();
    }

    private void Awake()
    {
        deadBoolHash = Animator.StringToHash("isDead");
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
            if (transform.position.y > -2.65)
            {
                anim.SetBool("Grounded", false);
            }
            else
            {
                anim.SetBool("Grounded", true);
            }
            //anim.enabled = true;
            if (Input.GetKeyDown(KeyCode.Space) || isJumping)
            {
                if (transform.position.y > -2.65)
                {
                    anim.SetBool("Grounded", false);
                }
                else
                {
                    StartCoroutine(PlayFire());
                    transform.DOLocalJump(transform.position, 2.5f, 1, 1.8f);
                    anim.SetTrigger("Jump");
                    anim.SetBool("Grounded", true);
                    rocketPack.Play();
                }
            }
            isJumping = false;
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            OnPause();
            paused = true;
            DOTween.Pause(transform);
        }
    }

    private void OnMouseDown()
    {
        isJumping = true;
    }
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    Debug.Log("collision");
    //    if(collision.gameObject.tag == "obstacle")
    //    {
    //        anim.SetBool("isDead", true);
    //        OnDead();
    //    }
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "obstacle" || collision.gameObject.tag == "bird")
        {
            anim.SetTrigger("died");
            anim.SetBool(deadBoolHash, true);
            OnDead();
            scoreText.text = "Score: " + 0;
            if (score > highScore)
            {
                highScore = score;
                highScoreText.text = "highscore: " + highScore;
                //PlayerPrefs.SetInt("highscore", highScore);
                //use name input field from start
                leaderboard.AddHighScore(new LeaderboardScore(playerName.text, highScore));
            }
            score = 0;
            timer = 0;
            hitSound.Play();
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
        yield return new WaitForSeconds(.6f);
        rocketFire.gameObject.SetActive(false);
    }
}
