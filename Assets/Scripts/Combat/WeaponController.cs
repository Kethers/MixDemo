using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class WeaponController : MonoBehaviour
{
    public Transform shootPoint;

    public GunData_SO gunData;
    public BulletData_SO bulletData;

    public GameObject casingPrefab;

    [HideInInspector] public bool fireInput;
    [HideInInspector] public bool reloadInput;
    [HideInInspector] public bool holsterInput;
    private float fireSinceLastTime;

    [Header("Gun Fire Light")]
    public ParticleSystem muzzleFlash;
    public Light muzzleFlashLight;

    [Header("Hit Particles")]
    public GameObject hitParticle;
    public GameObject bulletHole;

    [Header("Gun Sound")]
    private AudioSource audioSource;
    public AudioClip gunSoundClip;
    public AudioClip reloadAmmoLeftClip;
    public AudioClip reloadOutOfAmmoClip;

    private Animator anim;
    private PlayerController playerController;
    [HideInInspector] public bool isReloading, isFiring, isHolster;

    public float freezingDamageMultiplier;

    void Awake()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        isReloading = isFiring = isHolster = false;
        freezingDamageMultiplier = 20f;
    }

    void Start()
    {
        gunData.currentBullets = gunData.bulletsPerMag;
    }

    void InputProcess()
    {
        if (!WinLosePauseManager.Instance.isGamePaused && !WinLosePauseManager.Instance.isGameEnd)
        {
            fireInput = Input.GetMouseButton(0);
            reloadInput = Input.GetKeyDown(KeyCode.R);
            holsterInput = Input.GetKeyDown(KeyCode.Q);
        }
    }

    void Update()
    {
        InputProcess();

        // can fire when state is idle/walk/run
        if (fireInput && !isReloading && !isHolster && !playerController.isRun)
        {
            Fire();
        }
        else
        {
            muzzleFlashLight.enabled = false;
        }

        if (reloadInput && gunData.currentBullets < gunData.bulletsPerMag && gunData.backupBullets > 0)
        {
            Reload();
        }

        fireSinceLastTime += fireSinceLastTime < gunData.fireGap ? Time.deltaTime : 0f;

        SwitchAnimation();
    }

    void Fire()
    {
        if (fireSinceLastTime < gunData.fireGap || gunData.currentBullets <= 0)
            return;

        anim.SetTrigger("Fire");

        Vector3 shootDirection = shootPoint.forward;
        RaycastHit hit;
        if (Physics.Raycast(shootPoint.position, shootDirection, out hit, bulletData.shootRange))
        {
            // Debug.Log(hit.transform.name + " hit!");
            var hitParticleEffect = Instantiate(hitParticle, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
            var bulletHoleEffect = Instantiate(bulletHole, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));

            Destroy(hitParticleEffect, 1f);
            Destroy(bulletHoleEffect, 3f);
            TakeDamageToEnemy(ref hit);
        }

        audioSource.clip = gunSoundClip;
        PlaySound();

        muzzleFlash.Play();
        muzzleFlashLight.enabled = true;

        gunData.currentBullets--;
        fireSinceLastTime = 0f;
    }

    void Reload()
    {
        if (gunData.backupBullets <= 0)
            return;

        // out of ammo reload
        if (gunData.currentBullets == 0 && !isReloading)
        {
            anim.Play("Reload Out Of Ammo", 0, 0f);
            audioSource.clip = reloadOutOfAmmoClip;
            PlaySound();
        }
        else if (gunData.currentBullets > 0 && !isReloading)
        {
            anim.Play("Reload Ammo Left", 0, 0f);
            audioSource.clip = reloadAmmoLeftClip;
            PlaySound();
        }
    }

    void PlaySound()
    {
        audioSource.Play();
    }

    void SwitchAnimation()
    {
        anim.SetBool("Walk", playerController.isWalk);
        anim.SetBool("Run", playerController.isRun);
        // anim.SetBool("Fire", isFiring);
        // anim.SetBool("Holster", isHolster);
    }

    public void SwitchGun()
    {
        anim.Play("Holster", 0, 0f);
    }

    void TakeDamageToEnemy(ref RaycastHit hit)
    {
        if (hit.collider.gameObject.CompareTag("Enemy"))
        {
            EnemyController enemyController = hit.collider.gameObject.GetComponent<EnemyController>();
            CharacterStats enemyStats = enemyController.characterStats;
            if (gunData.gunName == "Frozer")    // shot by handgun
            {
                enemyStats.TakeDamage(Random.Range(bulletData.minDamage, bulletData.maxDamage), enemyStats);
                enemyController.Freeze();
            }
            else // shot by rifle
            {
                if (enemyController.isFrozen)
                {
                    enemyStats.TakeDamage((int)Random.Range(freezingDamageMultiplier * bulletData.minDamage, freezingDamageMultiplier * bulletData.maxDamage), enemyStats);
                    enemyController.UnFreeze(true);
                }
                else
                {
                    enemyStats.TakeDamage(Random.Range(bulletData.minDamage, bulletData.maxDamage), enemyStats);
                }
            }
        }
    }

}
