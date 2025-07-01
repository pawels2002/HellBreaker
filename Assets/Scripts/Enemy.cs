using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 3f;
    public int prize = 10;
    public int maxHP = 100;
    public int damage = 10;

    private Transform target;
    private int waypointIndex = 0;
    private int currentHP;//
    private Renderer[] renderers;
    private Color originalColor;
    private bool isFlashing = false;
    private bool isDead = false;

    public AudioClip deathClip;
    private AudioSource audioSource;

    public Vector3 CurrentDirection { get; private set; }

    private void Start()
    {
        target = Waypoints.points[0];
        transform.rotation = Quaternion.Euler(45f, 0f, 0f);
    }

    void Awake()//
    {
        currentHP = maxHP;
        renderers = GetComponentsInChildren<Renderer>();
        if (renderers.Length > 0)
        {
            originalColor = renderers[0].material.color;
        }
        audioSource = GetComponent<AudioSource>();
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
            if (!isDead)
            {
                isDead = true;
                GameManager.instance.CastleTakeDamage(damage);
                Spawner.onEnemyDestroy.Invoke();
                Destroy(gameObject);
            }
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

        if(!isFlashing)
        {
            StartCoroutine(FlashRed());
        }
    }

    private IEnumerator FlashRed()
    {
        isFlashing = true;
        float timer = 0f;
        bool isRed = false;

        while (timer < 0.37f)
        {
            SetEnemyColor(isRed ? Color.red : originalColor);
            isRed = !isRed;
            yield return new WaitForSeconds(0.07f);
            timer += 0.07f;
        }

        SetEnemyColor(originalColor);
        isFlashing = false;
    }

    private void SetEnemyColor(Color color)
    {
        if (renderers == null) return;
        foreach (var rend in renderers)
        {
            rend.material.color = color;
        }
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;
        Spawner.onEnemyDestroy.Invoke();
        Money.Instance.AddMoney(prize);

        if (audioSource != null && deathClip != null)
        {
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.PlayOneShot(deathClip);
            foreach (var rend in renderers)
                rend.enabled = false;
            GetComponent<Collider>().enabled = false;
            Destroy(gameObject, deathClip.length / audioSource.pitch);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void SetPlayerColor(Color color)
    {
        if (renderers == null) return;
        foreach (var rend in renderers)
        {
            rend.material.color = color;
        }
    }
}
