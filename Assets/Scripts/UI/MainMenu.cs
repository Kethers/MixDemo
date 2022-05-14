using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

public class MainMenu : MonoBehaviour
{
    Button adventureBtn;
    Button survivalBtn;
    Button quitBtn;

    // PlayableDirector director;

    void Awake()
    {
        adventureBtn = transform.GetChild(1).GetComponent<Button>();
        survivalBtn = transform.GetChild(2).GetComponent<Button>();
        quitBtn = transform.GetChild(3).GetComponent<Button>();

        adventureBtn.onClick.AddListener(AdventureModeGame);
        survivalBtn.onClick.AddListener(SurvivalModeGame);
        quitBtn.onClick.AddListener(QuitGame);

        // director = FindObjectOfType<PlayableDirector>();
        // director.stopped += NewGame;
    }

    // void PlayTimeline()
    // {
    //     director.Play();
    // }

    // void NewGame(PlayableDirector obj)
    // {
    //     PlayerPrefs.DeleteAll();
    //     // Switch scene
    //     SceneController.Instance.TransitionToFirstLevel();
    // }

    // void ContinueGame()
    // {
    //     // Switch scene and load data
    //     SceneController.Instance.TransitionToLoadGame();
    // }

    void AdventureModeGame()
    {
        SceneController.Instance.TransitionToAdventureLevel();
    }

    void SurvivalModeGame()
    {
        SceneController.Instance.TransitionToSurvivalLevel();
    }

    void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit the Game");
    }
}
