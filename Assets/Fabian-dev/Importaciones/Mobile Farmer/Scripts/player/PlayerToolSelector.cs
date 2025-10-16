using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerToolSelector : MonoBehaviour
{
    public enum Tool { None, Sow, Water, Harvest }
    private Tool activeTool;

    [Header("Elementos")]
    [SerializeField] private Image[] toolImages;

    [Header("Configuraciones")]
    [SerializeField] private Color selectedToolColor = Color.yellow;

    [Header("Acciones")]
    public Action<Tool> onToolSelected;

    void Start()
    {
        SelectTool(0);
    }

    void Update()
    {
        // Teclas numéricas superiores y del teclado numérico
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
            SelectTool((int)Tool.Sow);
        else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
            SelectTool((int)Tool.Water);
        else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
            SelectTool((int)Tool.Harvest);
        else if (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Keypad0))
            SelectTool((int)Tool.None);
    }

    public void SelectTool(int toolIndex)
    {
        activeTool = (Tool)toolIndex;

        for (int i = 0; i < toolImages.Length; i++)
        {
            toolImages[i].color = i == toolIndex ? selectedToolColor : Color.white;
        }

        onToolSelected?.Invoke(activeTool);
    }

    public bool CanSow() => activeTool == Tool.Sow;
    public bool CanWater() => activeTool == Tool.Water;
    public bool CanHarvest() => activeTool == Tool.Harvest;
}
