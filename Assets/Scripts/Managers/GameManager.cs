using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : Singleton<GameManager>
{
    public CharacterStats playerStats;
    private bool haveNotified;

    // private CinemachineFreeLook followCamera;

    List<IEndGameObserver> endGameObservers = new List<IEndGameObserver>();

    protected override void Awake()
    {
        base.Awake();
        haveNotified = false;
        Application.targetFrameRate = 144;
        // DontDestroyOnLoad(this);
    }

    public void RegisterPlayer(CharacterStats player)
    {
        playerStats = player;

        // Reset gunData (bullet nums)
        var guns = FindObjectsOfType<WeaponController>(true);
        foreach (var gun in guns)
        {
            if (gun.gunData.gunName == "Frozer")
            {
                gun.gunData.backupBullets = 28;
                gun.gunData.currentBullets = 7;
            }
            else if (gun.gunData.gunName == "Rifle")
            {
                gun.gunData.backupBullets = 90;
                gun.gunData.currentBullets = 30;
            }
        }

        // followCamera = FindObjectOfType<CinemachineFreeLook>();

        // if (followCamera != null)
        // {
        //     followCamera.Follow = playerStats.transform.GetChild(2);
        //     followCamera.LookAt = playerStats.transform.GetChild(2);
        // }
    }

    public void AddObserver(IEndGameObserver observer)
    {
        endGameObservers.Add(observer);
    }

    public void RemoveObserver(IEndGameObserver observer)
    {
        endGameObservers.Remove(observer);
    }

    public void GameLoseNotifyObservers()
    {
        if (haveNotified)
            return;
        haveNotified = true;
        foreach (var observer in endGameObservers)
        {
            observer.PlayerLoseNotify();
        }
    }

    public void GameWinNotifyObservers()
    {
        if (haveNotified)
            return;
        haveNotified = true;
        foreach (var observer in endGameObservers)
        {
            observer.PlayerWinNotify();
        }
    }

    public Transform GetEntrance()
    {
        foreach (var destination in FindObjectsOfType<TransitionDestination>())
        {
            if (destination.destinationTag == TransitionDestination.DestinationTag.ENTER)
                return destination.transform;
        }
        return null;
    }
}
