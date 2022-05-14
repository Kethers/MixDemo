using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    public float mouseSensitivity = 200.0f;

    [Header("Input Status")]
    public bool pauseKeyDown;
    public bool reloadKeyDown;
    public bool holsterKeyDown;
    public bool forwardWalkKey;
    public bool jumpBtnDown;
    public bool evacuateKeyDown;
    public bool runKey;
    public bool fireMouseBtn;

    public float horizontalAxisRaw, verticalAxisRaw;
    public float mouseX, mouseY;

    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {

    }


    void Update()
    {
        pauseKeyDown = Input.GetKeyDown(KeyCode.Escape);

        if (!WinLosePauseManager.Instance.isGamePaused && !WinLosePauseManager.Instance.isGameEnd)
        {
            // Key Down
            reloadKeyDown = Input.GetKeyDown(KeyCode.R);
            holsterKeyDown = Input.GetKeyDown(KeyCode.Q);
            evacuateKeyDown = Input.GetKeyDown(KeyCode.E);

            // Button Down
            jumpBtnDown = Input.GetButtonDown("Jump");

            // Key Hold
            forwardWalkKey = Input.GetKey(KeyCode.W);
            runKey = Input.GetKey(KeyCode.LeftShift);

            // Mouse Button
            fireMouseBtn = Input.GetMouseButton(0);

            // Mouse Movement
            mouseX = Input.GetAxis("Mouse X");
            mouseY = Input.GetAxis("Mouse Y");

            // Axis Raw Input (player movement)
            horizontalAxisRaw = Input.GetAxisRaw("Horizontal");
            verticalAxisRaw = Input.GetAxisRaw("Vertical");
        }

        else
        {
            reloadKeyDown = false;
            holsterKeyDown = false;
            evacuateKeyDown = false;

            // Button Down
            jumpBtnDown = false;

            // Key Hold
            runKey = false;
            forwardWalkKey = false;

            // Mouse Button
            fireMouseBtn = false;

            // Mouse Movement
            mouseX = 0f;
            mouseY = 0f;

            // Axis Raw Input (player movement)
            horizontalAxisRaw = 0f;
            verticalAxisRaw = 0f;
        }
    }
}
