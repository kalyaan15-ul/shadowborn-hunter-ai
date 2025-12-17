// I added simple comments to make it easier to understabd as well as easy for me to explain 
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public EnemyBlackboard blackboard; // Stores player info
    public EnemyShooter shooter;       // Shooting script

    public float moveSpeed = 4f; // Enemy speed
    public float stopDistance = 2f; // Distance he has to sotp 
    public float verticalSpeed = 2f; 

    private BTNode rootNode;           // Behaviour tree root

    void Start()
    {
        BuildBehaviourTree();          // Build the tree once
    }

    void Update()
    {
        // Run the tree every frame
        if (rootNode != null)
        {
            rootNode.Evaluate();
        }
    }

    void BuildBehaviourTree()
    {
        // LEAF NODES
        ConditionCanSeePlayer canSeePlayer = new ConditionCanSeePlayer(blackboard);

        ActionChasePlayer chasePlayer = new ActionChasePlayer(
            this.transform,
            blackboard,
            moveSpeed,
            stopDistance,
            verticalSpeed
        );

        ActionSetShooting enableShooting = new ActionSetShooting(shooter, true);

        ActionMoveToLastPosition moveToLastPosition = new ActionMoveToLastPosition(
            this.transform,
            blackboard,
            moveSpeed
        );

        ActionSetShooting disableShooting = new ActionSetShooting(shooter, false);

        // SEQUENCE: Engage player
        SequenceNode engagePlayerSequence = new SequenceNode(new List<BTNode>
        {
            canSeePlayer,
            chasePlayer,
            enableShooting
        });

        // SEQUENCE: Search for player
        SequenceNode searchSequence = new SequenceNode(new List<BTNode>
        {
            moveToLastPosition,
            disableShooting
        });

        // ROOT: Try to engage, otherwise search
        rootNode = new SelectorNode(new List<BTNode>
        {
            engagePlayerSequence,
            searchSequence
        });
    }
}
