using System.CodeDom;
using System.Diagnostics;
using UnityEngine;
using System.Collections;

public class Cannon : Tower
{

    protected override void Awake()
    {
        range = 3f; //this doesnt work - had to set it in unity -> inspector
        fireRate = 0.75f; //this doesnt work - had to set it in unity -> inspector
        cost = 50;  //this doesnt work - had to set it in unity -> inspector
        level = 0;
        upgradeCost = 50; 
        base.Awake();
    }

    protected override void Shoot(Transform target)
    {
        if (animator != null)
        {
            animator.SetBool("isFiring", true);
            animator.SetInteger("direction", 2);
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
            yield return new WaitForSeconds(0.53f); // Short delay for animation finish
            animator.SetBool("isFiring", false);
            animator.SetInteger("direction", 0);
        }

    }



}
