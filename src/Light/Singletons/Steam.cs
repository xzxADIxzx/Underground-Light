namespace Light.Singletons;

using Godot;
using Steamworks;
using System;

/// <summary> Singleton responsible for the authentication in Steam and backend updating. </summary>
public partial class Steam : Singleton<Steam>
{
    /// <summary> User account identifier in Steam. </summary>
    public static uint AccId;
    /// <summary> Application identifier in Steam. </summary>
    [Export] public uint AppId;

    public override void _EnterTree()
    {
        try
        {
            // connect to the local client of Steam
            SteamClient.Init(AppId, false);

            // cache the id, because SteamAPI_ISteamUser_GetSteamID is incredibly slow
            AccId = SteamClient.SteamId.AccountId;
        }
        catch (Exception ex)
        {
            Log.Error($"Couldn't initialize Steam: {ex}");
        }
    }

    public override void _ExitTree() => SteamClient.Shutdown();

    public override void _Ready() => Inst = this;

    public override void _PhysicsProcess(double delta) => SteamClient.RunCallbacks();
}
