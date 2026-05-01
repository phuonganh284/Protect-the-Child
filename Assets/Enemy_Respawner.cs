using UnityEngine;

public class Enemy_Respawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private Transform[] respawnPoints;
    [SerializeField] private float cooldown;
    [SerializeField] private float cooldownDecreaseRate = 0.05f;
    [SerializeField] private float cooldownCap = 0.7f;
    private Transform player;
    private float timer;
    void Awake()
    {
        player = FindAnyObjectByType<Player>().transform;
    }
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            timer = cooldown;
            CreateNewEnemy();
            cooldown = Mathf.Max(cooldownCap, cooldown - cooldownDecreaseRate);
        }
    }
    private void CreateNewEnemy()
    {
        int prefabIndex = Random.Range(0, enemyPrefabs.Length);
        GameObject selectedPrefab = enemyPrefabs[prefabIndex];
        Debug.Log(selectedPrefab.name);

        int respawnPointIndex = Random.Range(0, respawnPoints.Length);
        Vector3 spawnPoint = respawnPoints[respawnPointIndex].position;

        GameObject newEnemy = Instantiate(selectedPrefab, spawnPoint, Quaternion.identity);

        bool createdOnTheRight = newEnemy.transform.position.x > player.transform.position.x;

        if (createdOnTheRight)
            newEnemy.GetComponent<Enemy>().Flip();
    }
}
