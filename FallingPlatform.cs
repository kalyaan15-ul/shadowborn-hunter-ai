using UnityEngine;
using System.Collections;

public class FallingPlatform : MonoBehaviour
{
    public float shakeTime = 0.3f;
    public float fallDelay = 0.5f;

    Rigidbody2D rb;
    Vector3 originalPos;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalPos = transform.position;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            StartCoroutine(ShakeAndFall());
        }
    }

    IEnumerator ShakeAndFall()
    {
        float timer = 0f;

        while (timer < shakeTime)
        {
            transform.position = originalPos + (Vector3)Random.insideUnitCircle * 0.05f;
            timer += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPos;

        yield return new WaitForSeconds(fallDelay);

        rb.bodyType = RigidbodyType2D.Dynamic;  // platform falls
    }
}
