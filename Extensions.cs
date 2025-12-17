using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public static class Extensions
{
    private static LayerMask layerMask = LayerMask.GetMask("Default");
    public static bool Raycast(this Rigidbody2D rigidbody, Vector2 direction)
{
    if (rigidbody.isKinematic) {
        return false;
    }

    float capsuleWidth = 1f;     
    float capsuleHeight = 3.77f; 

    float distance = 0.1f;

    RaycastHit2D hit = Physics2D.CapsuleCast(
        rigidbody.position,
        new Vector2(capsuleWidth, capsuleHeight),
        CapsuleDirection2D.Vertical,
        0f,
        direction.normalized,
        distance,
        LayerMask.GetMask("Default")
    );

    return hit.collider != null && hit.rigidbody != rigidbody;
}

public static bool DotTest(this Transform transform, Transform other, Vector2 testDirection)
{
    Vector2 direction = other.position - transform.position;
    return Vector2.Dot(direction.normalized, testDirection) > 0.25f;
}




}
