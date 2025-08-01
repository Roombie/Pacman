using UnityEngine;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject readyText;
    [SerializeField] private TMP_Text gameOverText;
    [SerializeField] private TMP_Text highScoreText;

    [Header("Players")]
    [SerializeField] private TMP_Text[] scoreTexts;
    [SerializeField] private CanvasGroup[] playerTextGroups;

    private Coroutine[] flickerCoroutines;

    private void Awake()
    {
        flickerCoroutines = new Coroutine[playerTextGroups.Length];
    }

    public void InitializeUI(bool isMultiplayer)
    {
        SetScorePanelVisible(isMultiplayer);
        HidePlayerIntroText();
        ShowReadyText(false);
        ShowGameOverText(false);
    }

    public void ShowReadyText(bool visible) => readyText?.SetActive(visible);

    public void ShowGameOverText(bool visible) => gameOverText?.gameObject.SetActive(visible);

    public void SetScore(int playerIndex, int score)
    {
        if (IsValidPlayerIndex(playerIndex) && scoreTexts[playerIndex] != null)
            scoreTexts[playerIndex].text = score.ToString("D2");
    }

    public void UpdateScores(int[] scores)
    {
        for (int i = 0; i < scoreTexts.Length && i < scores.Length; i++)
        {
            if (scoreTexts[i] != null)
                scoreTexts[i].text = scores[i].ToString("D2");
        }
    }

    public void UpdateHighScore(int score)
    {
        if (highScoreText != null)
            highScoreText.text = score.ToString("D2");
    }

    public void UpdateIntroText(int currentPlayerIndex, bool isMultiplayer)
    {
        for (int i = 0; i < playerTextGroups.Length; i++)
        {
            if (playerTextGroups[i] != null)
                playerTextGroups[i].gameObject.SetActive(isMultiplayer && i == currentPlayerIndex);
        }
    }

    public void HidePlayerIntroText()
    {
        foreach (var group in playerTextGroups)
        {
            if (group != null)
                group.gameObject.SetActive(false);
        }
    }

    public void SetScorePanelVisible(bool isMultiplayer)
    {
        for (int i = 1; i < scoreTexts.Length; i++)
        {
            if (scoreTexts[i] != null)
                scoreTexts[i].gameObject.SetActive(isMultiplayer);
        }
    }

    public void StartPlayerFlicker(int playerIndex, float interval = 0.25f)
    {
        if (!IsValidPlayerIndex(playerIndex)) return;

        StopPlayerFlicker(playerIndex);
        flickerCoroutines[playerIndex] = StartCoroutine(FadeFlickerLoop(playerTextGroups[playerIndex], interval));
    }

    public void StopPlayerFlicker(int playerIndex)
    {
        if (!IsValidPlayerIndex(playerIndex)) return;

        if (flickerCoroutines[playerIndex] != null)
        {
            StopCoroutine(flickerCoroutines[playerIndex]);
            flickerCoroutines[playerIndex] = null;
        }

        // This is to make sure it always stays visible when you stop player's flicker effect 
        if (playerTextGroups[playerIndex] != null) 
            playerTextGroups[playerIndex].alpha = 1f;
    }

    private IEnumerator FadeFlickerLoop(CanvasGroup group, float interval)
    {
        if (group == null) yield break;

        bool fadingOut = false;

        while (true)
        {
            group.alpha = fadingOut ? 0f : 1f;
            fadingOut = !fadingOut;
            yield return new WaitForSeconds(interval);
        }
    }

    private bool IsValidPlayerIndex(int index)
    {
        return index >= 0 && index < playerTextGroups.Length;
    }
}