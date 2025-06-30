using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform target;
    private Vector3 direction;
    public float speed = 10f;
    public float lifetime = 3f; // bullet gets destroyed after this time
    public int damage = 10;

    public void Seek(Transform _target)
    {
        target = _target;
        if (target != null)
        {
            direction = (target.position - transform.position).normalized;
        }
        else
        {
            // No target, set default forward direction so bullet doesn't get stuck
            direction = transform.forward;
        }

        Destroy(gameObject, lifetime); // Automatically destroy after time
    }

    void Update()
    {
        if (target != null)
        {
            // Update direction every frame to track target
            direction = (target.position - transform.position).normalized;

            float distThisFrame = speed * Time.deltaTime;
            float distanceToTarget = Vector3.Distance(transform.position, target.position);

            if (distanceToTarget <= distThisFrame)
            {
                HitTarget();
                return;
            }

            // Move bullet toward the target
            transform.Translate(direction * distThisFrame, Space.World);
        }
        else
        {
            // Target lost, move bullet forward in last known direction
            // or just destroy it immediately
            // Option 1: Destroy bullet immediately
            Destroy(gameObject);

            // Option 2: Uncomment below to keep flying forward
            // transform.Translate(direction * speed * Time.deltaTime, Space.World);
        }
    }

    void HitTarget()
    {
        if (target != null)
        {
            Enemy enemy = target.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
        Destroy(gameObject);
    }
}
