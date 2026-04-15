public class Trash : BaseInteractable
{
    protected override InteractionUIData BuildUIData()
    {
        bool hasItem = PlayerController.Instance.HasItem();
        return new InteractionUIData
        {
            Title = "Trash",
            ShowOkButton = hasItem,
            OkLabel = hasItem ? $"Throw away {PlayerController.Instance.GetItem().Type}" : string.Empty,
            OnOk = OnOkAction
        };
    }

    protected override void OnOkAction()
    {
        if (!PlayerController.Instance.HasItem()) return;
        StartSlot(PlayerController.Instance.GetItem());
    }

    protected override void FinishJob()
    {
        PlayerController.Instance.ClearItem();
    }
}
