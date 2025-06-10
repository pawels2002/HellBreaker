
using TMPro;
using UnityEngine;

public class HealthPlayer : MonoBehaviour
{
    public static HealthPlayer Instance;

    public TextMeshProUGUI healthText; // Drag the UI text object here
    public int health;

    private void Awake()
    {
        Instance = this;
        health = 100;
        Update();
    }


    void Update()
    {
        //       Debug.Log("Health: " + Health); // Debug log to check the Health value
        healthText.text = health.ToString();
    }

    public int GetHealth()
    {
        return health;
    }

    public void AddHealth(int amount)
    {
        health += amount;
        //       Debug.Log("Added Health: " + amount); // Debug log to check the added amount
    }

    public void RemoveHealth(int amount)
    {
        if (health - amount <= 0)
        {
            health = 0;
            healthText.text = "D E A D"; 
            return;
        }
        else
        {
            health -= amount;
            Debug.Log("Player's current health: " + amount); // Debug log to check the removed amount

        }

    }
}
