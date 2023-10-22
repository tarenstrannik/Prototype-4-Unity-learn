using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] enemyPrefab;
    public GameObject[] bossPrefab;
    public GameObject[] powerupPrefab;

    private float spawnRange=9f;

    private int enemyCount;
    private int waveNumber=1;

    public int bossWaveKratn = 1;
    void Start()
    {
        SpawnEnemyWave(waveNumber);
        SpawnPowerup();
    }

    // Update is called once per frame
    void Update()
    {
        enemyCount = FindObjectsByType<Enemy>(FindObjectsSortMode.None).Length;
        if(enemyCount==0)
        {
            waveNumber++;
            if (waveNumber % bossWaveKratn == 0)
            {
                SpawnBoss();
            }
            else
            {
                SpawnEnemyWave(waveNumber);
            }
            
            SpawnPowerup();
        }
    }
    private Vector3 GenerateSpawnPosition()
    {
        float spawnPosX = Random.Range(-spawnRange, spawnRange);
        float spawnPosZ = Random.Range(-spawnRange, spawnRange);
        Vector3 spawnPos = new Vector3(spawnPosX, 0, spawnPosZ);
        return spawnPos;
    }
    private void SpawnEnemyWave(int enemiesToSpawn)
    {
        for(var i = 0;i< enemiesToSpawn; i++)
        {
            GenerateEnemy();
        }
    }
    private void SpawnBoss()
    {
        int bossIndex = Random.Range(0, bossPrefab.Length);
        Instantiate(bossPrefab[bossIndex], GenerateSpawnPosition()+new Vector3(0, bossPrefab[bossIndex].transform.localScale.y/2,0), bossPrefab[bossIndex].transform.rotation);
    }
    private void GenerateEnemy()
    {
        int enemyIndex = Random.Range(0, enemyPrefab.Length);
        Instantiate(enemyPrefab[enemyIndex], GenerateSpawnPosition(), enemyPrefab[enemyIndex].transform.rotation);
    }
    private void SpawnPowerup()
    {
        int powerupIndex = Random.Range(0, powerupPrefab.Length);
        Instantiate(powerupPrefab[powerupIndex], GenerateSpawnPosition(), powerupPrefab[powerupIndex].transform.rotation);
    }
}
