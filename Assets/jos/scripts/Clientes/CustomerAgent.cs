using System;
using UnityEngine;

public class CustomerAgent : MonoBehaviour
{
    [Header("Animator (opcional)")]
    [SerializeField] private Animator animator;
    [SerializeField] private string walkBoolName = "IsWalking";

    private Vector3 target;
    private float speed = 1.5f;
    private float arriveDist = 0.1f;
    private Quaternion fixedRotation;  // 🔹 rotación fija igual para todos

    public event Action OnArrived;

    public void Configure(Vector3 start, Vector3 firstTarget, float moveSpeed, float threshold)
    {
        if (animator == null) animator = GetComponentInChildren<Animator>();

        speed = Mathf.Max(0.05f, moveSpeed);
        arriveDist = Mathf.Max(0.01f, threshold);

        SetTarget(firstTarget);

        // 🔹 Guardamos la rotación del primer target (dirección hacia adelante)
        Vector3 lookDir = (firstTarget - start);
        lookDir.y = 0f;
        if (lookDir.sqrMagnitude > 0.001f)
            fixedRotation = Quaternion.LookRotation(lookDir.normalized, Vector3.up);
        else
            fixedRotation = Quaternion.identity;

        // 🔒 Nacer a la misma altura del target
        transform.position = new Vector3(start.x, firstTarget.y, start.z);
        transform.rotation = fixedRotation; // todos con la misma orientación base
        SetWalking(true);
    }

    public void SetTarget(Vector3 newTarget)
    {
        target = newTarget;
        // mantener la misma rotación general, no cambiar cada frame
        SetWalking(true);
    }

    private void Update()
    {
        // Movimiento sólo en XZ (altura fijada al target)
        Vector3 to = target - transform.position;
        to.y = 0f;
        float dist = to.magnitude;

        if (dist > arriveDist)
        {
            Vector3 dir = to / Mathf.Max(dist, 0.0001f);

            Vector3 newPos = transform.position + dir * speed * Time.deltaTime;
            newPos.y = target.y; // 🔒 altura constante
            transform.position = newPos;

            // 🔹 Mantener siempre la misma rotación (no girar hacia el movimiento)
            transform.rotation = fixedRotation;

            SetWalking(true);
        }
        else
        {
            // Asegura altura exacta al final
            var p = transform.position;
            p.y = target.y;
            transform.position = p;

            transform.rotation = fixedRotation; // 🔒 fija al final también

            SetWalking(false);
            OnArrived?.Invoke();
        }
    }

    private void SetWalking(bool w)
    {
        if (animator != null && !string.IsNullOrEmpty(walkBoolName))
            animator.SetBool(walkBoolName, w);
    }
}
