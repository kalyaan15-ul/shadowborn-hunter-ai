using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;

    public int coinCount = 0;
    public TextMeshProUGUI coinText;

    void Awake()
    {
        Instance = this;
    }

    public void AddCoin()
    {
        coinCount++;
        coinText.text = "Coins: " + coinCount;
    }
}
