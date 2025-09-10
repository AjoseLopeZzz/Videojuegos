using UnityEngine; // Importa la librería principal de Unity para acceso a funciones del motor

// Clase que representa un objeto recolectable dentro del juego
public class ObjetoRecolectable : MonoBehaviour
{
    // Cantidad de veces que este objeto puede ser recolectado antes de cambiar de estado
    public int cantidad = 6;
    // Intensidad de la escala que se aplicará al objeto cuando sea recolectado (efecto de pulsación)
    public float escalaDePulsacion = 0.1f;
    // Duración total del pulso (efecto visual) en segundos
    public float duracionDePulso = 0.3f;
    // Modelo que se mostrará cuando la cantidad llegue a cero (estado final)
    public GameObject modeloSimple;
    // Modelo que se mostrará mientras el objeto aún tiene cantidad disponible
    public GameObject modeloComplejo;
    // Temporizador interno que controla la duración del efecto de pulso
    private float temporizadorDePulso;
    // Método llamado al inicio: establece el estado visual inicial
    private void Start()
    {
        modeloSimple.SetActive(false);  // Oculta el modelo simple
        modeloComplejo.SetActive(true); // Muestra el modelo complejo
    }

    // Método llamado una vez por frame: actualiza la animación de pulsación
    private void Update()
    {
        // Decrementa el temporizador según el tiempo transcurrido entre frames
        temporizadorDePulso -= Time.deltaTime;

        // Calcula la escala adicional basada en el tiempo restante de pulso
        float escalaExtra = escalaDePulsacion * Mathf.Max(temporizadorDePulso / duracionDePulso, 0);

        // Aplica la escala al objeto (efecto de "latido" o "pulso")
        transform.localScale = new Vector3(1 + escalaExtra, 1 + escalaExtra, 1 + escalaExtra);
    }

    // Método público que se llama al recolectar el objeto
    public void Recolectar()
    {
        cantidad--; // Reduce la cantidad disponible en 1
        temporizadorDePulso = duracionDePulso; // Reinicia el pulso visual

        // Si la cantidad llega a cero, cambia el modelo visual
        if (cantidad == 0)
        {
            modeloSimple.SetActive(true);  // Muestra el modelo simple
            modeloComplejo.SetActive(false); // Oculta el modelo complejo
        }
    }
}
