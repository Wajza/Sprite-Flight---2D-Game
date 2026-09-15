using UnityEngine;

public class Shields : MonoBehaviour
{
    public float maxSize = 0.5f;
    public float minSize = 0.1f;

    public float minSpeed = 50f;
    public float maxSpeed = 150f;

    public float maxspinSpeed = 10f;
    Rigidbody2D rb;
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
    void Update()
    {
        
    }
}
