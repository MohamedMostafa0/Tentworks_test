using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;

public class WindowUI : MonoBehaviour
{
    [SerializeField] private Window window;

    [SerializeField] private TextMeshProUGUI orderText;
    [SerializeField] private TextMeshProUGUI elapsedTimerText;
    [SerializeField] private TextMeshProUGUI scorePopupText;

    [SerializeField] private float popupDuration = 2f;

    private void Awake()
    {
        window.OnOrderCompleted += ShowScorePopup;
        if (scorePopupText != null)
            scorePopupText.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        window.OnOrderCompleted -= ShowScorePopup;
    }

    private void Update()
    {
        Order order = window.CurrentOrder;

        if (order == null)
        {
            if (orderText != null) orderText.text = "No order";
            if (elapsedTimerText != null) elapsedTimerText.text = "";
            return;
        }

        if (orderText != null)
            orderText.text = BuildOrderText(order);

        if (elapsedTimerText != null)
            elapsedTimerText.text = $"{Mathf.FloorToInt(Time.time - order.StartTime)}s";
    }

    private static string BuildOrderText(Order order)
    {
        StringBuilder sb = new StringBuilder();
        foreach (var kvp in order.Required)
        {
            if (kvp.Value > 0)
                sb.AppendLine($"{kvp.Key} x{kvp.Value}");
        }
        return sb.Length > 0 ? sb.ToString().TrimEnd() : "Done!";
    }

    private void ShowScorePopup(int score)
    {
        if (scorePopupText == null) return;
        StopAllCoroutines();
        StartCoroutine(PopupCoroutine(score));
    }

    private IEnumerator PopupCoroutine(int score)
    {
        scorePopupText.text = score >= 0 ? $"+{score}" : $"{score}";
        scorePopupText.gameObject.SetActive(true);

        Color color = scorePopupText.color;
        float elapsed = 0f;

        while (elapsed < popupDuration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Lerp(1f, 0f, elapsed / popupDuration);
            scorePopupText.color = color;
            yield return null;
        }

        scorePopupText.gameObject.SetActive(false);
        color.a = 1f;
        scorePopupText.color = color;
    }
}
