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
        float movementX = Mathf.Clamp(InputManager.Instance.mouseX * swayAmplitude, -maxswayAmplitude, maxswayAmplitude);
        float movementY = Mathf.Clamp((InputManager.Instance.mouseY + playerController.playerVelocity.y) * swayAmplitude, -maxswayAmplitude, maxswayAmplitude);

        Vector3 newPos = new Vector3(-movementX, -movementY, 0);
        transform.localPosition = Vector3.Lerp(transform.localPosition, originPos + newPos, Time.deltaTime * smoothswayAmplitude);
    }
}
