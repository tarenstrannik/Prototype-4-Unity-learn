using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;



public class PlayerController : MonoBehaviour
{
    public float speed = 5f;

    private GameObject focalPoint;
    private Rigidbody playerRb;

    public float powerupStrength = 20f;
    public float powerupLast = 4f;
    private float curPowerupLast;
    public GameObject powerupIndicator;
    private MeshFilter powerupIndicatorMF;
    public Mesh powerupIndicatorMegaforce;
    public Mesh powerupIndicatorRockets;
    public Mesh powerupIndicatorSmash;

    public float jumpForce = 5f;
    public float playerPrevY = 0f;
    public float smashForce = 5f;
    public float smashExplForce = 0f;
    public float deltaSmashExplForce = 5f;
    public float shashExplosionRadius = 10f;
    public float smashExplForceUp = 1f;

    private GameObject[] enemies;
    public GameObject rocketPrefab;
    // Start is called before the first frame update
    public bool isSmashing = false;
    public bool forceDown = false;
    public enum Powerups
    {
        None,
        Megaforce,
        Rockets,
        Smash
    }

    private Powerups curPowerup=Powerups.None;
    void Start()
    {
        playerRb=GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("FocalPoint");
        powerupIndicatorMF = powerupIndicator.GetComponent<MeshFilter>();
    }

    // Update is called once per frame
    void Update()
    {
        float verticalInput = Input.GetAxis("Vertical");
        playerRb.AddForce(focalPoint.transform.forward * verticalInput * speed);
        powerupIndicator.transform.position = transform.position - new Vector3(0, 0.5f, 0);



        if (Input.GetKeyDown(KeyCode.Space))
        {

            if (curPowerup == Powerups.Rockets)
            {
            
                Shoot();
            }
            if (isSmashing && !forceDown) forceDown = true;
            if (curPowerup == Powerups.Smash && !isSmashing)
            {

                Smash();
            }
            
        }
    }

    private void Shoot()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach( GameObject e in enemies )
        {
            Vector3 eSdvigDirection = (e.transform.position - transform.position).normalized;

            GameObject rocketTemp = Instantiate(rocketPrefab, transform.position+ eSdvigDirection, rocketPrefab.transform.rotation);
            rocketTemp.GetComponent<RocketMove>().target = e;
            rocketTemp.GetComponent<RocketCollide>().rocketOwner = gameObject;
        }
    }

    private void Smash()
    {   if (!isSmashing)
        {
            smashExplForce = 0f;
            playerRb.velocity = Vector3.zero;
            isSmashing = true;
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
           StartCoroutine(WaitTillUp());
        }
    }

    IEnumerator WaitTillUp()
    {
        while(true)
        {
            smashExplForce += deltaSmashExplForce * Time.deltaTime;
            if (transform.position.y< playerPrevY || forceDown)
            {
                forceDown = true;
                playerRb.AddForce(-Vector3.up * smashForce, ForceMode.Impulse);
                playerPrevY = 0f;
                yield break;
            }
            else
            {
                playerPrevY = transform.position.y;
            }
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {
            powerupIndicator.gameObject.SetActive(true);
            curPowerup = other.gameObject.GetComponent<PowerupController>().typeOfPowerup;
            Destroy(other.gameObject);
            SetPowerupIndicatorMesh();
            curPowerupLast = powerupLast;
            StartCoroutine(PowerupCountdownRoutine());
        }
        
    }

    IEnumerator PowerupCountdownRoutine()
    {
        
        while (true)
        {
            curPowerupLast -= Time.deltaTime;
            if(curPowerupLast<=0)
            {
                curPowerup = Powerups.None;
                powerupIndicator.gameObject.SetActive(false);
                yield break;
            }
            yield return null;
        }
       
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && curPowerup==Powerups.Megaforce)
        {
            Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = (collision.gameObject.transform.position - transform.position).normalized;
            enemyRb.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
        }
        if(collision.gameObject.CompareTag("Ground") && isSmashing)
        {
            isSmashing = false;
            forceDown = false;
            Explosion(collision.GetContact(0).point);
        }
    }

    private void Explosion(Vector3 explPos)
    {
        
        Collider[] colliders = Physics.OverlapSphere(explPos, shashExplosionRadius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            //Debug.Log(smashExplForce);
            if (rb != null && rb.name != transform.name&& !rb.gameObject.CompareTag("Powerup") && !rb.gameObject.CompareTag("Rocket"))
                rb.AddExplosionForce(smashExplForce, explPos, shashExplosionRadius, smashExplForceUp, ForceMode.Impulse);
        }
        
    }
    private void SetPowerupIndicatorMesh()
    {
        if(curPowerup == Powerups.Megaforce)
        {
            powerupIndicatorMF.mesh = powerupIndicatorMegaforce;
        }
        else if (curPowerup == Powerups.Rockets)
        {
            powerupIndicatorMF.mesh = powerupIndicatorRockets;
        }
        else if (curPowerup == Powerups.Smash)
        {
            powerupIndicatorMF.mesh = powerupIndicatorSmash;
        }


    }
}
