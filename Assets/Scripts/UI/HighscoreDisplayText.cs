using TMPro;
using UnityEngine;

public class HighscoreDisplayText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI highscoreText;

    private void OnEnable()
    {
        UpdateHighscoreText();
    }

    private void Start()
    {
        UpdateHighscoreText();
    }

    private void UpdateHighscoreText()
    {
        int highscore = PlayerPrefs.GetInt("Highscore", 0);

        if (highscore <= 0)
        {
            highscoreText.gameObject.SetActive(false);
        }
        else
        {
            highscoreText.text = highscore.ToString("N0");
            highscoreText.gameObject.SetActive(true);
        }
    }
}