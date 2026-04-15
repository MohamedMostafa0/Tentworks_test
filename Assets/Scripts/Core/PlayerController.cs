using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : BaseSingleton<PlayerController>
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 15f;

    private CharacterController characterController;
    private Ingredient heldItem;

    protected void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (!GameManager.Instance.IsRunning) return;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(h, 0f, v).normalized * moveSpeed;
        characterController.SimpleMove(move);

        if (move != Vector3.zero)
        {
            Quaternion target = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, target, rotationSpeed * Time.deltaTime);
        }
    }

    public bool HasItem() => heldItem != null;
    public Ingredient GetItem() => heldItem;
    public void PickItem(Ingredient i) => heldItem = i;
    public void ClearItem() => heldItem = null;
}
