using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuyerInteractor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Buyer"))
        {
            TriggeredBuyer();
        }
    }
    private void TriggeredBuyer()
    {
        Debug.Log("Vendiendo");
    }
    private void SellCrops()
    {

    }
}
