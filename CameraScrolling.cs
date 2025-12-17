using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScrolling : MonoBehaviour
{
    private Transform ourPlayer;

    private void Awake()
    {
        ourPlayer = GameObject.FindWithTag("Player").transform;

    }

    private void LateUpdate()
    {
        Vector3 updatedCameraPosition = transform.position;
        updatedCameraPosition.x = Mathf.Max(updatedCameraPosition.x, ourPlayer.position.x);
        transform.position = updatedCameraPosition;
    }
}
