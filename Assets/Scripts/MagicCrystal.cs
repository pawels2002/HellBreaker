//using System.CodeDom;
//using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class MagicCrystal : Tower
{
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
        base.Shoot(target);
        
        
    }

    private IEnumerator DelayedShoot(GameObject[] enemies)
    {
        if (animator != null)
        {
            animator.SetBool("isFiring", true);
        }
        yield return new WaitForSeconds(0.35f); // Wait for 0.3 seconds before firing

        foreach (GameObject enemy in enemies)
        {
            // Unity null check: handles destroyed objects
            if (enemy == null) continue;

            float dist = Vector3.Distance(transform.position, enemy.transform.position);
            if (dist <= range)
            {
                Shoot(enemy.transform); // Shoot each enemy in range
            }
        }
        if (animator != null)
        {
            yield return new WaitForSeconds(0.40f); // Short delay for animation finish
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
                        //Debug.Log("Upgrade UI activated for tower: " + gameObject.name);
                        upgradeUIActive = true;
                    }
                    // Move upgrade button UI to the upgradeTowerPoint position
                    //upgradeButtonUI.transform.position = upgradeTowerPoint.position;
                    //Debug.Log("Upgrade button UI position set to: " + upgradeTowerPoint.position);
                    //upgradeButtonUI.transform.rotation = Quaternion.identity; // Optional: Keep UI upright
                }
                else
                {
                    if (upgradeUIActive)
                    {
                        upgradeButtonUI.SetActive(false);
                        upgradeUIActive = false;
                        //Debug.Log("Upgrade UI deactivated for tower: " + gameObject.name);
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
