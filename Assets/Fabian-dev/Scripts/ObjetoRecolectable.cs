using UnityEngine;

public class ObjetoRecolectable : MonoBehaviour
{
    public int cantidad = 6;
    public float escalaDePulsacion = 0.1f;
    public float duracionDePulso = 0.3f;
    public GameObject modeloSimple;
    public GameObject modeloComplejo;

    private float temporizadorDePulso;
    private Vector3 escalaOriginal; // Guarda la escala actual del objeto al inicio

    private void Start()
    {
        modeloSimple.SetActive(false);
        modeloComplejo.SetActive(true);

        // Guardamos la escala inicial del objeto
        escalaOriginal = transform.localScale;
    }

    private void Update()
    {
        temporizadorDePulso -= Time.deltaTime;

        // Calcula cuánto se agranda o reduce la escala según el tiempo restante del pulso
        float escalaExtra = escalaDePulsacion * Mathf.Max(temporizadorDePulso / duracionDePulso, 0);

        // Aplica la escala proporcional a la escala original
        transform.localScale = escalaOriginal * (1 + escalaExtra);
    }

    public void Recolectar()
    {
        cantidad--;
        temporizadorDePulso = duracionDePulso;

        if (cantidad == 0)
        {
            modeloSimple.SetActive(true);
            modeloComplejo.SetActive(false);
        }
    }
}
