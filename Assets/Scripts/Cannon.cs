using System.CodeDom;
using System.Diagnostics;
using UnityEngine;
using System.Collections;

public class Cannon : Tower
{
    [SerializeField] public int damage = 80;

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
        yield return new WaitForSeconds(0.3f);

        GameObject bulletGO = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bullet = bulletGO.GetComponent<Bullet>();
        if (bullet != null)
        {
            bullet.SetDamage(damage);
            bullet.Seek(target);
        }

        if (animator != null)
        {
            yield return new WaitForSeconds(0.47f);
            animator.SetBool("isFiring", false);
        }
    }
}
