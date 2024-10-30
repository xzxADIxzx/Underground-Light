namespace Light.UI;

using Godot;
using Light.Net;
using Light.Singletons;
using Steamworks;
using Steamworks.Data;

/// <summary> Animated panel containing the join menu. </summary>
public partial class Join : HudPanel
{
    private LineEdit code;
    private Button join, refresh;
    private FlowContainer list;

    public override void _Ready()
    {
        code = GetNode<LineEdit>("code");
        join = GetNode<Button>("join");
        refresh = GetNode<Button>("refresh");
        list = GetNode<FlowContainer>("list/container");
    }

    /// <summary> Checks the validity of the lobby code in the line edit. </summary>
    public void VerifyLobbyCode() => join.Disabled = !ulong.TryParse(code.Text, out _);

    /// <summary> Joins the given lobby and then starts the game loop if succeed, otherwise shows an error popup. </summary>
    public void JoinLobby(Lobby? lobby) => LobbyController.JoinLobby(lobby ?? new(ulong.Parse(code.Text)), result =>
    {
        if (result == RoomEnter.Success)
            GameLoop.Inst.Start();
        else
        {
            Log.Warning($"Couldn't join the lobby, the result is {result}");
            // TODO notification
        }
    });

    /// <summary> Updates the list of public lobbies. </summary>
    public void RefreshPublicLobbies()
    {
        if (refresh.Disabled) return;

        refresh.Disabled = true;
        refresh.Text = "REFRESHING";

        list.GetChildren().Each(n => n.QueueFree());

        LobbyController.FetchLobbies(lobby =>
        {
            var btn = new Button
            {
                Text = $"{lobby.GetData("name")}\n{lobby.MemberCount}/{lobby.MaxMembers} I {lobby.GetData("level")}",
                ClipText = true,
                Alignment = HorizontalAlignment.Left,
                CustomMinimumSize = new(400f - 8f, 96f)
            };
            btn.AddThemeFontSizeOverride("font_size", 28);
            btn.Pressed += () => JoinLobby(lobby);

            list.AddChild(btn);

        }, () =>
        {
            refresh.Disabled = false;
            refresh.Text = "REFRESH";
        });
    }
}
