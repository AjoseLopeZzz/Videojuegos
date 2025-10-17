using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class CropTIle : MonoBehaviour
{
    public TileFieldState state;

    [Header(" Elementos")]
    [SerializeField] private Transform cropParent;
    [SerializeField] private MeshRenderer tileRenderer;
    private Crop crop;
    private CropData cropData;

    [Header("Eventos")]
    public static Action<CropType> onCropHarvested;

    void Start()
    {
        state = TileFieldState.Empty;
    }


    public void Sow(CropData cropData)
    {
        state = TileFieldState.Sown;

        crop = Instantiate(cropData.cropPrefab, transform.position, Quaternion.identity, cropParent);

        this.cropData = cropData;
    }
    public void Harvest()
    {
        state = TileFieldState.Empty;
        crop.ScaleDown();

        StartCoroutine(ColorTileCoroutine(1f));

        onCropHarvested?.Invoke(cropData.cropType);
    }
    public void Water()
    {
        state = TileFieldState.Watered;
        crop.ScaleUp();
        StartCoroutine(ColorTileCoroutine(.3f));
    }

    IEnumerator ColorTileCoroutine(float colorSuelo)
    {
        float duration = 1f;
        float timer = 0f;

        // punto de partida = el color actual del material
        Color startColor = tileRenderer.material.color;
        
        Color targetColor = Color.white * colorSuelo;

        while (timer < duration)
        {
            float t = timer / duration;
            Color lerpedColor = Color.Lerp(startColor, targetColor, t);
            tileRenderer.material.color = lerpedColor;
            timer += Time.deltaTime;
            yield return null;
        }

        // asegurar que quede en el color final exacto
        tileRenderer.material.color = targetColor;
    }

    public bool IsEmpty()
    {
        return state == TileFieldState.Empty;
    }
    public bool IsSown()
    {
        return state == TileFieldState.Sown;
    }
}