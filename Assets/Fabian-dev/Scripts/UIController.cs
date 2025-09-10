using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public JugadorCollider jugadorCollider;
    public TextMeshProUGUI objetosTMP;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        objetosTMP.text = "Objetos: " + jugadorCollider.objetosRecolectados;
    }
}
