public class Table : BaseInteractableWorker
{
    protected override string WorkerName => "Table";
    protected override bool CanAccept(Ingredient ingredient) => ingredient.Type == IngredientType.Vegetable;
}
