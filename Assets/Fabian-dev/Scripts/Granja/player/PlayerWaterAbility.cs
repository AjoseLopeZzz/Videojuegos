using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerAnimator))]
[RequireComponent(typeof(PlayerToolSelector))]
public class PlayerWaterAbility : MonoBehaviour
{
    [Header("Elementos")]
    private PlayerAnimator playerAnimator;
    private PlayerToolSelector playerToolSelector;

    [Header("Configuraciones")]
    private CropField currentCropField;

    void Start()
    {
        playerAnimator = GetComponent<PlayerAnimator>();
        playerToolSelector = GetComponent<PlayerToolSelector>();

        WaterParticles.onWaterCollided += WaterCollidedCallback;
        CropField.onFullyWatered += CropFieldFullyWateredCallback;
        playerToolSelector.onToolSelected += ToolSelectedCallback;
    }

    private void OnDestroy()
    {
        WaterParticles.onWaterCollided -= WaterCollidedCallback;
        CropField.onFullyWatered -= CropFieldFullyWateredCallback;
        playerToolSelector.onToolSelected -= ToolSelectedCallback;
    }

    private void ToolSelectedCallback(PlayerToolSelector.Tool selectedTool)
    {
        if (!playerToolSelector.CanWater())
            playerAnimator.StopWaterAnimation();
    }

    private void CropFieldFullyWateredCallback(CropField cropField)
    {
        if (cropField == currentCropField)
            playerAnimator.StopWaterAnimation();
    }

    private void WaterCollidedCallback(Vector3[] waterPositions)
    {
        if (currentCropField == null)
            return;

        currentCropField.WaterCollidedCallback(waterPositions);
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("CropField"))
        {
            currentCropField = other.GetComponent<CropField>();
            EnteredCropField(currentCropField);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        
        if (other.CompareTag("CropField"))
        {
            EnteredCropField(other.GetComponent<CropField>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CropField"))
        {
            playerAnimator.StopWaterAnimation();
            currentCropField = null;
        }
    }

    private void EnteredCropField(CropField cropField)
    {
        if (playerToolSelector.CanWater())
        {
            if (currentCropField == null)
                currentCropField = cropField;

            playerAnimator.PlayWaterAnimation();
        }
    }
}
