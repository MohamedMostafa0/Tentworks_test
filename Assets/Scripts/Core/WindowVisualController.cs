using System.Collections.Generic;
using UnityEngine;

public class WindowVisualController : MonoBehaviour
{
    [SerializeField] private Window window;

    [SerializeField] private Transform[] displayPoints;

    private readonly GameObject[] spawnedVisuals = new GameObject[3];
    private readonly List<IngredientType> lastDisplayed = new List<IngredientType>();

    private void OnDestroy()
    {
        for (int i = 0; i < spawnedVisuals.Length; i++)
        {
            if (spawnedVisuals[i] != null)
                Destroy(spawnedVisuals[i]);
        }
    }

    private void Update()
    {
        List<IngredientType> required = BuildRequiredList();
        if (!HasChanged(required)) return;

        RebuildVisuals(required);

        lastDisplayed.Clear();
        lastDisplayed.AddRange(required);
    }

    private List<IngredientType> BuildRequiredList()
    {
        var list = new List<IngredientType>();
        if (window.CurrentOrder == null) return list;

        foreach (var kvp in window.CurrentOrder.Required)
            for (int i = 0; i < kvp.Value; i++)
                list.Add(kvp.Key);

        return list;
    }

    private bool HasChanged(List<IngredientType> current)
    {
        if (current.Count != lastDisplayed.Count) return true;
        for (int i = 0; i < current.Count; i++)
            if (current[i] != lastDisplayed[i]) return true;
        return false;
    }

    private void RebuildVisuals(List<IngredientType> required)
    {
        ReturnAll();

        for (int i = 0; i < displayPoints.Length && i < required.Count; i++)
            spawnedVisuals[i] = IngredientVisualPool.Instance.Rent(
                required[i], IngredientState.Ready, displayPoints[i]);
    }

    private void ReturnAll()
    {
        for (int i = 0; i < spawnedVisuals.Length; i++)
        {
            if (spawnedVisuals[i] != null)
            {
                IngredientVisualPool.Instance.Return(spawnedVisuals[i]);
                spawnedVisuals[i] = null;
            }
        }
    }
}
