using UnityEngine;
using System.Diagnostics;
using System.CodeDom;
using System.Collections;

public class HoundiusShootius : Tower
{
    [SerializeField] public int damage = 80;
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