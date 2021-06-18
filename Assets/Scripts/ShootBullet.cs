using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBullet : MonoBehaviour
{
    private float speed = 20.0f;
    //public Vector3 enemyDirection;
    private Transform target;
    private bool hasShoot = false;
    private float aliveTimer = 5.0f;
    private float bulletStrength = 15.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (hasShoot && target != null) {
            Vector3 enemyDirection = (target.transform.position - transform.position).normalized;
            //transform.Translate(enemyDirection * speed * Time.deltaTime); //bullet will trace the enemy!! Translate acts on local coordinate.
            transform.position += enemyDirection * speed * Time.deltaTime;
            transform.LookAt(target);
        }

        //Destroy bullet when enemy falls out of stage
        if (target == null) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy")) {
            
            Rigidbody enemyRb = other.gameObject.GetComponent<Rigidbody>();

            Vector3 awayDirection = other.gameObject.transform.position - transform.position;
    
            enemyRb.AddForce(awayDirection.normalized * bulletStrength, ForceMode.Impulse);

            Destroy(gameObject);
        }
    }

    public void Fire(Transform enemyTarget) {
        target = enemyTarget;
        hasShoot = true;
        Destroy(gameObject, aliveTimer);
    }
}
