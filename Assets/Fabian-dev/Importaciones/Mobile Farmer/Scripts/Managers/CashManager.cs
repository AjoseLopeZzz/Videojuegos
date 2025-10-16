 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CashManager : MonoBehaviour
{
    public static CashManager instance;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        LoadData();
        UpdateCoinContainers();
    }

    [Header("Configuraciones")]
    private int coins;
    

public void UpdateCoinContainers()
    {
        GameObject[] coinContainers = GameObject.FindGameObjectsWithTag("CoinAmount");

        foreach (GameObject coinContainer in coinContainers)
            coinContainer.GetComponent<TextMeshProUGUI>().text = coins.ToString();
    }
    public void AddCoins(int amount)
    {
        coins += amount;
        UpdateCoinContainers();
        SaveData();
    }
    private void LoadData()
    {
        coins = PlayerPrefs.GetInt("Coins");
    }
    private void SaveData()
    {
        PlayerPrefs.SetInt("Coins", coins);
    }
}
