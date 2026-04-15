public class Ingredient
{
    public IngredientType Type { get; private set; }
    public IngredientState State { get; private set; }
    public bool IsReady => State == IngredientState.Ready;

    public Ingredient(IngredientType type)
    {
        Type = type;
        State = type switch
        {
            IngredientType.Cheese => IngredientState.Ready,
            _ => IngredientState.Raw,
        };
    }

    public void SetReady()
    {
        State = IngredientState.Ready;
    }
}
