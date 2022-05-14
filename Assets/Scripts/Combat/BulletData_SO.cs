using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Bullet", menuName = "Attack/Bullet Data")]
public class BulletData_SO : ScriptableObject
{
    public string bulletName;
    public int shootRange;
    public int minDamage;
    public int maxDamage;
}
