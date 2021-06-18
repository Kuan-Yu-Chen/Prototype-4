using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 3.0f;
    private Rigidbody playerRB;
    private GameObject focalPoint;
    public bool hasPowerUp = false;
    private float powerUpStrength = 15.0f;
    public GameObject powerUpIndicator;
    public GameObject bulletPrefab;
    public PowerUpType currentPowerUp;
    private Coroutine powerupCountDown;
    public GameObject tempBullet;

    // Start is called before the first frame update
    void Start()
    {
        playerRB = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
    }

    // Update is called once per frame
    void Update()
    {
        float verticalInput = Input.GetAxis("Vertical");
        playerRB.AddForce(focalPoint.transform.forward * verticalInput * speed);
        powerUpIndicator.transform.position = transform.position + new Vector3(0, -0.5f, 0);

        if (Input.GetKeyDown(KeyCode.Space) && currentPowerUp == PowerUpType.Bullet) {
            Shoot();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PowerUp"))
        {
            hasPowerUp = true;
            powerUpIndicator.gameObject.SetActive(true);
            currentPowerUp = other.gameObject.GetComponent<PowerUp>().powerUpType;            
            Destroy(other.gameObject);

            if (powerupCountDown != null) {
                StopCoroutine(powerupCountDown);
            }
            powerupCountDown = StartCoroutine(PowerUpCountDownRoutine());
        }
        
    }

    IEnumerator PowerUpCountDownRoutine()
    {
        yield return new WaitForSeconds(7.0f);
        hasPowerUp = false;
        currentPowerUp = PowerUpType.None;
        powerUpIndicator.gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && hasPowerUp && currentPowerUp == PowerUpType.Pushback) 
        {
            Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = collision.gameObject.transform.position - transform.position;
            enemyRb.AddForce(awayFromPlayer * powerUpStrength, ForceMode.Impulse);
            Debug.Log("Collide with " + collision.gameObject.name + " with powerup type set to " + currentPowerUp.ToString());
        }

    }

    private void Shoot() {

        foreach (var enemy in FindObjectsOfType<Enemy>()) {
            tempBullet = Instantiate(bulletPrefab, transform.position + Vector3.up, bulletPrefab.transform.rotation); ;
            tempBullet.GetComponent<ShootBullet>().Fire(enemy.transform);
        }
        
    }
}
