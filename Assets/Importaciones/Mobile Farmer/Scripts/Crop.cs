using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crop : MonoBehaviour
{
    [Header("Elementos")]
    [SerializeField] private Transform cropRenderer;
    public void ScaleUp()
    {
        cropRenderer.localScale = Vector3.one;
    }
}
