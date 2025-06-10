using System.CodeDom;
using System.Diagnostics;
using UnityEngine;
using System.Collections;

public class Cannon : Tower
{
    protected override void Awake()
    {
        range = 3f;
        fireRate = 0.75f; 
        cost = 50;  
        level = 0;
        upgradeCost = 50; 
        base.Awake();
    }

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
        yield return new WaitForSeconds(0.3f); // Wait for 0.3 seconds before firing

        base.Shoot(target);

        // Optionally, reset the animation after firing
        if (animator != null)
        {
            yield return new WaitForSeconds(0.47f); // Short delay for animation finish
            animator.SetBool("isFiring", false);
        }

    }



}
