using System.Collections.Generic;
using UnityEngine;

public class IngredientVisualPool : BaseSingleton<IngredientVisualPool>
{
    [SerializeField] private IngredientVisualSet visualSet;
    [SerializeField] private int initialPoolSizePerPrefab = 3;

    private readonly Dictionary<GameObject, Queue<GameObject>> pools
        = new Dictionary<GameObject, Queue<GameObject>>();

    private readonly Dictionary<GameObject, GameObject> instanceToPrefab
        = new Dictionary<GameObject, GameObject>();

    private void Start()
    {
        PreWarm(visualSet.Cheese);
        PreWarm(visualSet.MeatRaw);
        PreWarm(visualSet.MeatCooked);
        PreWarm(visualSet.VegetableRaw);
        PreWarm(visualSet.VegetableChopped);
    }

    private void PreWarm(GameObject prefab)
    {
        if (prefab == null) return;

        var queue = GetOrCreateQueue(prefab);
        for (int i = 0; i < initialPoolSizePerPrefab; i++)
            queue.Enqueue(CreateInstance(prefab));
    }

    public GameObject Rent(IngredientType type, IngredientState state, Transform parent)
    {
        GameObject prefab = visualSet.Get(type, state);
        if (prefab == null) return null;

        var queue = GetOrCreateQueue(prefab);
        GameObject obj = queue.Count > 0 ? queue.Dequeue() : CreateInstance(prefab);

        obj.transform.SetParent(parent, false);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
        obj.SetActive(true);

        instanceToPrefab[obj] = prefab;
        return obj;
    }

    public void Return(GameObject instance)
    {
        if (instance == null) return;

        if (!instanceToPrefab.TryGetValue(instance, out GameObject prefab))
        {
            Destroy(instance);
            return;
        }

        instance.SetActive(false);
        instance.transform.SetParent(transform, false);
        instanceToPrefab.Remove(instance);
        GetOrCreateQueue(prefab).Enqueue(instance);
    }

    private Queue<GameObject> GetOrCreateQueue(GameObject prefab)
    {
        if (!pools.TryGetValue(prefab, out var queue))
        {
            queue = new Queue<GameObject>();
            pools[prefab] = queue;
        }
        return queue;
    }

    private GameObject CreateInstance(GameObject prefab)
    {
        GameObject obj = Instantiate(prefab, transform);
        obj.SetActive(false);
        return obj;
    }
}
