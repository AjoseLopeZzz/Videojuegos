using TMPro;
using UnityEngine;

[DisallowMultipleComponent]
public class MoneyDisplay : MonoBehaviour
{
    [SerializeField] private Wallet wallet;
    [SerializeField] private TextMeshProUGUI txtMoney;

    private void OnEnable()
    {
        if (wallet != null)
        {
            wallet.OnMoneyChanged += Refresh;
            Refresh(wallet.Money);
        }
        else
        {
            Debug.LogWarning("[MoneyDisplay] Wallet no asignado.");
        }
    }

    private void OnDisable()
    {
        if (wallet != null) wallet.OnMoneyChanged -= Refresh;
    }

    private void Refresh(int value)
    {
        if (txtMoney) txtMoney.text = value.ToString();
    }
}
