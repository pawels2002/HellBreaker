using System.Collections;
using TMPro;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.Events;

public class Spawner : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private GameObject[] enemyPrefab;

    [Header("Attributes")]
    [SerializeField] private int baseEnemies = 8;
    [SerializeField] private float enemiesPerSecond = 0.5f;
    [SerializeField] private float timeBetweenWaves = 20f;
    [SerializeField] private float difficultyScalingFactor = 0.75f;
    [SerializeField] public Transform spawnPoint;
    [SerializeField] public int maxWaves = 5;
    [SerializeField] private RectTransform clockArrow;


    [Header("UI")]
    [SerializeField] private TextMeshProUGUI waveCounterText;


    [Header("Events")]
    public static UnityEvent onEnemyDestroy;

    private int currentWave = 1;
    private float timeSinceLastSpawn;
    private int enemiesAlive;
    private int enemiesLeftToSpawn;
    private bool isSpawning = false;
    private float waveCountdown = 0f;
    private bool isWaitingForWave = false;
    private int totalEnemiesThisWave = 0;
    float startAngle = 45f;
    private bool waveStarted = false;


    private void Awake()
    {
        if (onEnemyDestroy == null)
        {
            onEnemyDestroy = new UnityEvent();
        }
        onEnemyDestroy.AddListener(EnemyDestroyed);
    }

    private void Start()
    {
        UpdateWaveCounter();
        StartCoroutine(WaveCooldownMethod());
    }

    private void Update()
    {
        if (isWaitingForWave)
        {
            waveCountdown -= Time.deltaTime;
            float t = Mathf.Clamp01(waveCountdown / timeBetweenWaves);
            if (clockArrow != null)
                clockArrow.localRotation = Quaternion.Euler(0, 0, startAngle - 360f * (1 - t));
            if (waveCountdown <= 0f)
                isWaitingForWave = false;
            return;
        }
        if (!isSpawning) return;
        timeSinceLastSpawn += Time.deltaTime;
        float progress = 1f;
        if (totalEnemiesThisWave > 0)
            progress = (float)(enemiesLeftToSpawn + enemiesAlive) / totalEnemiesThisWave;

        if (clockArrow != null)
            clockArrow.localRotation = Quaternion.Euler(0, 0, startAngle - 360f * (1 - progress));

        if (timeSinceLastSpawn >= (1f / enemiesPerSecond) && enemiesLeftToSpawn > 0)
        {
            enemiesLeftToSpawn--;
            SpawnEnemy(); 
            timeSinceLastSpawn = 0f;
        }

        if (enemiesLeftToSpawn == 0 && enemiesAlive == 0 && isSpawning)
        {
            EndWave();
        }
    }

    private void EndWave()
    {
        isSpawning = false;
        timeSinceLastSpawn = 0f;

        if (currentWave < maxWaves)
        {
            currentWave++;
            UpdateWaveCounter();
            StartCoroutine(WaveCooldownMethod());
        }
        else
        {
            Debug.Log("All waves completed!");
            GameManager.instance.CompleteLevel();
        }
    }

    private IEnumerator WaveCooldownMethod()
    {
        waveCountdown = timeBetweenWaves;
        isWaitingForWave = true;
        while (waveCountdown > 0f)
        {
            waveCountdown -= Time.deltaTime;
            float t = Mathf.Clamp01(waveCountdown / timeBetweenWaves);
            if (clockArrow != null)
                clockArrow.localRotation = Quaternion.Euler(0, 0, startAngle - 360f * (1 - t));
            yield return null;
        }
        isWaitingForWave = false;
        isSpawning = true;
        enemiesLeftToSpawn = EnemiesPerWave();
        totalEnemiesThisWave = enemiesLeftToSpawn;
        waveStarted = false;

    }

    private void EnemyDestroyed()
    {
        enemiesAlive--;
        Debug.Log("Enemy Destroyed, current enemies alive: "+enemiesAlive);
    }

    private void UpdateWaveCounter()
    {
        if (waveCounterText != null)
            waveCounterText.text = $"Wave: {currentWave}/{maxWaves}";
    }

    private void SpawnEnemy()
    {
        int index = Random.Range(0, enemyPrefab.Length);
        GameObject prefabToSpawn = enemyPrefab[index];
        Instantiate(prefabToSpawn, spawnPoint.position, spawnPoint.rotation);
        enemiesAlive++;
        Debug.Log("Enemy Spawned, current enemies alive: " + enemiesAlive);
        waveStarted = true;
    }

    /*private IEnumerator StartWave()
    {
        isSpawning = true;
        enemiesLeftToSpawn = EnemiesPerWave();
        totalEnemiesThisWave = enemiesLeftToSpawn;
        yield break;

    }*/

    private int EnemiesPerWave()
    {
        return Mathf.RoundToInt(baseEnemies * Mathf.Pow(currentWave, difficultyScalingFactor));
    }
}
