namespace Light.Singletons;

using Godot;
using Light.IO;
using Light.Net;

/// <summary> Singleton responsible for the general game process. </summary>
public partial class GameLoop : Singleton<GameLoop>
{
    /// <summary> Current version of the game build. </summary>
    public static string Version => ProjectSettings.GetSetting("application/config/version").ToString();

    public override void _EnterTree() => Log.Debug($"The version of the game is {Version}");

    public override void _Ready()
    {
        Inst = this;

        Pointers.Allocate();
        LobbyController.Load();
        // Networking.Load();
    }

    /// <summary> Starts the gameplay loop, loads the elevator and local player. </summary>
    public void Start()
    {
        // TODO
    }
}
