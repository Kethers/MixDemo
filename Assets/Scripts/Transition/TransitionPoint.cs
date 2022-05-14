using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionPoint : MonoBehaviour
{
    public enum TransitionType
    {
        SameScene, DifferentScene
    }

    [Header("Transition Info")]
    public string sceneName;
    public TransitionType transitionType;

    public TransitionDestination.DestinationTag destinationTag;

    private bool canTrans;

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Room" && destinationTag == TransitionDestination.DestinationTag.ENTER)
        {
            Destroy(gameObject, 2f);
        }
    }

    void Update()
    {
        if (InputManager.Instance.evacuateKeyDown
            && canTrans
            && destinationTag == TransitionDestination.DestinationTag.EXIT)
        {
            // SceneController transition
            // SceneController.Instance.TransitionToDestination(this);
            if (SceneManager.GetActiveScene().name == "Game")
            {
                WinLosePauseManager.Instance.target_2_Finished = true;
            }
            else if (SceneManager.GetActiveScene().name == "Room")
            {
                WinLosePauseManager.Instance.target_2_Finished = true;
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            canTrans = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            canTrans = false;
    }
}
