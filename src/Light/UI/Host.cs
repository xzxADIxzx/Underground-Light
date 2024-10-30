namespace Light.UI;

using Godot;
using Light.Net;
using Light.Singletons;

/// <summary> Animated panel containing the host menu. </summary>
public partial class Host : HudPanel
{
    /// <summary> List of all lobby access levels. </summary>
    public readonly string[] Levels = { "PRIVATE", "FRIENDS ONLY", "PUBLIC" };

    private LineEdit name;
    private Button accessibility;

    public override void _Ready()
    {
        name = GetNode<LineEdit>("name");
        accessibility = GetNode<Button>("accessibility");
    }

    /// <summary> Changes the accessibility of the lobby. </summary>
    public void ChangeAccessibility() => accessibility.Text = Levels[(Levels.IndexOf(accessibility.Text) + 1) % 3];

    /// <summary> Hosts a lobby, applies the settings and then starts the game loop. </summary>
    public void HostLobby() => LobbyController.CreateLobby(lobby =>
    {
        if (!lobby.Id.IsValid) Log.Warning("The lobby is invalid");

        lobby.SetJoinable(true);
        lobby.SetData("light", "true");
        lobby.SetData("name", name.Text == string.Empty ? "DESCENDING TO THE ABYSS" : name.Text);
        lobby.SetData("level", "Elevator");

        switch (Levels.IndexOf(accessibility.Text))
        {
            case 0: lobby.SetPrivate(); break;
            case 1: lobby.SetFriendsOnly(); break;
            case 2: lobby.SetPublic(); break;
        }

        GameLoop.Inst.Start();
    });
}
