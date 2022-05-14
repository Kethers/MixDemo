using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    public GameObject rifle;
    public GameObject handgun;
    public GameObject activeGun;
    private WeaponController activeGunController;
    private bool isHandgunActive;

    private void Awake()
    {
        activeGun = rifle;
    }

    private void Start()
    {
        isHandgunActive = false;
        activeGun = rifle;
    }

    private void Update()
    {
        activeGunController = activeGun.GetComponent<WeaponController>();
        if (Input.GetKeyDown(KeyCode.Q)
            && !activeGunController.isReloading
            && !WinLosePauseManager.Instance.isGamePaused
            && !WinLosePauseManager.Instance.isGameEnd)
        {
            activeGunController.SwitchGun();
            activeGun.SetActive(false);
            if (isHandgunActive)
            {
                activeGun = rifle;
                isHandgunActive = false;
            }
            else
            {
                activeGun = handgun;
                isHandgunActive = true;
            }
            activeGun.SetActive(true);
        }
    }
}
