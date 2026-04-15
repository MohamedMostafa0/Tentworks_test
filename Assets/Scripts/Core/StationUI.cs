using TMPro;
using UnityEngine;

public class StationUI : MonoBehaviour
{
    [SerializeField] private BaseInteractableWorker worker;

    [Tooltip("One text element per slot (order matches slot index).")]
    [SerializeField] private TextMeshProUGUI[] slotTimerTexts;

    [SerializeField] private TextMeshProUGUI doneText;

    private void Update()
    {
        var slots = worker.ActiveSlots;

        for (int i = 0; i < slotTimerTexts.Length; i++)
        {
            if (i < slots.Count)
            {
                slotTimerTexts[i].gameObject.SetActive(true);
                slotTimerTexts[i].text = $"{slots[i].TimeRemaining:F1}s";
            }
            else
            {
                slotTimerTexts[i].gameObject.SetActive(false);
            }
        }

        if (doneText != null)
        {
            doneText.gameObject.SetActive(worker.DoneItemCount > 0);
            if (worker.DoneItemCount > 0)
                doneText.text = worker.DoneItemCount > 1
                    ? $"Ready x{worker.DoneItemCount}!"
                    : "Ready!";
        }
    }
}
