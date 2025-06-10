//using System.Diagnostics;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 3f;

    private Transform target;
    private int waypointIndex = 0;

    public int maxHP = 100;//
    private int currentHP;//

    public Vector3 CurrentDirection { get; private set; }

    private void Start()
    {
        target = Waypoints.points[0];
        transform.rotation = Quaternion.Euler(45f, 0f, 0f);
    }

    void Awake()//
    {
        currentHP = maxHP;
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

    public void TakeDamage(int amount)//
    {
        currentHP -= amount;
        Debug.Log($"{gameObject.name} took {amount} damage. HP left: {currentHP}");
        if (currentHP <= 0)
        {
            Die();
        }
    }

    private void Die()//
    {
        Debug.Log($"{gameObject.name} died!");
        Destroy(gameObject);
    }
}
