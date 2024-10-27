namespace Light.Singletons;

using Godot;

/// <summary> Singletons perform a variety of tasks and, most often, are initialized immediately after starting the game in the root scene. </summary>
public partial class Singleton<T> : Node
{
    static T instance;

    /// <summary> Singleton instance available everywhere. </summary>
    public static T Inst
    {
        get => instance;
        set
        {
            instance = value;
            GD.Print($"Initializing {typeof(T).Name} singleton...");
        }
    }
}
