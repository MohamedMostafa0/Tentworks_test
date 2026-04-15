using System.Collections.Generic;
using UnityEngine;

public abstract class BaseInteractableWorker : BaseInteractable
{
    private readonly List<Ingredient> doneItems = new List<Ingredient>();

    public int DoneItemCount => doneItems.Count;
    public IReadOnlyList<Ingredient> DoneItems => doneItems;

    protected override bool CanStartSlot => ActiveSlots.Count + doneItems.Count < MaxSlots;

    protected abstract string WorkerName { get; }
    protected abstract bool CanAccept(Ingredient ingredient);

    protected override InteractionUIData BuildUIData()
    {
        if (!PlayerController.Instance.HasItem() && doneItems.Count > 0)
            return new InteractionUIData
            {
                Title = $"{WorkerName} — Ready!",
                ShowOkButton = true,
                OkLabel = $"Pick up {doneItems[0].Type}",
                OnOk = OnOkAction
            };

        if (PlayerController.Instance.HasItem())
        {
            Ingredient held = PlayerController.Instance.GetItem();
            if (CanAccept(held) && !held.IsReady && CanStartSlot)
                return new InteractionUIData
                {
                    Title = WorkerName,
                    ShowOkButton = true,
                    OkLabel = $"Place {held.Type} ({ActiveSlots.Count}/{ActiveSlots.Count + (CanStartSlot ? 1 : 0)})",
                    OnOk = OnOkAction
                };
        }

        return new InteractionUIData { Title = WorkerName, ShowOkButton = false };
    }

    protected override void OnOkAction()
    {
        if (!PlayerController.Instance.HasItem())
        {
            if (doneItems.Count == 0) return;
            PlayerController.Instance.PickItem(doneItems[0]);
            doneItems.RemoveAt(0);
            return;
        }

        Ingredient held = PlayerController.Instance.GetItem();
        if (!CanAccept(held) || held.IsReady || !CanStartSlot) return;

        PlayerController.Instance.ClearItem();
        StartSlot(held);
    }

    protected override void FinishJob()
    {
        if (currentSlot.Ingredient == null) return;
        currentSlot.Ingredient.SetReady();
        doneItems.Add(currentSlot.Ingredient);
    }
}
