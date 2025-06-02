//using System.Diagnostics;
using UnityEngine;


public abstract class Tower : MonoBehaviour
{
    [Header("Tower Stats")]
    public float range;
    public float fireRate;
    public int cost;

    [Header("Bullet Settings")]
    public GameObject bulletPrefab;
    public Transform firePoint;

    [Header("Sprites")]
    public SpriteRenderer spriteRenderer;
    public Sprite frontView;
    public Sprite backView;
    public Sprite sideView;
    private Vector3 inputDirection;

    protected float fireCountdown = 0f;
    protected virtual void Awake()
    {
        transform.rotation = Quaternion.Euler(45f, 0f, 0f);
        
    }

    protected virtual void Update()
    {
        if (!PauseMenu.isPaused) { 
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
                FaceEnemy(nearest.transform);
                Shoot(nearest.transform);
                fireCountdown = 1f / fireRate;
            }
        }

        fireCountdown -= Time.deltaTime;
    }
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

    protected void FaceEnemy(Transform target) //fix this - only rotates left and right
    {
        Vector3 direction = target.position - transform.position;
        float angle = Vector3.SignedAngle(Vector3.up, direction, Vector3.forward);
        Debug.Log("Angle to enemy: " + angle);
        if (angle >= -45f && angle <= 45f)
        {
            spriteRenderer.sprite = frontView;
            spriteRenderer.flipX = false;
        }
        else if (angle >= 135f || angle <= -135f)
        {
            spriteRenderer.sprite = backView;
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.sprite = sideView;
            spriteRenderer.flipX = direction.x < 0f; // Flip based on direction
        }
    }


}
