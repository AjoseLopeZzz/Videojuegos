using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DepositTrigger : MonoBehaviour
{
    [SerializeField] public DepositZoneWithCrates depositZone;
    [SerializeField] private string playerTag = "Player";

    private void Awake()
    {
        var col = GetComponent<Collider>();
        if (!col.isTrigger) col.isTrigger = true; // asegúrate de que sea trigger
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            depositZone?.ExecuteDeposit();
        }
    }
}
