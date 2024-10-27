namespace Light.Net;

using Godot;
using Steamworks;
using Steamworks.Data;

/// <summary> Static class responsible for managing the lobby. </summary>
public static class LobbyController
{
    /// <summary> The current lobby the local player is connected to. </summary>
    public static Lobby? Lobby;
    public static bool Online => Lobby != null;
    public static bool Offline => Lobby == null;

    /// <summary> Id of the last lobby owner. </summary>
    public static SteamId LastOwner;
    /// <summary> Whether the local player owns the lobby. </summary>
    public static bool IsOwner;

    #region control

    /// <summary> Creates a new lobby and connects to it. </summary>
    public static void CreateLobby(Cons<Lobby> cons)
    {
        if (Online) return;
        GD.Print("Creating a lobby...");

        SteamMatchmaking.CreateLobbyAsync(4).ContinueWith(task =>
        {
            IsOwner = true;
            Lobby = task.Result;

            Lobby?.SetJoinable(true);
            Lobby?.SetData("light", "true");

            cons(Lobby.Value);
        });
    }

    /// <summary> Leaves the lobby. If the player is the owner, then all other players will be thrown to the main menu. </summary>
    public static void LeaveLobby()
    {
        if (Offline) return;
        GD.Print("Leaving the lobby...");

        Lobby?.Leave();
        Lobby = null;
    }

    /// <summary> Opens the overlay with an invitation dialog. </summary>
    public static void InviteFriend() => SteamFriends.OpenGameInviteOverlay(Lobby.Value.Id);

    /// <summary> Connects the local player to the given lobby. </summary>
    public static void JoinLobby(Lobby lobby, Cons<RoomEnter> cons)
    {
        if (Lobby?.Id == lobby.Id) return;
        GD.Print("Joining a lobby...");

        // leave the previous lobby before joining the new one
        if (Online) LeaveLobby();

        lobby.Join().ContinueWith(task =>
        {
            if (task.Result == RoomEnter.Success)
            {
                IsOwner = false;
                Lobby = lobby;
            }
            cons(task.Result);
        });
    }

    #endregion
    #region lobby code & browser

    /// <summary> Copies the lobby code to the clipboard. </summary>
    public static void CopyCode()
    {
        DisplayServer.ClipboardSet(Lobby?.Id.ToString() ?? "How did you do this?");
    }

    /// <summary> Fetches the list of public lobbies. </summary>
    public static void FetchLobbies(Cons<Lobby> cons)
    {
        SteamMatchmaking.LobbyList.RequestAsync().ContinueWith(task => task.Result.Each(l => l.Data.Any(p => p.Key == "light"), cons));
    }

    #endregion
}
