using System;
//using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public abstract class Tower : MonoBehaviour
{
    // Static reference to the tower currently showing its range
    public static Tower activeRangeTower = null;

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
    public Transform firePointFront;
    public Transform firePointBack;
    public Transform firePointRight;
    public Transform firePointLeft;

    [Header("Sprites")]
    public SpriteRenderer star1;
    public SpriteRenderer star2;
    public SpriteRenderer star3;
    public SpriteRenderer spriteRenderer;
    public Sprite frontView;
    public Sprite backView;
    public Sprite sideView;
    protected Vector3 inputDirection;

    [Header("UI")]
    public GameObject upgradeButtonUI;
    //public GameObject sellTowerButtonUI;
    public Transform upgradeTowerPoint;  //since the button is in UI, this can be deleted - leaving this just in case
    public float upgradeButtonDisplayRange = 3f;
    protected bool upgradeUIActive = false;
    protected Transform playerTransform;
    private float buildDelay = 0.4f;
    private float buildTimer = 0f;
    public bool isBuilt = false;
    [SerializeField] public GameObject rangeVisualizer;

    protected float fireCountdown = 0f;
    protected Vector3 vec3; //delete

    protected virtual void Awake()
    {
        buildTimer = buildDelay;
        isBuilt = false;
        star1.enabled = false;
        star2.enabled = false;
        star3.enabled = false;
        rangeVisualizer.transform.localScale = Vector3.one * range * 1f;
        rangeVisualizer.SetActive(false); // Hide by default
        if (animator == null)
            animator = GetComponent<Animator>();
        transform.rotation = Quaternion.Euler(45f, 0f, 0f);
        playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        upgradeButtonUI = Instantiate(upgradeButtonUI, vec3, Quaternion.identity);
        // sellTowerButtonUI = Instantiate(sellTowerButtonUI, vec3, Quaternion.identity);
        //upgradeButtonUI.transform.position = upgradeTowerPoint.position;
        //upgradeButtonUI.transform.rotation = Quaternion.Euler(45f, 0f, 0f);
        Button[] allButtons = upgradeButtonUI.GetComponentsInChildren<Button>();

        // Get specific button by GameObject name
        Button btn = allButtons.FirstOrDefault(b => b.gameObject.name == "UpgradeButton");
        Button sellBtn = allButtons.FirstOrDefault(b => b.gameObject.name == "SellButton");
        sellBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Sell Tower: " + cost.ToString(); // Set sell button text
        if (btn != null || sellBtn != null)
        {
            Debug.Log("Setting the listener");
            btn.onClick.AddListener(OnButtonClick);
            sellBtn.onClick.AddListener(SellTower);
        }
        else
        {
            Debug.Log("Couldnt find button");
        }

        if (playerTransform == null)
        {
            //Debug.LogError("Player transform not found! Make sure the player has the 'Player' tag.");
        }
        else
        {
            //Debug.Log("Player transform found: " + playerTransform.name);
        }

        if (upgradeButtonUI != null)
        {
            //Debug.Log("Upgrade button UI found: " + upgradeButtonUI.name);
            ShowRange(false); // Hide range visualizer initially
            upgradeButtonUI.SetActive(false);
            //sellTowerButtonUI.SetActive(false); // Hide sell button initially
        }
        else
        {
            //Debug.LogError("Upgrade button UI not assigned in the inspector!");
        }
    }

    protected virtual void Update()
    {
        rangeVisualizer.transform.localScale = Vector3.one * range * 1f;
        if (!isBuilt)
        {
            buildTimer -= Time.deltaTime;
            if (buildTimer <= 0f)
            {
                isBuilt = true;
            }
            return; // Skip the rest of Update until built
        }
        if (!PauseMenu.isPaused)
        {
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
                    Shoot(nearest.transform);
                    fireCountdown = 1f / fireRate;
                }
            }

            fireCountdown -= Time.deltaTime;

            if (playerTransform != null && upgradeButtonUI != null && upgradeTowerPoint != null )
            {
                float distToPlayer = Vector3.Distance(transform.position, playerTransform.position);

                if (distToPlayer <= upgradeButtonDisplayRange)
                {
                    if (!upgradeUIActive)
                    {
                        // Hide previous tower's range/UI if any
                        if (activeRangeTower != null && activeRangeTower != this)
                        {
                            activeRangeTower.HideUpgradeUI();
                        }

                        Button btn = upgradeButtonUI.GetComponentInChildren<Button>();
                        if (level == 3)
                        {
                            btn.GetComponentInChildren<TextMeshProUGUI>().text = "Max level!";
                        }
                        else
                        {
                            btn.GetComponentInChildren<TextMeshProUGUI>().text = "Upgrade: " + upgradeCost.ToString();
                        }
                        upgradeButtonUI.SetActive(true);
                  //      sellTowerButtonUI.SetActive(true); // Show sell button
                        upgradeUIActive = true;

                        ShowRange(true);

                        // Set this tower as the active one showing range
                        activeRangeTower = this;
                    }
                }
                else
                {
                    if (upgradeUIActive)
                    {
                        HideUpgradeUI();

                        // Clear active tower if this was the active one
                        if (activeRangeTower == this)
                        {
                            activeRangeTower = null;
                        }
                    }
                }
            }
        }
    }

    // Helper method to hide UI and range
    public void HideUpgradeUI()
    {
        upgradeButtonUI.SetActive(false);
       // sellTowerButtonUI.SetActive(false); // Hide sell button
        ShowRange(false);
        upgradeUIActive = false;
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

    protected int FaceEnemy(Transform target)
    {
        Vector3 direction = target.position - transform.position;
        float angle = Vector3.SignedAngle(Vector3.forward, direction, Vector3.up);

        if (angle >= -45 && angle <= 45)
        {
            spriteRenderer.sprite = backView;
            firePoint = firePointBack;
            spriteRenderer.flipX = false;
            return 1; // up
        }
        else if (angle >= 145 || angle <= -145)
        {
            spriteRenderer.sprite = frontView;
            firePoint = firePointFront;
            spriteRenderer.flipX = false;
            return 2; // down
        }
        else
        {
            spriteRenderer.sprite = sideView;
            firePoint = (angle > 0) ? firePointRight : firePointLeft;
            spriteRenderer.flipX = direction.x < 0f;
            return 3; // side
        }
        // Optionally, return 0 for idle if needed
    }

    public void upgradeTower()
    {
        switch (level)
        {
            case 0:
            case 1:
            case 2:
                if (upgradeCost < Money.Instance.GetMoney())
                {
                    Money.Instance.RemoveMoney(upgradeCost);
                    improveTowerStatistics();
                    //add star here
                    level++;
                    if (level == 3)
                    {
                        Button btn = upgradeButtonUI.GetComponentInChildren<Button>();
                        btn.GetComponentInChildren<TextMeshProUGUI>().text = "Max level!";
                    }
                }
                else
                {
                    Debug.Log("Not enough money to upgrade the tower.");
                }
                break;
            default:
                Debug.Log("This tower is upgraded to maximum"); //this should never be triggered
                break;
        }
    }

    protected virtual void improveTowerStatistics()
    {
        switch (level)
        {
            case 0:
                range += 0.25f;
                fireRate += 0.25f;
                upgradeCost += 100;
                star1.enabled = true;
                break;
            case 1:
                range += 0.5f;
                fireRate += 0.5f;
                upgradeCost += 200;
                star1.enabled = false;
                star2.enabled = true;
                break;
            case 2:
                range += 1f;
                fireRate += 1f;
                star2.enabled = false;
                star3.enabled = true;
                break;
        }
    }

    public void OnButtonClick()
    {
        Debug.Log("Upgrade button clicked!");
        upgradeTower();
        if (level == 3)
        {
            Button btn = upgradeButtonUI.GetComponentInChildren<Button>();
            btn.GetComponentInChildren<TextMeshProUGUI>().text = "Max level!";
        }
        else
        {
            Button btn = upgradeButtonUI.GetComponentInChildren<Button>();
            btn.GetComponentInChildren<TextMeshProUGUI>().text = "Upgrade: " + upgradeCost.ToString();
        }
        
    }

    public void ShowRange(bool show)
    {
        rangeVisualizer.SetActive(show);
    }

    public void SellTower()
    {
        Money.Instance.AddMoney(cost); // TODO: think about how much money to give back
        Destroy(gameObject);
        HideUpgradeUI();
        ShowRange(false);
        if (activeRangeTower == this)
        {
            activeRangeTower = null; // Clear active tower if this was the one
        }
    }
}
