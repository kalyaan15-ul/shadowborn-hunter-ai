// I added simple comments to make it easier to understabd as well as easy for me to explain 

using System.Collections.Generic;
using UnityEngine;

// Possible results from each behaviour tree node
public enum NodeState
{
    Success,
    Failure,
    Running
}

// Base class for all behaviour tree nodes
public abstract class BTNode
{
    public abstract NodeState Evaluate();
}

// This is the SELECTOR NODE
// Tries each child one by one.
// If a child succeeds or is still running → selector returns that child.
public class SelectorNode : BTNode
{
    private List<BTNode> children;

    public SelectorNode(List<BTNode> children)
    {
        this.children = children; // store the list of children
    }

    public override NodeState Evaluate()
    {
        foreach (BTNode child in children)
        {
            NodeState state = child.Evaluate();

            // If any child works, stop and return it
            if (state == NodeState.Success || state == NodeState.Running)
            {
                return state;
            }
        }

        // None worked
        return NodeState.Failure;
    }
}

// This is the SEQUENCE NODE
// Runs children in order.
// If any child fails → whole sequence fails.
// If all succeed → sequence succeeds.
public class SequenceNode : BTNode
{
    private List<BTNode> children;

    public SequenceNode(List<BTNode> children)
    {
        this.children = children; // store steps
    }

    public override NodeState Evaluate()
    {
        foreach (BTNode child in children)
        {
            NodeState state = child.Evaluate();

            // If a step fails then stop the sequence
            if (state == NodeState.Failure)
            {
                return NodeState.Failure;
            }

            // If a step is still running then return running
            if (state == NodeState.Running)
            {
                return NodeState.Running;
            }
        }

        // All steps succeeded
        return NodeState.Success;
    }
}

// CONDITION: Can the enemy see the player?
public class ConditionCanSeePlayer : BTNode
{
    private EnemyBlackboard blackboard;

    public ConditionCanSeePlayer(EnemyBlackboard blackboard)
    {
        this.blackboard = blackboard;
    }

    public override NodeState Evaluate()
    {
        // Checks the blackboard for visibility
        if (blackboard != null && blackboard.playerVisible && blackboard.player != null)
        {
            return NodeState.Success;
        }

        return NodeState.Failure;
    }
}

// ACTION: Chase the player
// Moves horizontally toward player and adjusts height slowly.
public class ActionChasePlayer : BTNode
{
    private Transform enemyTransform;
    private EnemyBlackboard blackboard;
    private float moveSpeed;
    private float stopDistance;
    private float verticalSpeed;

    public ActionChasePlayer(Transform enemyTransform,
                             EnemyBlackboard blackboard,
                             float moveSpeed,
                             float stopDistance,
                             float verticalSpeed)
    {
        this.enemyTransform = enemyTransform;
        this.blackboard = blackboard;
        this.moveSpeed = moveSpeed;
        this.stopDistance = stopDistance;
        this.verticalSpeed = verticalSpeed;
    }

    public override NodeState Evaluate()
    {
        // If we don’t know where the player is then fail
        if (blackboard == null || blackboard.player == null)
        {
            return NodeState.Failure;
        }

        Vector2 target = blackboard.player.position;

        // Make enemy face the player
        if (target.x > enemyTransform.position.x)
        {
            enemyTransform.localScale = new Vector3(1f, 1f, 1f);
        }
        else
        {
            enemyTransform.localScale = new Vector3(-1f, 1f, 1f);
        }

        float dist = Vector2.Distance(enemyTransform.position, target);

        // Move only if far from player
        if (dist > stopDistance)
        {
            enemyTransform.position = Vector2.MoveTowards(
                enemyTransform.position,
                new Vector2(target.x, enemyTransform.position.y),
                moveSpeed * Time.deltaTime
            );
        }

        // Adjust vertical position
        float newY = Mathf.MoveTowards(
            enemyTransform.position.y,
            target.y,
            verticalSpeed * Time.deltaTime
        );

        enemyTransform.position = new Vector2(enemyTransform.position.x, newY);

        return NodeState.Running; // keep chasing
    }
}

// ACTION: Move to where the player was last seen
public class ActionMoveToLastPosition : BTNode
{
    private Transform enemyTransform;
    private EnemyBlackboard blackboard;
    private float moveSpeed;

    private float arrivalThreshold = 0.1f; // distance to consider "arrived"

    public ActionMoveToLastPosition(Transform enemyTransform,
                                    EnemyBlackboard blackboard,
                                    float moveSpeed)
    {
        this.enemyTransform = enemyTransform;
        this.blackboard = blackboard;
        this.moveSpeed = moveSpeed;
    }

    public override NodeState Evaluate()
    {
        if (blackboard == null)
        {
            return NodeState.Failure;
        }

        Vector2 target = blackboard.lastSeenPosition;

        // Move enemy toward that saved spot
        enemyTransform.position = Vector2.MoveTowards(
            enemyTransform.position,
            target,
            moveSpeed * Time.deltaTime
        );

        float dist = Vector2.Distance(enemyTransform.position, target);

        // If reached
        if (dist <= arrivalThreshold)
        {
            return NodeState.Success;
        }

        return NodeState.Running; // still moving
    }
}

// ACTION: Turn shooting ON or OFF
public class ActionSetShooting : BTNode
{
    private EnemyShooter shooter;
    private bool enable;

    public ActionSetShooting(EnemyShooter shooter, bool enable)
    {
        this.shooter = shooter;
        this.enable = enable;
    }

    public override NodeState Evaluate()
    {
        if (shooter == null)
        {
            return NodeState.Failure;
        }

        shooter.enabled = enable; // activate/deactivate shooting

        return NodeState.Success;
    }
}
