namespace Light;

using Godot;
using Steamworks;
using Steamworks.Data;
using System;
using System.Collections.Generic;

/// <summary> List of events used by the game. </summary>
public partial class Events : Node
{
    #region events

    /// <summary> Event triggered when a loading of any scene has started. </summary>
    public static SafeEvent OnLoadingStarted = new();
    /// <summary> Event triggered after loading any scene. </summary>
    public static SafeEvent OnLoaded = new();
    /// <summary> Event triggered after loading the main menu. </summary>
    public static SafeEvent OnMainMenuLoaded = new();

    /// <summary> Event triggered when an action is taken on the lobby: creation, closing, connection or modifying. </summary>
    public static SafeEvent OnLobbyAction = new();
    /// <summary> Event triggered when the local player enters a lobby. </summary>
    public static SafeEvent OnLobbyEntered = new();

    /// <summary> Event triggered when someone invites you to their lobby. </summary>
    public static SafeEvent<Lobby> OnLobbyInvite = new();
    /// <summary> Event triggered when someone joins the lobby. </summary>
    public static SafeEvent<Friend> OnMemberJoin = new();
    /// <summary> Event triggered when someone leaves the lobby. </summary>
    public static SafeEvent<Friend> OnMemberLeave = new();

    #endregion

    /// <summary> List of tasks that will need to be completed on the next frame. </summary>
    public static Queue<Call> Tasks = new();

    public override void _Ready()
    {
        SteamMatchmaking.OnLobbyDataChanged += lobby => Post(OnLobbyAction.Fire);
        SteamMatchmaking.OnLobbyEntered += lobby => Post(OnLobbyEntered.Fire);

        SteamFriends.OnGameLobbyJoinRequested += (lobby, id) => OnLobbyInvite.Fire(lobby);

        SteamMatchmaking.OnLobbyMemberJoined += (lobby, member) => OnMemberJoin.Fire(member);
        SteamMatchmaking.OnLobbyMemberLeave += (lobby, member) => OnMemberLeave.Fire(member);
    }

    public override void _Process(double delta)
    {
        int amount = Tasks.Count;
        for (int i = 0; i < amount; i++) Tasks.Dequeue().Invoke();
    }

    /// <summary> Posts the task for execution in the next frame. </summary>
    public static void Post(Call task) => Tasks.Enqueue(task);

    /// <summary> Safe event that will output all exceptions to the console and guarantee the execution of each listener, regardless of errors. </summary>
    public class SafeEvent<T>
    {
        /// <summary> List of all event listeners. </summary>
        private List<Cons<T>> listeners = new();

        /// <summary> Fires the event, ensuring that all listeners will be executed regardless of exceptions. </summary>
        public void Fire(T t)
        {
            for (int i = 0; i < listeners.Count; i++)
            {
                try { listeners[i](t); }
                catch (Exception ex) { Log.Error(ex.ToString()); }
            }
        }

        /// <summary> Fires the event without arguments, ensuring that all listeners will be executed regardless of exceptions. </summary>
        public void Fire() => Fire(default);

        public static SafeEvent<T> operator +(SafeEvent<T> e, Cons<T> listener) { e.listeners.Add(listener); return e; }
        public static SafeEvent<T> operator -(SafeEvent<T> e, Cons<T> listener) { e.listeners.Remove(listener); return e; }
    }

    /// <summary> Safe event that will output all exceptions to the console and guarantee the execution of each listener, regardless of errors. </summary>
    public class SafeEvent : SafeEvent<object>
    {
        public static SafeEvent operator +(SafeEvent e, Call listener) { _ = e + (_ => listener()); return e; }
        public static SafeEvent operator -(SafeEvent e, Call listener) { _ = e - (_ => listener()); return e; }
    }
}
