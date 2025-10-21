using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [Header("Prefabs de clientes")]
    [SerializeField] private GameObject[] customerPrefabs;

    [Header("Puntos")]
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;

    [Header("Spawning")]
    [SerializeField] private int maxActiveCustomers = 5;
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private float initialDelay = 0.5f;

    [Header("Movimiento")]
    [SerializeField] private float moveSpeed = 1.6f;
    [SerializeField] private float arriveThreshold = 0.12f;

    [Header("Cola")]
    [SerializeField] private float queueSpacing = 0.9f; // distancia entre clientes

    private readonly List<CustomerAgent> active = new();

    private void OnEnable() { StartCoroutine(SpawnLoop()); }
    private void OnDisable() { StopAllCoroutines(); }

    private IEnumerator SpawnLoop()
    {
        yield return new WaitForSeconds(initialDelay);

        while (enabled)
        {
            // Limpieza de nulos
            for (int i = active.Count - 1; i >= 0; i--)
                if (active[i] == null) active.RemoveAt(i);

            // Intentar spawnear si hay cupo
            if (active.Count < maxActiveCustomers && customerPrefabs?.Length > 0)
                SpawnOne();

            // Actualizar destinos de la cola (fila)
            UpdateQueueTargets();

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void Update()
    {
        // Ajuste suave cada frame
        UpdateQueueTargets();
    }

    private void UpdateQueueTargets()
    {
        if (endPoint == null || startPoint == null) return;

        // Dirección desde el punto final hacia el inicio (para formar hacia atrás)
        Vector3 dir = (startPoint.position - endPoint.position);
        dir.y = 0f;
        dir = dir.sqrMagnitude < 1e-4f ? Vector3.back : dir.normalized;

        for (int i = 0; i < active.Count; i++)
        {
            if (active[i] == null) continue;

            Vector3 slot = endPoint.position + dir * (queueSpacing * i);
            active[i].SetTarget(slot);
        }
    }

    private void SpawnOne()
    {
        if (startPoint == null || endPoint == null) return;

        GameObject prefab = customerPrefabs[Random.Range(0, customerPrefabs.Length)];
        if (prefab == null) return;

        // --------- SOLO SOBRE LA LÍNEA start → end ----------
        Vector3 a = startPoint.position;
        Vector3 b = endPoint.position;

        // punto aleatorio en el segmento [a, b]
        float t = Random.Range(0f, 1f);
        Vector3 spawnPos = Vector3.Lerp(a, b, t);

        // fijar la altura exactamente a la del endPoint (línea nivelada)
        spawnPos.y = endPoint.position.y;

        // rotar mirando hacia el endPoint
        Vector3 lookDir = (b - a); lookDir.y = 0f;
        Quaternion spawnRot = lookDir.sqrMagnitude > 1e-4f
            ? Quaternion.LookRotation(lookDir.normalized, Vector3.up)
            : Quaternion.identity;
        // -----------------------------------------------------

        var go = Instantiate(prefab, spawnPos, spawnRot);
        var agent = go.GetComponent<CustomerAgent>();
        if (agent == null) agent = go.AddComponent<CustomerAgent>();

        // Primer target provisional; se recalcula en UpdateQueueTargets
        Vector3 firstTarget = endPoint.position;
        agent.Configure(spawnPos, firstTarget, moveSpeed, arriveThreshold);

        agent.OnArrived += () => { /* se queda idle */ };

        active.Add(agent);
        UpdateQueueTargets(); // recalcula la fila con el nuevo
    }
}
