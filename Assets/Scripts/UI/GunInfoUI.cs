using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunInfoUI : MonoBehaviour
{
    private GunData_SO gunData;
    private GameObject activeGun;

    public Text bulletsCurrentBackupText;
    public GameObject rifleInfo;
    public GameObject handgunInfo;

    void Start()
    {

    }

    void Update()
    {
        activeGun = GameObject.Find("Player/Main Camera/Weapon Holder").GetComponent<WeaponSwitcher>().activeGun;
        gunData = activeGun.GetComponent<WeaponController>().gunData;
        if (gunData)
        {
            bulletsCurrentBackupText.text = gunData.currentBullets + " / " + gunData.backupBullets;
            if (gunData.gunName == "Frozer")
            {
                rifleInfo.SetActive(false);
                handgunInfo.SetActive(true);
            }
            else
            {
                handgunInfo.SetActive(false);
                rifleInfo.SetActive(true);
            }
        }

    }
}
