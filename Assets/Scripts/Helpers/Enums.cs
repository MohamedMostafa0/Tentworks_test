public enum GameState : byte
{
    MainMenu,
    InGame,
    Pause,
    GameOver
}

public enum IngredientType : byte
{
    Vegetable,
    Cheese,
    Meat
}

public enum IngredientState : byte
{
    Raw,
    Ready
}
public enum WorkerState : byte
{
    Empty,
    Processing,
    Done
}