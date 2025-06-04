
using System.Collections.Specialized;
using UnityEngine.UI;
using UnityEngine;
//using static System.Net.Mime.MediaTypeNames;

public class Build : MonoBehaviour
{
    public GameObject selectedTowerPrefab;

    public void SetTowerToBuild(GameObject towerPrefab)
    {
        selectedTowerPrefab = towerPrefab;
        GameObject selectedTowerImage = GameObject.Find("CurrentlySelectedTurretImage");
        selectedTowerImage.SetActive(true);
        Sprite towerSprite = towerPrefab.GetComponent<Tower>().frontView;
        selectedTowerImage.GetComponent<Image>().sprite = towerSprite;
        GameObject radialBuildMenu = GameObject.Find("RadialBuyMenuPanel");
        radialBuildMenu.SetActive(false);
        Time.timeScale = 1.0f;
    }


    public void BuildTower(Vector3 spawnPos)
    {
        if (selectedTowerPrefab == null)
        {
            Debug.Log("No tower selected to build!"); //change to UI
            return;
        }

        float checkRadius = 0.5f; // Adjust based on tower size
        Collider[] colliders = Physics.OverlapSphere(spawnPos, checkRadius);
        foreach (Collider col in colliders)
        {
            if (col.GetComponent<Tower>() != null)
            {
                Debug.Log("There is already a tower at this location!"); // change to UI
                return;
            }
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
