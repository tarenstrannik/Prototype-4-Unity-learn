using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirthChildren : MonoBehaviour
{
    public GameObject[] enemyPrefab;
    private float spawnRange = 9f;
    public float minChildTime = 1.0f;
    public float maxChildTime = 10f;

    // Start is called before the first frame update
    void Start()
    {
        float randChildTime = Random.Range(minChildTime, maxChildTime);
        Invoke("GenerateChildEnemy", randChildTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private Vector3 GenerateSpawnPosition()
    {
        float spawnPosX = Random.Range(-spawnRange, spawnRange);
        float spawnPosZ = Random.Range(-spawnRange, spawnRange);
        Vector3 spawnPos = new Vector3(spawnPosX, 0, spawnPosZ);
        return spawnPos;
    }
    private void GenerateChildEnemy()
    {
        int enemyIndex = Random.Range(0, enemyPrefab.Length);
        Instantiate(enemyPrefab[enemyIndex], GenerateSpawnPosition(), enemyPrefab[enemyIndex].transform.rotation);
        float randChildTime = Random.Range(minChildTime, maxChildTime);
        Invoke("GenerateChildEnemy", randChildTime);
    }
}
