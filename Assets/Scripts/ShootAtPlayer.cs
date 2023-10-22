using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAtPlayer : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject player;
    public GameObject bossRocketPrefab;
    public float minShootDelay = 1.0f;
    public float maxShootDelay = 3f;
    void Start()
    {
        player = GameObject.Find("Player");
        float randShootDelay = Random.Range(minShootDelay, maxShootDelay);
        Invoke("Shoot", randShootDelay);
    }
    private void Shoot()
    {

            Vector3 eSdvigDirection = (player.transform.position - transform.position).normalized*transform.localScale.y/2;

            GameObject rocketTemp = Instantiate(bossRocketPrefab, transform.position + eSdvigDirection, bossRocketPrefab.transform.rotation);
            rocketTemp.GetComponent<BossRocketMove>().target = player;
        
            rocketTemp.GetComponent<RocketCollide>().rocketOwner = gameObject;
        float randShootDelay = Random.Range(minShootDelay, maxShootDelay);
        Invoke("Shoot", randShootDelay);
    }
    // Update is called once per frame
    void Update()
    {
        
        
    }
}
