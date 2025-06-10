using UnityEngine;
using System.Diagnostics;
using System.CodeDom;
using System.Collections;

public class HoundiusShootius : Tower
{
    protected override void Awake()
    {
        range = 4f; 
        fireRate = 2f; 
        cost = 300;
        level = 0;
        upgradeCost = 200;
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
        yield return new WaitForSeconds(0.41f); // Wait for 0.3 seconds before firing

        base.Shoot(target);

        // Optionally, reset the animation after firing
        if (animator != null)
        {
            yield return new WaitForSeconds(0.35f); // Short delay for animation finish
            animator.SetBool("isFiring", false);
        }



    }
}