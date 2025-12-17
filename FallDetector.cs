using UnityEngine;

public class FallDetector : MonoBehaviour
{
    public float fallY = -100f;  // change height as needed

    void Update()
    {
        if (transform.position.y < fallY)
        {
            Debug.Log("PLAYER FELL! GAME OVER");
            UIManager.Instance.ShowLoseScreen();
        }
    }
}
