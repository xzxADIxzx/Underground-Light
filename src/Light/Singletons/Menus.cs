namespace Light;

using Godot;

/// <summary> Singleton responsible for managing user interface. </summary>
public partial class Menus : Singleton<Menus>
{
    /// <summary> All interface elements. There are few of them, so as not to interfere with immersion. </summary>
    public Panel MainMenu, Settings;

    public override void _Ready()
    {
        Inst = this;

        MainMenu = GetNode<Panel>("viewport/main-menu");
        Settings = GetNode<Panel>("viewport/settings");

        ShowMain();
    }

    public void ShowMain()
    {
        Hide();
        MainMenu.Visible = true;
    }

    public void ShowSettings()
    {
        Hide();
        Settings.Visible = true;
    }

    /// <summary> Hides all menus. </summary>
    public void Hide() => MainMenu.Visible = Settings.Visible = false;

    /// <summary> Closes the game application. </summary>
    public void Exit() => GetTree().Quit();
}
