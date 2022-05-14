using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    public Button continueBtn;
    public Button backToMainMenuBtn;

    private void Awake()
    {
        continueBtn = transform.GetChild(2).GetComponent<Button>();
        backToMainMenuBtn = transform.GetChild(3).GetComponent<Button>();

        continueBtn.onClick.AddListener(PausedContinue);
        backToMainMenuBtn.onClick.AddListener(PausedBackToMainMenu);
    }

    private void OnEnable()
    {
        Time.timeScale = 0f;
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;
    }

    void Start()
    {

    }


    void Update()
    {

    }

    void PausedContinue()
    {
        WinLosePauseManager.Instance.isGamePaused = false;
    }

    void PausedBackToMainMenu()
    {
        WinLosePauseManager.Instance.isGamePaused = false;
        WinLosePauseManager.Instance.isGameEnd = true;
        SceneController.Instance.TransitionToMain();
    }
}
