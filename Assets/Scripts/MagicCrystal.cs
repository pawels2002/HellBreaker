//using System.CodeDom;
//using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class MagicCrystal : Tower
{
    [SerializeField] public int damage = 80;
    protected override void Awake()
    { 
        range = 3f; 
        fireRate = 0.25f; 
        cost = 600;
        level = 0;
        upgradeCost = 600;
        base.Awake();
    }

    // You can override Shoot() or Update() to customize behavior
    protected override void Shoot(Transform target)
    {
        GameObject bulletGO = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bullet = bulletGO.GetComponent<Bullet>();
        if (bullet != null)
        {
            bullet.SetDamage(damage); // Pass crystal's damage to bullet
            bullet.Seek(target);
        }
    }

    private IEnumerator DelayedShoot(GameObject[] enemies)
    {
        if (animator != null)
        {
            animator.SetBool("isFiring", true);
        }
        yield return new WaitForSeconds(0.35f);

        foreach (GameObject enemy in enemies)
        {
            if (enemy == null) continue;

            float dist = Vector3.Distance(transform.position, enemy.transform.position);
            if (dist <= range)
            {
                Shoot(enemy.transform);
            }
        }

        if (animator != null)
        {
            yield return new WaitForSeconds(0.40f);
            animator.SetBool("isFiring", false);
        }
    }

    protected override void Update()
    {
        if (!PauseMenu.isPaused)
        {
            // First we check the tower's cooldown timer
            if (fireCountdown > 0f)
            {
                fireCountdown -= Time.deltaTime;
            }

            //Only when the tower is ready, we check for enemies in range
            //I made it this way because otherwise the tower would play the fire animation without any enemies
            //and sometimes with the previous logic it would not fire at all
            if (fireCountdown <= 0f)
            {
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                bool enemyInRange = false;

                foreach (GameObject enemy in enemies)
                {
                    float dist = Vector3.Distance(transform.position, enemy.transform.position);
                    if (dist <= range)
                    {
                        enemyInRange = true;
                        break;
                    }
                }

                if (enemyInRange)
                {
                    StartCoroutine(DelayedShoot(enemies));
                    fireCountdown = 1f / fireRate; // Reset cooldown after firing
                }
            }
            if (playerTransform != null && upgradeButtonUI != null && upgradeTowerPoint != null)
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

    protected override void improveTowerStatistics()
    {
        switch (level)
        {
            case 0:
                //range += 0.25f;
                fireRate += 0.25f;
                upgradeCost += 200;
                star1.enabled = true;
                break;
            case 1:
                //range += 0.25f;
                fireRate += 0.25f;
                upgradeCost += 400;
                star1.enabled = false;
                star2.enabled = true;
                break;
            case 2:
                //range += 1f;
                fireRate += 0.25f;
                star2.enabled = false;
                star3.enabled = true;
                break;
        }
    }
}
