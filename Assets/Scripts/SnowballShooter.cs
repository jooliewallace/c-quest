// SnowballShooter.cs

using System.Collections;
using UnityEngine;

public class SnowballShooter : MonoBehaviour
{
    public GameObject snowballPrefab;
    public Transform shootingPoint;
    public float minForce = 5f;
    public float maxForce = 15f;
    public float shootingInterval = 2f;
    public float snowballLifetime = 5f; // Adjust the lifetime as needed

    private void Start()
    {
        InvokeRepeating("ShootSnowball", 0f, shootingInterval);
    }

    private void ShootSnowball()
    {
        // Instantiate a new snowball at the shooting point
        GameObject snowball = Instantiate(snowballPrefab, shootingPoint.position, Quaternion.identity);

        // Calculate a random shooting direction with an upward angle
        Vector3 shootingDirection = Quaternion.Euler(Random.Range(-10f, 20f), Random.Range(-10f, 20f), Random.Range(10f, 30f)) * transform.forward;

        // Calculate a random shooting force
        float shootingForce = Random.Range(minForce, maxForce);

        // Add force to make the snowball move in the calculated direction
        Rigidbody snowballRb = snowball.GetComponent<Rigidbody>();
        snowballRb.AddForce(shootingDirection.normalized * shootingForce, ForceMode.Impulse);

        // Start coroutine to destroy the snowball after a certain time
        StartCoroutine(DestroySnowballAfterTime(snowball, snowballLifetime));
    }

    private IEnumerator DestroySnowballAfterTime(GameObject snowball, float lifetime)
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(snowball);
    }
}
