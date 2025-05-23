
using System.Collections.Specialized;

using UnityEngine;

public class Build : MonoBehaviour
{
    public GameObject selectedTowerPrefab;



    public void SetTowerToBuild(GameObject towerPrefab)
    {
        selectedTowerPrefab = towerPrefab;
        Debug.Log("Selected tower: " + towerPrefab.name);
    }


    public void BuildTower(Vector3 spawnPos)
    {
        if (selectedTowerPrefab == null)
        {
            Debug.Log("No tower selected to build!"); //change to UI
            return;
        }


        int cost = selectedTowerPrefab.GetComponent<Tower>().cost;

        if (Money.Instance.GetMoney() < cost)
        {
            Debug.Log("Not enough money to build this tower!");  //change to UI
            return; 
        }


        Money.Instance.RemoveMoney(cost);
        Instantiate(selectedTowerPrefab, spawnPos, Quaternion.identity);
        Debug.Log("Tower spawned!");
    }

}
