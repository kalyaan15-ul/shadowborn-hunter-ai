using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject winPanel;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI winText; 

    void Awake()
    {
        Instance = this;
        winPanel.SetActive(false);   // hide at start
    }

    public void ShowWinScreen()
    {
        winPanel.SetActive(true);
        finalScoreText.text = "Coins: " + CoinManager.Instance.coinCount;
        Time.timeScale = 0f;   // stop game
    }

    public void ShowLoseScreen()
    {
        winPanel.SetActive(true);
        winText.text = "You Lose!";   
        finalScoreText.text = "Coins: " + CoinManager.Instance.coinCount;
        Time.timeScale = 0f;         
    }

}
