public class Stove : BaseInteractableWorker
{
    protected override string WorkerName => "Stove";
    protected override bool CanAccept(Ingredient ingredient) => ingredient.Type == IngredientType.Meat;
}
