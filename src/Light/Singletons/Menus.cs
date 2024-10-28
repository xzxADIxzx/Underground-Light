namespace Light.Singletons;

using Godot;
using Light.UI;

/// <summary> Singleton responsible for managing user interface. </summary>
public partial class Menus : Singleton<Menus>
{
    /// <summary> All interface elements. There are few of them, so as not to interfere with immersion. </summary>
    public HudPanel MainMenu, Host, Join, Settings;
    /// <summary> Exit button contains a little detail. </summary>
    public Button ExitButton;

    public override void _Ready()
    {
        Inst = this;

        MainMenu = GetNode<HudPanel>("viewport/main-menu");
        Host = GetNode<HudPanel>("viewport/host");
        Join = GetNode<HudPanel>("viewport/join");
        Settings = GetNode<HudPanel>("viewport/settings");
        ExitButton = MainMenu.GetNode<Button>("exit");

        ShowMain();
    }

    public void ShowMain() { Hide(); MainMenu.Show(); }

    public void ShowHost() { Hide(); Host.Show(); }

    public void ShowJoin() { Hide(); Join.Show(); }

    public void ShowSettings() { Hide(); Settings.Show(); }

    /// <summary> Hides all menus. </summary>
    public void Hide() => MainMenu.Visible = Host.Visible = Join.Visible = Settings.Visible = false;

    /// <summary> Closes the game application. </summary>
    public void Exit() => GetTree().Quit();

    /// <summary> Changes the text of the exit button, making it creepy. </summary>
    public void MakeExitCreepy() => ExitButton.Text = "THERE IS NO EXIT";

    /// <summary> Changes the text of the exit button, making it normal. </summary>
    public void MakeExitNormal() => ExitButton.Text = "EXIT";
}
