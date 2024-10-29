namespace Light.UI;

using Godot;
using Light.Net;
using Steamworks;
using Steamworks.Data;

/// <summary> Animated panel containing the join menu. </summary>
public partial class Join : HudPanel
{
    private LineEdit code;
    private Button join, refresh;

    public override void _Ready()
    {
        code = GetNode<LineEdit>("code");
        join = GetNode<Button>("join");
        refresh = GetNode<Button>("refresh");
    }

    /// <summary> Checks the validity of the lobby code in the line edit. </summary>
    public void VerifyLobbyCode() => join.Disabled = !ulong.TryParse(code.Text, out _);

    /// <summary> Joins the given lobby and then starts the game loop if succeed, otherwise shows an error popup. </summary>
    public void JoinLobby(Lobby? lobby) => LobbyController.JoinLobby(lobby ?? new(ulong.Parse(code.Text)), result =>
    {
        if (result == RoomEnter.Success) ;
        // GameLoop.Start();
        else
        {
            Log.Warning($"Couldn't join the lobby, the result is {result}");
            // TODO notification
        }
    });
}
