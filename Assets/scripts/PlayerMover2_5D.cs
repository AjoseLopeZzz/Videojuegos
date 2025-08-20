using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMover2_5D : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 4f;
    public bool movimientoRelativoACamara = true; // En AR: true

    [Header("Suavizado")]
    public float desaceleracion = 10f;

    Rigidbody rb;
    Transform cam;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation; // Evita vuelcos
        cam = Camera.main != null ? Camera.main.transform : null;
    }

    void FixedUpdate()
    {
        // Entradas (WASD / Flechas / Joystick)
        float h = Input.GetAxisRaw("Horizontal"); // izq/der
        float v = Input.GetAxisRaw("Vertical");   // arriba/abajo

        Vector3 dir = new Vector3(h, 0f, v);

        if (dir.sqrMagnitude > 1f) dir.Normalize();

        // ¿Movimiento relativo a cámara?
        if (movimientoRelativoACamara && cam != null)
        {
            Vector3 camForward = cam.forward; camForward.y = 0f; camForward.Normalize();
            Vector3 camRight = cam.right; camRight.y = 0f; camRight.Normalize();
            dir = camForward * v + camRight * h;
        }

        // Velocidad objetivo en plano
        Vector3 velObjetivo = dir * velocidad;
        // Conserva la Y del rigidbody
        Vector3 velActual = rb.velocity;
        Vector3 velPlano = new Vector3(velActual.x, 0f, velActual.z);

        // Suaviza cambio de velocidad
        Vector3 velPlanoNueva = Vector3.Lerp(velPlano, velObjetivo, 1f - Mathf.Exp(-desaceleracion * Time.fixedDeltaTime));
        rb.velocity = new Vector3(velPlanoNueva.x, velActual.y, velPlanoNueva.z);

        // Opcional: orientar sprite según avance (solo si tu sprite rota en Y)
        if (dir.sqrMagnitude > 0.0001f)
        {
            Quaternion look = Quaternion.LookRotation(dir, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, look, 0.2f);
        }
    }
}
