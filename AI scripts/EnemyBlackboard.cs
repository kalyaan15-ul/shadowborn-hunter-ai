// I added simple comments to make it easier to understabd as well as easy for me to explain 
using UnityEngine;

public class EnemyBlackboard : MonoBehaviour
{
    public Transform player;          // Player reference
    public bool playerVisible = false; // Can we see the player?

    public Vector2 lastSeenPosition;   // Saved last known position
    public float lastSeenTime = 0f;    // When we saw them

    public Vector2 playerVelocity;     // Estimated speed/direction
}
