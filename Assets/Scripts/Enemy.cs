using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 3f;

    private Transform target;
    private int waypointIndex = 0;

    public Vector3 CurrentDirection { get; private set; }

    private void Start()
    {
        target = Waypoints.points[0];
        transform.rotation = Quaternion.Euler(45f, 0f, 0f);
    }

    private void Update()
    {
        Vector3 dir = target.position - transform.position;
        CurrentDirection = dir.normalized;

        transform.Translate(CurrentDirection * speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, target.position) <= 0.2f)
        {
            GetNextWaypoint();
        }
    }

    void GetNextWaypoint()
    {
        if (waypointIndex >= Waypoints.points.Length - 1)
        {
            Destroy(gameObject);
            return;
        }

        waypointIndex++;
        target = Waypoints.points[waypointIndex];
    }
}
