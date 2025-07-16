using System.Collections;
using UnityEngine;

public class SpikedBallSpawner : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private GameObject spikedBallPrefab; 
    [Header("Spawn Settings")]
    [SerializeField] private float spawnInterval = 1.5f;
    [SerializeField] private float initialDelay = 0.5f;
    [SerializeField] private int ballsPerSpawn = 1;

    private BoxCollider2D triggerZone;
    private Coroutine spawnCoroutine;

    private void Awake()
    {
        triggerZone = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (spawnCoroutine == null)
            {
                spawnCoroutine = StartCoroutine(SpawnBallsRoutine());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (spawnCoroutine != null)
            {
                StopCoroutine(spawnCoroutine);
                spawnCoroutine = null;
            }
        }
    }

    private IEnumerator SpawnBallsRoutine()
    {
        yield return new WaitForSeconds(initialDelay);
        while (true)
        {
            for (int i = 0; i < ballsPerSpawn; i++)
            {
                SpawnOneBall();
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnOneBall()
    {
        if (spikedBallPrefab == null) return;
        Bounds bounds = triggerZone.bounds;
        float spawnX = Random.Range(bounds.min.x, bounds.max.x);
        float spawnY = bounds.max.y; 
        Vector2 spawnPosition = new Vector2(spawnX, spawnY);
        Instantiate(spikedBallPrefab, spawnPosition, Quaternion.identity);
    }
}