public class Fridge : BaseInteractable
{
    protected override InteractionUIData BuildUIData() =>
        new InteractionUIData
        {
            Title = "Fridge",
            ShowOkButton = !PlayerController.Instance.HasItem(),
            OkLabel = "Take ingredient",
            OnOk = OnOkAction
        };

    protected override void OnOkAction()
    {
        if (PlayerController.Instance.HasItem()) return;
        StartSlot();
    }

    protected override void FinishJob()
    {
        PlayerController.Instance.PickItem(new Ingredient(OrderManager.Instance.GetRandomIngredient()));
    }
}
