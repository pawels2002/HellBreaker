using UnityEngine;
using System.Collections;

public class Ballista : Tower
{
    public GameObject arrowFrontPrefab;
    public GameObject arrowBackPrefab;
    public GameObject arrowSidePrefab;

    private GameObject currentBulletPrefab;
    private bool flipBulletX = false;

    protected override void Awake()
    {
        range = 4f;
        fireRate = 1.5f;
        cost = 100;
        level = 0;
        upgradeCost = 100;
        base.Awake();
    }

    protected override void Shoot(Transform target)
    {
        Vector3 direction = target.position - transform.position;
        int animDirection = FaceEnemy(target); // sets sprite + flip

        if (animator != null)
        {
            animator.SetBool("isFiring", true);
            animator.SetInteger("direction", animDirection);
        }

        // Choose correct bullet prefab and whether to flip it
        switch (animDirection)
        {
            case 1: // back
                currentBulletPrefab = arrowBackPrefab;
                flipBulletX = false;
                break;
            case 2: // front
                currentBulletPrefab = arrowFrontPrefab;
                flipBulletX = false;
                break;
            case 3: // side
                currentBulletPrefab = arrowSidePrefab;
                flipBulletX = direction.x < 0;
                break;
            default:
                currentBulletPrefab = arrowFrontPrefab;
                flipBulletX = false;
                break;
        }

        StartCoroutine(DelayedShoot(target));
    }

    private IEnumerator DelayedShoot(Transform target)
    {
        yield return new WaitForSeconds(0.13f); // sync with animation

        GameObject bulletGO = Instantiate(currentBulletPrefab, firePoint.position, firePoint.rotation);
        SpriteRenderer bulletRenderer = bulletGO.GetComponent<SpriteRenderer>();
        if (bulletRenderer != null)
        {
            bulletRenderer.flipX = flipBulletX;
        }

        Bullet bullet = bulletGO.GetComponent<Bullet>();
        if (bullet != null)
        {
            bullet.Seek(target);
        }

        if (animator != null)
        {
            yield return new WaitForSeconds(0.37f); // finish animation
            animator.SetBool("isFiring", false);
        }
    }
}
