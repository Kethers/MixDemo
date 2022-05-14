using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [Header("Arm Sway")]
    public float swayAmplitude;
    public float smoothswayAmplitude;
    public float maxswayAmplitude;
    private Vector3 originPos;
    private PlayerController playerController;

    private void Awake()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    void Start()
    {
        originPos = transform.localPosition;
    }

    void Update()
    {
        ArmSway();
    }

    void ArmSway()
    {
        if (!WinLosePauseManager.Instance.isGamePaused && !WinLosePauseManager.Instance.isGameEnd)
        {
            float movementX = Mathf.Clamp(Input.GetAxis("Mouse X") * swayAmplitude, -maxswayAmplitude, maxswayAmplitude);
            float movementY = Mathf.Clamp((Input.GetAxis("Mouse Y") + playerController.playerVelocity.y) * swayAmplitude, -maxswayAmplitude, maxswayAmplitude);

            Vector3 newPos = new Vector3(-movementX, -movementY, 0);
            transform.localPosition = Vector3.Lerp(transform.localPosition, originPos + newPos, Time.deltaTime * smoothswayAmplitude);
        }
    }
}
