using UnityEngine;

/// <summary>
/// Attach to the Player GameObject.
/// Rents the correct 3D prefab from the pool at holdPoint to show what the player carries.
/// </summary>
public class PlayerItemVisual : MonoBehaviour
{
    [SerializeField] private Transform holdPoint;

    private GameObject currentVisual;
    private Ingredient trackedItem;
    private IngredientState trackedState;

    private void Update()
    {
        Ingredient held = PlayerController.Instance.HasItem()
            ? PlayerController.Instance.GetItem()
            : null;

        IngredientState state = held?.State ?? IngredientState.Raw;

        if (held == trackedItem && state == trackedState) return;

        if (currentVisual != null)
        {
            IngredientVisualPool.Instance.Return(currentVisual);
            currentVisual = null;
        }

        if (held != null)
            currentVisual = IngredientVisualPool.Instance.Rent(held.Type, held.State, holdPoint);

        trackedItem = held;
        trackedState = state;
    }
}
