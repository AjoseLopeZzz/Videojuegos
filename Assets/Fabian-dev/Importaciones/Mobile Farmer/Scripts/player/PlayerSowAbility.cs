using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerAnimator))]
[RequireComponent(typeof(PlayerToolSelector))]
public class PlayerSowAbility : MonoBehaviour
{
    [Header(" Elementos ")]
    private PlayerAnimator playerAnimator;
    [Header(" Configuraciones")]
    private CropField currentCropField;
    private PlayerToolSelector playerToolSelector;
    void Start()
    {
        playerAnimator = GetComponent<PlayerAnimator>();
        playerToolSelector = GetComponent<PlayerToolSelector>();

        SeedParticles.onSeedsCollided += SeedsCollidedCallback;

        CropField.onFullySown += CropFieldFullySownCallback;

        playerToolSelector.onToolSelected += ToolSelectedCallback;
    }
    private void OnDestroy()
    {
        SeedParticles.onSeedsCollided -= SeedsCollidedCallback;
        CropField.onFullySown -= CropFieldFullySownCallback;
        playerToolSelector.onToolSelected -= ToolSelectedCallback;
    }
    private void ToolSelectedCallback(PlayerToolSelector.Tool selectedTool)
    {
        if (!playerToolSelector.CanSow())
            playerAnimator.StopSowAnimation();
    }

  
    private void CropFieldFullySownCallback(CropField cropField)
    {
        if (cropField == currentCropField)
            playerAnimator.StopSowAnimation();
    }
    void SeedsCollidedCallback(Vector3[] seedPositions)
    {
        if(currentCropField == null)
        {
            return;
        }
        currentCropField.SeedsCollidedCallback(seedPositions);
    }
    private void OnTriggerEnter(Collider other)
    {           
        if (other.CompareTag("CropField") && other.GetComponent<CropField>().IsEmpty())
        {     
            currentCropField = other.GetComponent<CropField>();
            EnteredCropField(currentCropField);
        }
    }
    private void EnteredCropField(CropField cropField)
    {
        if(playerToolSelector.CanSow())
            playerAnimator.PlaySowAnimation();
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("CropField") && other.GetComponent<CropField>().IsEmpty())
            EnteredCropField(other.GetComponent<CropField>());
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CropField"))
        {
            playerAnimator.StopSowAnimation();
            currentCropField = null;
        }
    }
}
