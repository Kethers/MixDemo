using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinLoseUI : MonoBehaviour
{
    public Button backToMainMenuBtn;

    private void Awake()
    {
        backToMainMenuBtn = transform.GetChild(3).GetComponent<Button>();

        backToMainMenuBtn.onClick.AddListener(BackToMainMenu);
    }

    void Start()
    {

    }


    void Update()
    {

    }

    void BackToMainMenu()
    {
        WinLosePauseManager.Instance.StopWinLoseSound();
        SceneController.Instance.TransitionToMain();
    }
}
