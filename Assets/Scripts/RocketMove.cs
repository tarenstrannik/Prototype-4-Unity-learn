using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketMove : MonoBehaviour
{
    public GameObject target;
    private Rigidbody rocketRb;
    public float rocketSpeed = 10f;
    public float rocketRotationSpeed = 10f;
    // Start is called before the first frame update
    void Start()
    {
        rocketRb = GetComponent<Rigidbody>();
        Vector3 direction = (target.transform.position - transform.position).normalized;
        
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = targetRotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            Vector3 direction = (target.transform.position - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rocketRotationSpeed * Time.deltaTime);
            rocketRb.AddForce(direction * rocketSpeed);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
