using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerAnimator))]
public class PlayerSowAbility : MonoBehaviour
{
    [Header(" Elementos ")]
    private PlayerAnimator playerAnimator;
    void Start()
    {
        playerAnimator = GetComponent<PlayerAnimator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CropField"))
        {
            playerAnimator.PlaySowAnimation();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CropField"))
        {
            playerAnimator.StopSowAnimation();
        }
    }
}
