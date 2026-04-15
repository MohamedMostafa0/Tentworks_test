using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseInteractable : MonoBehaviour
{
    [SerializeField] protected float jobTime = 2f;
    [SerializeField] private int maxSlots = 1;

    public class Slot
    {
        public Ingredient Ingredient;
        public float TimeRemaining;
    }

    private readonly List<Slot> activeSlots = new List<Slot>();

    public IReadOnlyList<Slot> ActiveSlots => activeSlots;

    protected Slot currentSlot;

    protected int MaxSlots => maxSlots;
    protected virtual bool CanStartSlot => activeSlots.Count < maxSlots;

    public float JobTime => jobTime;

    protected abstract InteractionUIData BuildUIData();

    protected abstract void FinishJob();

    protected virtual void OnOkAction() { }

    protected void StartSlot(Ingredient ingredient = null)
    {
        if (!CanStartSlot) return;

        Slot slot = new Slot { Ingredient = ingredient, TimeRemaining = jobTime };
        activeSlots.Add(slot);

        if (jobTime <= 0f)
        {
            activeSlots.Remove(slot);
            currentSlot = slot;
            FinishJob();
        }
        else
        {
            StartCoroutine(RunSlot(slot));
        }
    }

    private IEnumerator RunSlot(Slot slot)
    {
        float elapsed = 0f;
        while (elapsed < jobTime)
        {
            elapsed += Time.deltaTime;
            slot.TimeRemaining = Mathf.Max(0f, jobTime - elapsed);
            yield return null;
        }

        activeSlots.Remove(slot);
        currentSlot = slot;
        FinishJob();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UIManager.Instance.ShowInteractionUI(BuildUIData());
            print("Player entered interaction range");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UIManager.Instance.HideInteractionUI();
            print("Player exited interaction range");
        }
    }
}
