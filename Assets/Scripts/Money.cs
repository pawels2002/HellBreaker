//using System.Diagnostics;
using TMPro;
using UnityEngine;

public class Money : MonoBehaviour
{
    public static Money Instance;

    public TextMeshProUGUI moneyText; // Drag the UI text object here
    public int money;

    private void Awake()
    {
        Instance = this;
        money = 100;
        Update();
    }


    void Update()
    {
 //       Debug.Log("Money: " + money); // Debug log to check the money value
        moneyText.text = "Money: " + money;
    }

    public int GetMoney()
    {
        return money;
    }

    public void AddMoney(int amount)
    {
        money += amount;
 //       Debug.Log("Added Money: " + amount); // Debug log to check the added amount
    }

    public void RemoveMoney(int amount)
    {
        if (money - amount < 0)
        {
            Debug.Log("Not enough money!"); // Debug log to check if there's not enough money
            return;
        }
        else {
            money -= amount;
            Debug.Log("Removed Money: " + amount); // Debug log to check the removed amount

        }

    }
}
