using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    #region deprecated rpg code
    // private NavMeshAgent agent;
    // private Animator anim;
    // private CharacterStats characterStats;

    // private GameObject attackTarget;
    // private float lastAttackTime;
    // private bool isDead;

    // private float stopDistance;

    // void Awake()
    // {
    //     agent = GetComponent<NavMeshAgent>();
    //     anim = GetComponent<Animator>();
    //     characterStats = GetComponent<CharacterStats>();

    //     stopDistance = agent.stoppingDistance;
    // }

    // void OnEnable()
    // {
    //     MouseManager.Instance.OnMouseClicked += MoveToTarget;
    //     MouseManager.Instance.OnEnemyClicked += EventAttack;
    //     GameManager.Instance.RegisterPlayer(characterStats);
    // }

    // void Start()
    // {
    //     SaveManager.Instance.LoadPlayerData();
    // }

    // void OnDisable()
    // {
    //     MouseManager.Instance.OnMouseClicked -= MoveToTarget;
    //     MouseManager.Instance.OnEnemyClicked -= EventAttack;
    // }

    // void Update()
    // {
    //     isDead = characterStats.CurrentHealth == 0;

    //     if (isDead)
    //         GameManager.Instance.NotifyObservers();

    //     SwitchAnimation();

    //     lastAttackTime -= Time.deltaTime;
    // }

    // private void SwitchAnimation()
    // {
    //     anim.SetFloat("Speed", agent.velocity.sqrMagnitude);
    //     anim.SetBool("Death", isDead);
    // }

    // public void MoveToTarget(Vector3 target)
    // {
    //     StopAllCoroutines();
    //     if (isDead) return;

    //     agent.stoppingDistance = stopDistance;
    //     agent.isStopped = false;
    //     agent.destination = target;
    // }

    // private void EventAttack(GameObject target)
    // {
    //     if (isDead) return;

    //     if (target != null)
    //     {
    //         attackTarget = target;
    //         characterStats.isCritical = UnityEngine.Random.value < characterStats.attackData.criticalChance;
    //         StartCoroutine(MoveToAttackTarget());
    //     }
    // }

    // IEnumerator MoveToAttackTarget()
    // {
    //     agent.isStopped = false;
    //     agent.stoppingDistance = characterStats.attackData.attackRange;
    //     transform.LookAt(attackTarget.transform);

    //     while (Vector3.Distance(attackTarget.transform.position, transform.position) > characterStats.attackData.attackRange)
    //     {
    //         agent.destination = attackTarget.transform.position;
    //         yield return null;
    //     }

    //     agent.isStopped = true;

    //     // Attack
    //     if (lastAttackTime < 0)
    //     {
    //         anim.SetBool("Critical", characterStats.isCritical);
    //         anim.SetTrigger("Attack");
    //         // Reset CD time
    //         lastAttackTime = characterStats.attackData.coolDown;
    //     }
    // }

    // // Animation Event
    // void Hit()
    // {
    //     if (attackTarget.CompareTag("Attackable"))
    //     {
    //         if (attackTarget.GetComponent<Rock>() && attackTarget.GetComponent<Rock>().rockStates == Rock.RockStates.HitNothing)
    //         {
    //             attackTarget.GetComponent<Rock>().rockStates = Rock.RockStates.HitEnemy;
    //             attackTarget.GetComponent<Rigidbody>().velocity = Vector3.one;
    //             attackTarget.GetComponent<Rigidbody>().AddForce(transform.forward * 20, ForceMode.Impulse);
    //         }
    //     }
    //     else
    //     {
    //         var targetStats = attackTarget.GetComponent<CharacterStats>();

    //         targetStats.TakeDamage(characterStats, targetStats);
    //     }
    // }
    #endregion

    private CharacterStats characterStats;
    // public float mouseSensitivity = 200.0f; // move it to input manager
    private float rotX = 20f;
    private float speed;
    public float walkSpeed = 5f, runSpeed = 10f;
    public Transform playerBody;
    private CharacterController characterController;
    private Transform cam;
    private bool isPlayerGrounded;
    private Vector3 moveDirection;
    private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;
    [HideInInspector] public Vector3 playerVelocity;
    private Transform groundCheckPoint;
    private float groundDistance = 0.1f;
    [HideInInspector] public bool isWalk, isRun, isJump, isDead;
    public float slopeForce = 6.0f; // Applied downforce when on slope
    public float slopeForceRayLength = 2.0f;

    [Header("Audio Settings")]
    private AudioSource audioSource;
    public AudioClip walkingSound;
    public AudioClip runningSound;

    private WeaponController weaponController;

    private void OnEnable()
    {
        // GameManager.Instance.RegisterPlayer(characterStats);
    }

    private void Awake()
    {
        characterStats = GetComponent<CharacterStats>();
        characterController = GetComponent<CharacterController>();
        groundCheckPoint = GameObject.Find("Player/Ground Check Point").GetComponent<Transform>();
        audioSource = GetComponent<AudioSource>();
        // groundMask = LayerMask.NameToLayer("Ground");
        cam = transform.GetChild(0);
        weaponController = transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<WeaponController>();
        speed = walkSpeed;
    }

    private void Start()
    {
        GameManager.Instance.RegisterPlayer(characterStats);
    }

    private void Update()
    {
        isDead = characterStats.CurrentHealth == 0;
        if (isDead)
        {
            Debug.Log("Player is dead!");
            GameManager.Instance.GameLoseNotifyObservers();
        }
        else
        {
            InputProcess();
            CameraRotation();
            PlayerMovement();
            Jump();
            PlaySound();
        }
    }

    void FixedUpdate()
    {
        if (Physics.CheckSphere(groundCheckPoint.position, groundDistance, LayerMask.GetMask("Ground")))
            isPlayerGrounded = true;
        else
            isPlayerGrounded = false;
    }

    void InputProcess()
    {
        isRun = InputManager.Instance.runKey && !weaponController.isReloading;
        isJump = InputManager.Instance.jumpBtnDown;
    }

    void PlayerMovement()
    {
        speed = isRun ? runSpeed : walkSpeed;

        float h = InputManager.Instance.horizontalAxisRaw;
        float v = InputManager.Instance.verticalAxisRaw;
        moveDirection = (transform.right * h + transform.forward * v).normalized;
        isWalk = moveDirection.sqrMagnitude > 0.9f;

        characterController.Move(moveDirection * speed * Time.deltaTime);

        if (IsOnSlope())
        {
            // Apply downforce
            characterController.Move(Vector3.down * characterController.height / 2 * slopeForce * Time.deltaTime);
        }
    }

    void CameraRotation()
    {
        float mouseX = InputManager.Instance.mouseX * InputManager.Instance.mouseSensitivity * Time.deltaTime;
        float mouseY = InputManager.Instance.mouseY * InputManager.Instance.mouseSensitivity * Time.deltaTime;

        rotX -= mouseY;
        rotX = Mathf.Clamp(rotX, -80f, 80f);
        Quaternion targetRotX = Quaternion.Euler(rotX, 0f, 0f);
        Quaternion targetRotY = Quaternion.Euler(0f, mouseX, 0f);

        // up and down rotation for camera
        cam.localRotation = Quaternion.Lerp(cam.localRotation, targetRotX, 0.4f);
        // left and right rotation for playerbody
        transform.Rotate(Vector3.up * mouseX);
    }

    void Jump()
    {
        if (isPlayerGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        if (isJump && isPlayerGrounded)
        {
            // Physics formula: v^2 = -2gh
            playerVelocity.y += Mathf.Sqrt(-2.0f * gravityValue * jumpHeight);
        }
        playerVelocity.y += gravityValue * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);
    }

    bool IsOnSlope()
    {
        if (isJump)
            return false;

        RaycastHit hit;
        // Cast a downward ray and check the normal of hitinfo
        if (Physics.Raycast(transform.position, Vector3.down, out hit, characterController.height / 2 * slopeForceRayLength))
        {
            if (hit.normal != Vector3.up)
            {
                return true;
            }
        }

        return false;
    }

    void PlaySound()
    {
        if (isPlayerGrounded && isWalk && !isJump)
        {
            audioSource.clip = isRun ? runningSound : walkingSound;
            if (!audioSource.isPlaying)
                audioSource.Play();
        }
        else
        {
            if (audioSource.isPlaying)
            {
                audioSource.Pause();
            }
        }
    }
}
