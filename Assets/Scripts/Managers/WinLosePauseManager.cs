using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class WinLosePauseManager : Singleton<WinLosePauseManager>, IEndGameObserver
{
    public GameObject victoryCanvas, defeatCanvas, pauseCanvas;
    public bool isGamePaused, isGameEnd;
    public AudioClip victorySound, defeatSound;
    private AudioSource audioSource;
    public GameObject adventureTarget, SurvivalTarget;
    public float survivalTime;
    public bool target_1_Finished, target_2_Finished;

    protected override void Awake()
    {
        base.Awake();

        audioSource = GetComponent<AudioSource>();
        victoryCanvas.SetActive(false);
        defeatCanvas.SetActive(false);
        pauseCanvas.SetActive(false);
        isGamePaused = false;
        survivalTime = 30f;
        target_1_Finished = target_2_Finished = false;
    }

    void Start()
    {
        GameManager.Instance.AddObserver(this);
    }

    void Update()
    {
        GamePause();
        WinJudge();
    }

    void WinJudge()
    {
        if (SceneManager.GetActiveScene().name == "Game")
        {
            if (KillAllEnemy())
            {
                adventureTarget.transform.GetChild(1).GetComponent<Text>().color = Color.green;
                target_1_Finished = true;
            }
            if (target_2_Finished)
            {
                adventureTarget.transform.GetChild(2).GetComponent<Text>().color = Color.green;
            }
        }

        else if (SceneManager.GetActiveScene().name == "Room")
        {
            var timeText = SurvivalTarget.transform.GetChild(1).GetComponent<Text>();
            var target_2_Text = SurvivalTarget.transform.GetChild(2).GetComponent<Text>();
            if (survivalTime <= 0f)
            {
                survivalTime = 0f;
                timeText.color = Color.green;
                target_1_Finished = true;
                GameObject.Find("Portals").transform.Find("Portal EXIT").gameObject.SetActive(true);
                target_2_Text.text = "从传送门撤离";
            }
            else
            {
                survivalTime -= Time.deltaTime;
            }
            timeText.text = "剩余充能时间：" + (int)survivalTime + "s";

            if (target_2_Finished)
            {
                target_2_Text.color = Color.green;
            }
        }

        if (target_1_Finished && target_2_Finished)
        {
            GameManager.Instance.GameWinNotifyObservers();
        }
    }

    public void GamePause()
    {
        if (InputManager.Instance.pauseKeyDown)
        {
            isGamePaused = !isGamePaused;
        }

        if (isGamePaused && !isGameEnd)
        {
            pauseCanvas.SetActive(true);
        }
        else
        {
            pauseCanvas.SetActive(false);
        }
    }

    public void PlayerLoseNotify()
    {
        isGameEnd = true;
        defeatCanvas.SetActive(true);
        audioSource.clip = defeatSound;
        PlayWinLoseSound();
    }

    public void PlayerWinNotify()
    {
        isGameEnd = true;
        victoryCanvas.SetActive(true);
        audioSource.clip = victorySound;
        PlayWinLoseSound();
    }

    void PlayWinLoseSound()
    {
        audioSource.Play();
    }

    public void StopWinLoseSound()
    {
        audioSource.Stop();
    }

    public bool KillAllEnemy()
    {
        return GameObject.FindGameObjectsWithTag("Enemy").Length == 0;
    }
}
