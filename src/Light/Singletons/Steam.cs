namespace Light.Singletons;

using Godot;
using Steamworks;

/// <summary> Singleton responsible for the authentication in Steam and backend updating. </summary>
public partial class Steam : Singleton<Steam>
{
	/// <summary> Application identifier in Steam. </summary>
	[Export] public uint AppID;

	public override void _EnterTree() => SteamClient.Init(AppID, false);

	public override void _ExitTree() => SteamClient.Shutdown();

	public override void _Ready() => Inst = this;

	public override void _PhysicsProcess(double delta) => SteamClient.RunCallbacks();
}
