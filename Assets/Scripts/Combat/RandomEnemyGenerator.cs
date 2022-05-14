using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEnemyGenerator : Singleton<RandomEnemyGenerator>
{
    public Transform roomCenter;
    public Transform[] spawnPoints;
    public Grunt gruntPrefab;
    public float spawnEnemyTimeGap;
    private float spawnEnemyTimer;

    protected override void Awake()
    {
        base.Awake();
        spawnEnemyTimeGap = 4f;
        spawnEnemyTimer = spawnEnemyTimeGap;
    }

    void Start()
    {
        // spawnPoints = new Transform[3];
    }

    void Update()
    {
        if (!WinLosePauseManager.Instance.isGameEnd && !WinLosePauseManager.Instance.isGamePaused)
        {
            if (spawnEnemyTimer <= 0f)
            {
                spawnEnemyTimer = spawnEnemyTimeGap;
                int randomInt = Random.Range(0, 3);
                Transform spawnPoint = spawnPoints[randomInt];
                Vector3 facingDirection = roomCenter.position - spawnPoint.position;
                Instantiate(gruntPrefab, spawnPoint.position, Quaternion.FromToRotation(gruntPrefab.transform.forward, facingDirection));
            }
            else
            {
                spawnEnemyTimer -= Time.deltaTime;
            }
        }
    }
}
