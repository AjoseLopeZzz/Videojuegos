using System;
using UnityEngine;

[DisallowMultipleComponent]
public class Wallet : MonoBehaviour
{
    [Header("Dinero (valor por defecto si no hay archivo)")]
    [SerializeField] private int defaultMoney = 0;

    [Header("Archivo")]
    [SerializeField] private string coinsFileName = "coinsData.txt";

    private CoinsData coins = new CoinsData();

    public int Money => coins.money;
    public event Action<int> OnMoneyChanged;

    private void Awake()
    {
        // Cargar de archivo o inicializar con default
        if (DataFiles.TryLoadJson<CoinsData>(coinsFileName, out var loaded))
        {
            coins = loaded;
        }
        else
        {
            coins.money = Mathf.Max(0, defaultMoney);
            Persist();
        }

        // Sincroniza UI al iniciar
        OnMoneyChanged?.Invoke(coins.money);
    }

    public void Add(int amount)
    {
        if (amount <= 0) return;
        coins.money += amount;
        Persist();
        OnMoneyChanged?.Invoke(coins.money);
    }

    public void Set(int value)
    {
        coins.money = Mathf.Max(0, value);
        Persist();
        OnMoneyChanged?.Invoke(coins.money);
    }

    public bool Spend(int amount)
    {
        if (amount <= 0) return false;
        if (coins.money < amount) return false;

        coins.money -= amount;
        Persist();
        OnMoneyChanged?.Invoke(coins.money);
        return true;
    }

    private void Persist()
    {
        DataFiles.SaveJson(coinsFileName, coins);
    }

    void Start()
    {
        Debug.Log("Ruta de datos persistentes: " + Application.persistentDataPath);
    }

}
