namespace Light;

using Godot;

/// <summary> Singleton responsible for managing user interface. </summary>
public partial class Menus : Singleton<Menus>
{
    /// <summary> Menu elements. </summary>
    public CanvasLayer MainMenu, Settings;

    public override void _Ready()
    {
        Inst = this;

        MainMenu = GetNode<CanvasLayer>("main-menu");
        Settings = GetNode<CanvasLayer>("settings");

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
