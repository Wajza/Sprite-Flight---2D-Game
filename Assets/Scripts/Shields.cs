//AI was used to help with some adujments to the code, but the majority of the code was written by me.

using UnityEngine;

public class Shields : MonoBehaviour
{
    public float maxSize = 0.5f;
    public float minSize = 0.1f;

    public float minSpeed = 50f;
    public float maxSpeed = 150f;

    public float maxspinSpeed = 10f;
    Rigidbody2D rb;
    public GameObject bounceEffectPrefab; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        float randomSize = Random.Range(minSize, maxSize);
        transform.localScale = new Vector3(randomSize, randomSize, 1);

        rb = GetComponent<Rigidbody2D>();

        float randomSpeed = Random.Range(minSpeed, maxSpeed) / randomSize;
        Vector2 randomDirection = Random.insideUnitCircle;
        rb.AddForce(randomDirection * randomSpeed);

        float randomSpin = Random.Range(-maxspinSpeed, maxspinSpeed);
        rb.AddTorque(randomSpin);
    }

    // Update is called once per frame
    void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 contactPoint = collision.GetContact(0).point; 
        GameObject bounceEffect = Instantiate(bounceEffectPrefab, contactPoint, Quaternion.identity);
        Destroy(bounceEffect, 1f);
    }
}
