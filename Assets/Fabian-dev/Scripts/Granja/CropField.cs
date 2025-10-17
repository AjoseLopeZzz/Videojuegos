using System;
using System.Collections.Generic;
using UnityEngine;

public class CropField : MonoBehaviour
{
    [Header("Elementos")]
    [SerializeField] private Transform tilesParent;
    private List<CropTIle> cropTiles = new List<CropTIle>();

    [Header("Configuraciones")]
    [SerializeField] private CropData cropData;
    private TileFieldState state;
    private int tilesSown;
    private int tilesWatered;
    private int tilesHarvested;

    [Header("Acciones")]
    public static Action<CropField> onFullySown;
    public static Action<CropField> onFullyWatered;
    public static Action<CropField> onFullyHarvested;

    void Start()
    {
        state = TileFieldState.Empty;
        StoreTiles();
    }

    private void StoreTiles()
    {
        cropTiles.Clear();
        for (int i = 0; i < tilesParent.childCount; i++)
        {
            CropTIle tile = tilesParent.GetChild(i).GetComponent<CropTIle>();
            if (tile != null)
                cropTiles.Add(tile);
        }
    }

    public void SeedsCollidedCallback(Vector3[] seedPositions)
    {
        foreach (Vector3 seedPos in seedPositions)
        {
            CropTIle closestTile = GetClosestCropTile(seedPos);
            if (closestTile == null)
                continue;
            if (!closestTile.IsEmpty())
                continue;

            Sow(closestTile);
        }
    }

    private void Sow(CropTIle tile)
    {
        tile.Sow(cropData);
        tilesSown++;

        if (tilesSown >= cropTiles.Count)
            FieldFullySown();
    }

    public void WaterCollidedCallback(Vector3[] waterPositions)
    {
        foreach (Vector3 waterPos in waterPositions)
        {
            CropTIle closestTile = GetClosestCropTile(waterPos);
            if (closestTile == null)
                continue;
            if (!closestTile.IsSown())
                continue;

            Water(closestTile);
        }
    }

    private void Water(CropTIle tile)
    {
        tile.Water();
        tilesWatered++;

        if (tilesWatered >= cropTiles.Count)
            FieldFullyWatered();
    }

    private void FieldFullySown()
    {
        if (state != TileFieldState.Sown)
        {
            state = TileFieldState.Sown;
            onFullySown?.Invoke(this);
            Debug.Log("Campo completamente sembrado ");
        }
    }

    private void FieldFullyWatered()
    {
        if (state != TileFieldState.Watered)
        {
            state = TileFieldState.Watered;
            onFullyWatered?.Invoke(this);
            Debug.Log("Campo completamente regado ");
        }
    }

    public void Harvest(Transform harvestSphere)
    {
        float sphereRadius = harvestSphere.localScale.x;

        foreach (CropTIle tile in cropTiles)
        {
            if (tile.IsEmpty())
                continue;

            float dist = Vector3.Distance(harvestSphere.position, tile.transform.position);
            if (dist <= sphereRadius)
                HarvestTile(tile);
        }
    }

    private void HarvestTile(CropTIle tile)
    {
        tile.Harvest();
        tilesHarvested++;

        if (tilesHarvested >= cropTiles.Count)
            FieldFullyHarvested();
    }

    private void FieldFullyHarvested()
    {
        tilesSown = 0;
        tilesWatered = 0;
        tilesHarvested = 0;
        state = TileFieldState.Empty;

        onFullyHarvested?.Invoke(this);
        Debug.Log("Campo completamente cosechado ");
    }

    [NaughtyAttributes.Button]
    private void InstantlySowTiles()
    {
        foreach (CropTIle tile in cropTiles)
            Sow(tile);
    }

    [NaughtyAttributes.Button]
    private void InstantlyWaterTiles()
    {
        foreach (CropTIle tile in cropTiles)
            Water(tile);
    }

    private CropTIle GetClosestCropTile(Vector3 seedPosition)
    {
        float minDistance = float.MaxValue;
        CropTIle closestTile = null;

        foreach (CropTIle tile in cropTiles)
        {
            float dist = Vector3.Distance(tile.transform.position, seedPosition);
            if (dist < minDistance)
            {
                minDistance = dist;
                closestTile = tile;
            }
        }

        // Consejo adicional: asegúrate de que la semilla no esté demasiado lejos
        // para evitar falsos positivos (por ejemplo si rebota fuera del campo)
        if (closestTile != null && minDistance > 1.2f)
            return null;

        return closestTile;
    }

    public bool IsEmpty() => state == TileFieldState.Empty;
    public bool IsSown() => state == TileFieldState.Sown;
    public bool IsWatered() => state == TileFieldState.Watered;
}
