using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRocketMove : MonoBehaviour
{
    public GameObject target;
    private Rigidbody rocketRb;
    public float rocketSpeed = 10f;
    private Vector3 direction;

    public float selfDestroyRange =20f;
    //public float rocketRotationSpeed = 10f;
    // Start is called before the first frame update
    void Start()
    {
        rocketRb = GetComponent<Rigidbody>();
        direction = (target.transform.position - transform.position).normalized;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = targetRotation;
        rocketRb.AddForce(direction * rocketSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        rocketRb.AddForce(direction * rocketSpeed); 

        if(transform.position.x<-selfDestroyRange|| transform.position.x > selfDestroyRange || transform.position.z < -selfDestroyRange || transform.position.z > selfDestroyRange  )
        {
            Destroy(gameObject);
        }

    }
}