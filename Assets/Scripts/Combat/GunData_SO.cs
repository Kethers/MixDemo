using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gun", menuName = "Attack/Gun Data")]
public class GunData_SO : ScriptableObject
{
    public string gunName;
    public int bulletsPerMag;
    public int currentBullets;
    public int backupBullets;
    public float fireGap;
}
