using System.Collections.Generic;
using UnityEngine;

public class Order
{
    public Dictionary<IngredientType, int> Required = new Dictionary<IngredientType, int>();
    private Dictionary<IngredientType, int> originalRequired = new Dictionary<IngredientType, int>();

    public float StartTime;

    public void FinalizeRequirements()
    {
        originalRequired = new Dictionary<IngredientType, int>(Required);
    }

    public bool CanDeliver(Ingredient ingredient)
    {
        if (!ingredient.IsReady) return false;
        return Required.TryGetValue(ingredient.Type, out int count) && count > 0;
    }

    public bool TryDeliver(Ingredient ingredient)
    {
        if (!CanDeliver(ingredient)) return false;
        Required[ingredient.Type]--;
        return true;
    }

    public bool IsComplete()
    {
        foreach (var kvp in Required)
        {
            if (kvp.Value > 0) return false;
        }
        return true;
    }

    public int CalculateScore(float currentTime)
    {
        int baseScore = GetBaseScore();
        int timePenalty = Mathf.FloorToInt(currentTime - StartTime);
        return baseScore - timePenalty;
    }

    private int GetBaseScore()
    {
        int score = 0;
        foreach (var kvp in originalRequired)
        {
            int value = kvp.Key switch
            {
                IngredientType.Vegetable => 20,
                IngredientType.Cheese => 10,
                IngredientType.Meat => 30,
                _ => 0
            };
            score += value * kvp.Value;
        }
        return score;
    }
}
