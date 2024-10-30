namespace Light.Singletons;

using Godot;
using Light.Net;

/// <summary> Singleton responsible for the general game process. </summary>
public partial class GameLoop : Singleton<GameLoop>
{
    public override void _Ready()
    {
        Inst = this;

        LobbyController.Load();
        // Networking.Load();
    }

    /// <summary> Starts the gameplay loop, loads the elevator and local player. </summary>
    public void Start()
    {
        // TODO
    }
}
