using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBox : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // private void OnCollisionEnter(Collision other)
    // {
    //     Debug.Log("in collision");
    //     if (other.gameObject.CompareTag("Player"))
    //     {
    //         GameManager.Instance.playerStats.CurrentHealth = 0;
    //     }
    //     else
    //     {
    //         Destroy(other.gameObject);
    //     }
    // }

    private void OnTriggerEnter(Collider other)
    {
        // Debug.Log("in trigger");
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.playerStats.CurrentHealth = 0;
        }
        else
        {
            Destroy(other.gameObject);
        }
    }
}
