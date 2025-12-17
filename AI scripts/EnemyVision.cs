// I added simple comments to make it easier to understabd as well as easy for me to explain 
using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    public EnemyBlackboard blackboard;
    public Transform player;
    public float visionRange = 12f;   // How far the enemy can see

    void Update()
    {
        float dist = Vector2.Distance(transform.position, player.position);

        // If the player is close enough
        if (dist < visionRange)
        {
            Vector2 currentPos = player.position;
            Vector2 velocity = (currentPos - blackboard.lastSeenPosition) / Time.deltaTime;

            blackboard.playerVisible = true;        // mark visible
            blackboard.lastSeenPosition = currentPos; // update memory
            blackboard.lastSeenTime = Time.time;
            blackboard.player = player;
            blackboard.playerVelocity = velocity;
        }
        else
        {
            blackboard.playerVisible = false;      
        }
    }
}
