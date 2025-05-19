using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    [Header("Tower Stats")]
    public float range;
    public float fireRate;

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
        range = 20f;
        fireRate = 2f;
    }

    protected virtual void Update()
    {
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
            //FaceEnemy(nearest.transform, Camera.main);

            if (fireCountdown <= 0f)
            {
                Shoot(nearest.transform);
                fireCountdown = 1f / fireRate;
            }
        }
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        inputDirection = new Vector3(x, 0, z).normalized;

        fireCountdown -= Time.deltaTime;
    }

    protected virtual void Shoot(Transform target)
    {
        GameObject bulletGO = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bullet = bulletGO.GetComponent<Bullet>();
        if (bullet != null)
        {
            bullet.Seek(target);
        }

        Debug.Log("Bullet spawned at: " + firePoint.position);
    }

    private void FaceEnemy(Transform target, Camera cam)
    {
        Vector3 direction = target.position - transform.position;
        float angle = Vector3.SignedAngle(Vector3.up, direction, Vector3.forward);

        // Subtract camera's Z rotation so the facing is camera-relative
        angle -= cam.transform.eulerAngles.z;

        // Normalize angle to [-180, 180]
        angle = Mathf.Repeat(angle + 180f, 360f) - 180f;

        if (angle >= -45 && angle <= 45)
        {
            spriteRenderer.sprite = frontView;
            spriteRenderer.flipX = false;
        }
        else if (angle >= 135 || angle <= -135)
        {
            spriteRenderer.sprite = backView;
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.sprite = sideView;
            spriteRenderer.flipX = (direction.x < 0);
        }
    }


}
