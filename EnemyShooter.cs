using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    public Transform firePoint;       // Where bullets spawn
    public GameObject fireballPrefab; // Bullet prefab
    public float fireForce = 10f;
    public float shootInterval = 1.5f;

    public EnemyBlackboard blackboard;

    private float timer = 0f;

    void Update()
    {
        // Behaviour tree enables/disables this script
        if (!enabled) return;

        timer += Time.deltaTime;

        // Shoot after interval
        if (timer >= shootInterval)
        {
            Shoot();
            timer = 0f;
        }
    }

    void Shoot()
    {
        if (blackboard.player == null) return;

        GameObject ball = Instantiate(fireballPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();

        // Predict where player will be
        Vector2 predictedPos =
            (Vector2)blackboard.player.position +
            blackboard.playerVelocity * 0.4f;

        // Shoot toward predicted point
        Vector2 direction =
            (predictedPos - (Vector2)firePoint.position).normalized;

        rb.velocity = direction * fireForce;
    }
}
