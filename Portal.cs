using UnityEngine;

public class Portal : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("PORTAL REACHED!");

            UIManager.Instance.ShowWinScreen();

            other.GetComponent<PlayerMotionController>().enabled = false;
        }
    }
}
