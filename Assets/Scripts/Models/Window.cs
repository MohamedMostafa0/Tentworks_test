using System;
using System.Collections;
using UnityEngine;

public class Window : BaseInteractable
{
    [SerializeField] private float respawnDelay = 5f;

    public Order CurrentOrder { get; private set; }
    public event Action<int> OnOrderCompleted;

    protected override InteractionUIData BuildUIData()
    {
        if (CurrentOrder == null)
            return new InteractionUIData { Title = "Window No order", ShowOkButton = false };

        bool canDeliver = PlayerController.Instance.HasItem()
                          && CurrentOrder.CanDeliver(PlayerController.Instance.GetItem());

        return new InteractionUIData
        {
            Title = "Window",
            ShowOkButton = canDeliver,
            OkLabel = canDeliver ? $"Deliver {PlayerController.Instance.GetItem().Type}" : "Waiting for ingredient",
            OnOk = OnOkAction
        };
    }

    protected override void OnOkAction()
    {
        if (CurrentOrder == null || !PlayerController.Instance.HasItem()) return;

        Ingredient item = PlayerController.Instance.GetItem();
        if (!CurrentOrder.CanDeliver(item)) return;

        StartSlot(item);
    }

    protected override void FinishJob()
    {
        CurrentOrder.TryDeliver(currentSlot.Ingredient);
        PlayerController.Instance.ClearItem();

        if (CurrentOrder.IsComplete())
            CompleteOrder();
    }

    public void SpawnOrder()
    {
        CurrentOrder = OrderManager.Instance.CreateOrder();
    }

    private void CompleteOrder()
    {
        int score = GameManager.Instance.AddScore(CurrentOrder);
        OnOrderCompleted?.Invoke(score);
        CurrentOrder = null;
        StartCoroutine(RespawnOrderCor());
    }

    private IEnumerator RespawnOrderCor()
    {
        yield return new WaitForSeconds(respawnDelay);
        SpawnOrder();
    }
}
