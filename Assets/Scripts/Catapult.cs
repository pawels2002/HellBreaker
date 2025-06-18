using UnityEngine;
using System.Diagnostics;
using System.CodeDom;
using System.Collections;

public class Catapult : Tower
{
    protected override void Awake()
    {
        range = 10f; 
        fireRate = 0.5f; 
        cost = 150;
        level = 0;
        upgradeCost = 150;
        base.Awake();
    }

    // You can override Shoot() or Update() to customize behavior
    protected override void Shoot(Transform target)
    {
        int animDirection = FaceEnemy(target); // Get direction from base method

        if (animator != null)
        {
            animator.SetBool("isFiring", true);
            animator.SetInteger("direction", animDirection);
        }
        StartCoroutine(DelayedShoot(target));
    }

    private IEnumerator DelayedShoot(Transform target)
    {
        yield return new WaitForSeconds(0.13f); // Wait for 0.3 seconds before firing

        base.Shoot(target);

        // Optionally, reset the animation after firing
        if (animator != null)
        {
            yield return new WaitForSeconds(0.37f); // Short delay for animation finish
            animator.SetBool("isFiring", false);
        }

    }
}