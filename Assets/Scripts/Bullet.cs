using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform target;
    public float speed = 10f;

    public void Seek(Transform _target)
    {
        target = _target;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distThisFrame, Space.World);
    }

    void HitTarget()
    {
        // Optionally deal damage here
        Destroy(target.gameObject); // Just destroy for demo
        Money.Instance.AddMoney(5); //adding money - now it always adds. Hit detection should be implemented
        Destroy(gameObject);
    }
}
