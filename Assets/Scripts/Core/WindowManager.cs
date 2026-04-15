using UnityEngine;
public class WindowManager : BaseSingleton<WindowManager>
{
    [SerializeField] private Window[] windows = new Window[4];

    public Window[] Windows => windows;

    public void SpawnAllOrders()
    {
        foreach (Window window in windows)
            window.SpawnOrder();
    }
}
