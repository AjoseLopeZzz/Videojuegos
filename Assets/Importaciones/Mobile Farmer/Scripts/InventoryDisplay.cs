using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryDisplay : MonoBehaviour
{
    [Header("Elementos")]
    [SerializeField] private Transform cropContainersParent;
    [SerializeField] private UICropContainer uiCropContainerPrefab;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Configure(Inventory inventory)
    {
        InventoryItem[] items = inventory.GetInventoryItems();

        for (int i = 0; i < items.Length; i++)
        {
            UICropContainer cropContainerInstance = Instantiate(uiCropContainerPrefab, cropContainersParent);

            Sprite cropIcon=DataManager.instance.GetCropSpriteFromCropType(items[i].cropType);
            cropContainerInstance.Configure(cropIcon,items[i].amount);
        }
    }
}
