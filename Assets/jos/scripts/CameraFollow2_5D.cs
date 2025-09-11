using UnityEngine;

public class CameraFollow2_5D : MonoBehaviour
{
    public Transform objetivo;              // Player (root con Rigidbody)
    public Vector3 offsetMundo = new Vector3(0f, 6f, -10f); // offset fijo en mundo
    public float suavePos = 0.12f;          // 0.08–0.2 va bien
    public float suaveRot = 8f;

    Vector3 velRef; // para SmoothDamp

    void LateUpdate()
    {
        if (!objetivo) return;

        // Posición
        Vector3 destino = objetivo.position + offsetMundo;
        transform.position = Vector3.SmoothDamp(transform.position, destino, ref velRef, suavePos);

        // Rotación (opcional)
        Quaternion look = Quaternion.LookRotation(objetivo.position - transform.position, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, look, 1f - Mathf.Exp(-suaveRot * Time.deltaTime));
    }
}
