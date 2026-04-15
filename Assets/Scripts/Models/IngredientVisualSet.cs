using UnityEngine;

[CreateAssetMenu(menuName = "Game/Ingredient Visual Set")]
public class IngredientVisualSet : ScriptableObject
{
    [Header("Cheese (always ready — only one prefab needed)")]
    public GameObject Cheese;

    [Header("Meat")]
    public GameObject MeatRaw;
    public GameObject MeatCooked;

    [Header("Vegetable")]
    public GameObject VegetableRaw;
    public GameObject VegetableChopped;

    /// <summary>Returns the correct prefab for a given type and state.</summary>
    public GameObject Get(IngredientType type, IngredientState state)
    {
        return type switch
        {
            IngredientType.Cheese => Cheese,
            IngredientType.Meat => state == IngredientState.Ready ? MeatCooked : MeatRaw,
            IngredientType.Vegetable => state == IngredientState.Ready ? VegetableChopped : VegetableRaw,
            _ => null
        };
    }
}
