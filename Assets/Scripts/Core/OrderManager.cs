using UnityEngine;

public class OrderManager : BaseSingleton<OrderManager>
{
    public Order CreateOrder()
    {
        Order order = new Order
        {
            StartTime = Time.time
        };

        int count = Random.value < 0.5f ? 2 : 3;

        for (int i = 0; i < count; i++)
        {
            var type = GetRandomIngredient();

            if (!order.Required.ContainsKey(type))
                order.Required[type] = 0;

            order.Required[type]++;
        }

        order.FinalizeRequirements();
        return order;
    }

    public IngredientType GetRandomIngredient()
    {
        int r = Random.Range(0, 3);
        return (IngredientType)r;
    }
}
