using UnityEngine; // Importa la librería principal de Unity para usar sus componentes y funciones

// Clase que gestiona la interacción del jugador con los objetos recolectables mediante colisiones tipo "trigger"
public class JugadorCollider : MonoBehaviour
{
    // Tiempo mínimo entre cada recolección consecutiva (en segundos)
    public float duracionDeRecolectar = 0.5f;

    // Contador interno que limita la frecuencia de recolección
    private float contadorDeRecoleccion;

    // Contador de objetos recolectados por el jugador
    private int objetosRecolectados;

    // Método llamado cada frame: decrementa el temporizador de recolección
    private void Update()
    {
        contadorDeRecoleccion -= Time.deltaTime; // Reduce el contador en función del tiempo transcurrido
    }

    // Método que se ejecuta mientras el jugador permanece dentro de un trigger
    private void OnTriggerStay(Collider other)
    {
        // Verifica si el objeto con el que colisiona tiene un componente ObjetoRecolectable
        if (other.gameObject.GetComponent<ObjetoRecolectable>() != null)
        {
            // Obtiene la referencia al componente ObjetoRecolectable
            ObjetoRecolectable objeto = other.gameObject.GetComponent<ObjetoRecolectable>();

            // Solo recolecta si el temporizador ha expirado y el objeto aún tiene cantidad disponible
            if (contadorDeRecoleccion <= 0 && objeto.cantidad > 0)
            {
                // Reinicia el temporizador para evitar recolecciones instantáneas consecutivas
                contadorDeRecoleccion = duracionDeRecolectar;

                // Incrementa el contador de objetos recolectados
                objetosRecolectados++;

                // Llama al método Recolectar del objeto para reducir su cantidad y actualizar su estado
                objeto.Recolectar();
            }
        }
    }
}
