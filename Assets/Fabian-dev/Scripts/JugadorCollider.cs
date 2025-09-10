using UnityEngine; // Importa la librer�a principal de Unity para usar sus componentes y funciones

// Clase que gestiona la interacci�n del jugador con los objetos recolectables mediante colisiones tipo "trigger"
public class JugadorCollider : MonoBehaviour
{
    // Tiempo m�nimo entre cada recolecci�n consecutiva (en segundos)
    public float duracionDeRecolectar = 0.5f;

    // Contador interno que limita la frecuencia de recolecci�n
    private float contadorDeRecoleccion;

    // Contador de objetos recolectados por el jugador
    private int objetosRecolectados;

    // M�todo llamado cada frame: decrementa el temporizador de recolecci�n
    private void Update()
    {
        contadorDeRecoleccion -= Time.deltaTime; // Reduce el contador en funci�n del tiempo transcurrido
    }

    // M�todo que se ejecuta mientras el jugador permanece dentro de un trigger
    private void OnTriggerStay(Collider other)
    {
        // Verifica si el objeto con el que colisiona tiene un componente ObjetoRecolectable
        if (other.gameObject.GetComponent<ObjetoRecolectable>() != null)
        {
            // Obtiene la referencia al componente ObjetoRecolectable
            ObjetoRecolectable objeto = other.gameObject.GetComponent<ObjetoRecolectable>();

            // Solo recolecta si el temporizador ha expirado y el objeto a�n tiene cantidad disponible
            if (contadorDeRecoleccion <= 0 && objeto.cantidad > 0)
            {
                // Reinicia el temporizador para evitar recolecciones instant�neas consecutivas
                contadorDeRecoleccion = duracionDeRecolectar;

                // Incrementa el contador de objetos recolectados
                objetosRecolectados++;

                // Llama al m�todo Recolectar del objeto para reducir su cantidad y actualizar su estado
                objeto.Recolectar();
            }
        }
    }
}
