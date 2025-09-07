using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JugadorCollider : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<ObjetoRecolectable>() != null)
        {
            ObjetoRecolectable objeto = other.gameObject.GetComponent<ObjetoRecolectable>();
            Debug.Log("Deberia funcionar");
        }
    }
}
