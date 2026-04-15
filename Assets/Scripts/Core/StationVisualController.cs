using System;
using UnityEngine;
using UnityEngine.UI;

public class StationVisualController : MonoBehaviour
{
    [Serializable]
    public class SlotDisplay
    {
        public Transform itemPoint;
        public Image progressBar;
    }

    [SerializeField] private BaseInteractableWorker worker;
    [SerializeField] private SlotDisplay[] slots;

    private GameObject[] spawnedVisuals;
    private IngredientType?[] trackedTypes;
    private IngredientState?[] trackedStates;

    private void Awake()
    {
        spawnedVisuals = new GameObject[slots.Length];
        trackedTypes = new IngredientType?[slots.Length];
        trackedStates = new IngredientState?[slots.Length];
    }

    private void Update()
    {
        var active = worker.ActiveSlots;
        var done = worker.DoneItems;

        for (int i = 0; i < slots.Length; i++)
        {
            bool hasActive = i < active.Count;
            bool hasDone = !hasActive && (i - active.Count) < done.Count;

            if (hasActive)
            {
                var slot = active[i];
                ShowVisual(i, slot.Ingredient.Type, IngredientState.Raw, slots[i].itemPoint);

                if (slots[i].progressBar != null)
                {
                    slots[i].progressBar.gameObject.SetActive(true);
                    float fill = worker.JobTime > 0f
                        ? 1f - slot.TimeRemaining / worker.JobTime
                        : 1f;
                    slots[i].progressBar.fillAmount = Mathf.Clamp01(fill);
                }
            }
            else if (hasDone)
            {
                int doneIndex = i - active.Count;
                ShowVisual(i, done[doneIndex].Type, IngredientState.Ready, slots[i].itemPoint);

                if (slots[i].progressBar != null)
                    slots[i].progressBar.gameObject.SetActive(false);
            }
            else
            {
                ClearVisual(i);

                if (slots[i].progressBar != null)
                    slots[i].progressBar.gameObject.SetActive(false);
            }
        }
    }

    private void ShowVisual(int index, IngredientType type, IngredientState state, Transform parent)
    {
        if (trackedTypes[index] == type && trackedStates[index] == state) return;

        ClearVisual(index);
        spawnedVisuals[index] = IngredientVisualPool.Instance.Rent(type, state, parent);
        trackedTypes[index] = type;
        trackedStates[index] = state;
    }

    private void ClearVisual(int index)
    {
        if (spawnedVisuals[index] != null)
        {
            IngredientVisualPool.Instance.Return(spawnedVisuals[index]);
            spawnedVisuals[index] = null;
        }
        trackedTypes[index] = null;
        trackedStates[index] = null;
    }
}
