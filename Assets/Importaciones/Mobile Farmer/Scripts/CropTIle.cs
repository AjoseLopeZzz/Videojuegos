using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileFieldState { Empty,Sown,Watered}
public class CropTIle : MonoBehaviour
{
    public TileFieldState state;

    [Header(" Elementos")]
    [SerializeField] private Transform cropParent;
    [SerializeField] private MeshRenderer tileRenderer;
    private Crop crop;
    void Start()
    {
        state = TileFieldState.Empty;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Sow(CropData cropData)
    {
        state = TileFieldState.Sown;

        crop = Instantiate(cropData.cropPrefab, transform.position, Quaternion.identity, cropParent);
    }
    public void Water()
    {
        state = TileFieldState.Watered;
        //tileRenderer.material.color = Color.white * .3f;
        crop.ScaleUp();
        StartCoroutine("ColorTileCoroutine");
    }

    IEnumerator ColorTileCoroutine()
    {
        float duration = 1;
        float timer = 0;

        while (timer < duration)
        {
            float t = timer / duration;
            Color lerpedColor = Color.Lerp(Color.white, Color.white * .3f, t);
            tileRenderer.material.color = lerpedColor;
            timer += Time.deltaTime;
            yield return null;
        }
        yield return null;
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
