namespace Light.UI;

using Godot;

/// <summary> Animated panel containing an interface element. </summary>
public partial class HudPanel : Panel
{
    public override void _Process(double delta)
    {
        if (!Visible || Scale == Vector2.One) return;

        // increase the size in width, and then in height
        if (Scale.X <= .99f) Scale = Scale.MoveToward(new(1f, .1f), (float)delta * 20f);
        else
        if (Scale.Y <= .99f) Scale = Scale.MoveToward(Vector2.One, (float)delta * 20f);
        else
        {
            Scale = Vector2.One;
            Modulate = Colors.White;
        }

        // increase the transparency in parallel
        Modulate = Modulate with { A = Mathf.MoveToward(Modulate.A, 1f, (float)delta * 10f) };
    }

    /// <summary> Shows the animation of the panel appearance. </summary>
    public new void Show()
    {
        Visible = true;
        Scale = Vector2.Zero;
        Modulate = Colors.Transparent;
    }
}
