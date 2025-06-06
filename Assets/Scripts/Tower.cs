using System;
//using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public abstract class Tower : MonoBehaviour
{
    [Header("Animation")]
    public Animator animator;

    [Header("Tower Stats")]
    public float range;
    public float fireRate;
    public int cost;
    public int level;
    public int upgradeCost;

    [Header("Bullet Settings")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    

    [Header("Sprites")]
    public SpriteRenderer spriteRenderer;
    public Sprite frontView;
    public Sprite backView;
    public Sprite sideView;
    private Vector3 inputDirection;

    [Header("UI")]
    public GameObject upgradeButtonUI;
    public Transform upgradeTowerPoint;
    public float upgradeButtonDisplayRange = 3f;
    private bool upgradeUIActive = false;
    private Transform playerTransform;

    protected float fireCountdown = 0f;
    private Vector3 vec3; //delete

    protected virtual void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
        transform.rotation = Quaternion.Euler(45f, 0f, 0f);
        playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        upgradeButtonUI = Instantiate(upgradeButtonUI, vec3, Quaternion.identity);
        //   upgradeButtonUI.transform.position = upgradeTowerPoint.position;
        //   upgradeButtonUI.transform.rotation = Quaternion.Euler(45f, 0f, 0f);
        Button btn = upgradeButtonUI.GetComponentInChildren<Button>();
        if (btn != null)
        {
            Debug.Log("Setting the listener");
            btn.onClick.AddListener(OnButtonClick);
        }
        else
        {
            Debug.Log("Couldnt find button");
        }

        if (playerTransform == null)
        {
            //  Debug.LogError("Player transform not found! Make sure the player has the 'Player' tag.");
        }
        else
        {
            //    Debug.Log("Player transform found: " + playerTransform.name);
        }

        if (upgradeButtonUI != null)
        {
    //        Debug.Log("Upgrade button UI found: " + upgradeButtonUI.name);
            upgradeButtonUI.SetActive(false);
        }
        else
        {
        //    Debug.LogError("Upgrade button UI not assigned in the inspector!");
        }
    }

    protected virtual void Update()
    {
        if (!PauseMenu.isPaused) { 
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject nearest = null;
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float dist = Vector3.Distance(transform.position, enemy.transform.position);
            if (dist < shortestDistance && dist <= range)
            {
                shortestDistance = dist;
                nearest = enemy;
            }
        }

        if (nearest != null)
        {


            if (fireCountdown <= 0f)
            {
               
                FaceEnemy(nearest.transform);
                Shoot(nearest.transform);
                fireCountdown = 1f / fireRate;
            }
        }

        fireCountdown -= Time.deltaTime;

            if (playerTransform != null && upgradeButtonUI != null && upgradeTowerPoint != null && level != 3)
            {
                float distToPlayer = Vector3.Distance(transform.position, playerTransform.position);
                //Debug.Log("Distance to player: " + distToPlayer);
                if (distToPlayer <= upgradeButtonDisplayRange)
                {
                    if (!upgradeUIActive)
                    {
                        Button btn = upgradeButtonUI.GetComponentInChildren<Button>();
                        btn.GetComponentInChildren<TextMeshProUGUI>().text = "Upgrade: " + upgradeCost.ToString();
                        upgradeButtonUI.SetActive(true);
                       // Debug.Log("Upgrade UI activated for tower: " + gameObject.name);
                        upgradeUIActive = true;
                    }
                    // Move upgrade button UI to the upgradeTowerPoint position
                    //upgradeButtonUI.transform.position = upgradeTowerPoint.position;
                  //  Debug.Log("Upgrade button UI position set to: " + upgradeTowerPoint.position);
                    //upgradeButtonUI.transform.rotation = Quaternion.identity; // Optional: Keep UI upright
                }
                else
                {
                    if (upgradeUIActive)
                    {
                        upgradeButtonUI.SetActive(false);
                        upgradeUIActive = false;
                      //  Debug.Log("Upgrade UI deactivated for tower: " + gameObject.name);
                    }
                }
            }
        }
    }

    protected virtual void Shoot(Transform target)
    {
        GameObject bulletGO = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bullet = bulletGO.GetComponent<Bullet>();
        if (bullet != null)
        {
            bullet.Seek(target);
        }

        //Debug.Log("Bullet spawned at: " + firePoint.position);
    }

    protected void FaceEnemy(Transform target) //fix this - only rotates left and right
    {
        Vector3 direction = target.position - transform.position;
        float angle = Vector3.SignedAngle(Vector3.up, direction, Vector3.forward);
       // Debug.Log("Angle to enemy: " + angle);
        if (angle >= -45f && angle <= 45f)
        {
            spriteRenderer.sprite = frontView;
            spriteRenderer.flipX = false;
        }
        else if (angle >= 135f || angle <= -135f)
        {
            spriteRenderer.sprite = backView;
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.sprite = sideView;
            spriteRenderer.flipX = direction.x < 0f; // Flip based on direction
        }
    }

    public void upgradeTower()
    {
        switch (level)
        {
            case 0:  
                if(upgradeCost < Money.Instance.GetMoney())
                {
                    Money.Instance.RemoveMoney(upgradeCost);
                    improveTowerStatistics();
                    upgradeCost += 100;
                    //add star
                    level++;
                }
                else
                {
                    Debug.Log("Not enough money to upgrade the tower.");
                }
                break;
            case 1:
                if (upgradeCost < Money.Instance.GetMoney())
                {
                    Money.Instance.RemoveMoney(upgradeCost);
                    improveTowerStatistics();
                    upgradeCost += 200;
                    //add star
                    level++;
                }
                else
                {
                    Debug.Log("Not enough money to upgrade the tower.");
                }
                break;
            case 2:
                if (upgradeCost < Money.Instance.GetMoney())
                {
                    Money.Instance.RemoveMoney(upgradeCost);
                    improveTowerStatistics();
                    upgradeCost += 300;
                    //add star
                    upgradeButtonUI.SetActive(false);
                    upgradeUIActive = false;
                    level++;
                }
                else
                {
                    Debug.Log("Not enough money to upgrade the tower.");
                }
                break;
            default:
                Debug.Log("This tower is upgraded to maximum");
                break;
        }
    }

    public void improveTowerStatistics()
    {
        switch(level)
        {
            case 0:
                range += 0.25f;
                fireRate += 0.25f;
                break;
             case 1:
                range += 0.5f;
                fireRate += 0.5f;
                break;
             case 2:
                range += 1f;
                fireRate += 1f;
                break;
        }
    }

    public void OnButtonClick()
    {
        Debug.Log("Upgrade button clicked!");
        upgradeTower();
        Button btn = upgradeButtonUI.GetComponentInChildren<Button>();
        btn.GetComponentInChildren<TextMeshProUGUI>().text = "Upgrade: " + upgradeCost.ToString();
    }

  
}
