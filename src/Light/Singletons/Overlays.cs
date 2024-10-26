namespace Light;

using Godot;

/// <summary> Singleton responsible for managing post processing. </summary>
public partial class Overlays : Singleton<Overlays>
{
    /// <summary> Overlays that use shaders as materials. </summary>
    public ColorRect Vignette, Scale, Blood, VHS, CRT, Distortion;
    /// <summary> Parameters of the strength of shaders. </summary>
    public float VignetteValue, ScaleValue, BloodValue;

    /// <summary> It's strange that Godot does not accumulate this value. </summary>
    private float time;
    /// <summary> Whether the local player is alive. </summary>
    private bool alive => hp > 0;

    public override void _Ready()
    {
        Inst = this;

        Vignette = GetNode<ColorRect>("vignette/rect");
        Scale = GetNode<ColorRect>("grey-scale/rect");
        Blood = GetNode<ColorRect>("blood/rect");
        VHS = GetNode<ColorRect>("vhs/rect");
        CRT = GetNode<ColorRect>("crt/rect");
        Distortion = GetNode<ColorRect>("distortion/rect");

        VHS.Visible = CRT.Visible = Distortion.Visible = false;
    }

    public override void _Process(double delta)
    {
        time += (float)delta;

        void Move(ref float value, float speed, float target) => value = Mathf.Lerp(value, target, speed * (float)delta);

        // the local player is dying
        if (alive && hp <= 60)
        {
            float progress = 1f - hp / 60f;
            float psinwave = Mathf.Sin(time * progress * Mathf.Tau) + 1f;

            Move(ref VignetteValue, 4f, progress * (.75f + psinwave / 8f));
            Move(ref ScaleValue, 4f, progress * (.50f + psinwave / 4f));
            Move(ref BloodValue, 1f, progress);
        }
        // the local player is dead or has full hp
        else
        {
            Move(ref VignetteValue, 1f, alive ? 0f : 1f);
            Move(ref ScaleValue, 1f, alive ? 0f : 1f);
            Move(ref BloodValue, .5f, 0f);
        }

        void Set(ColorRect rect, float value) => (rect.Material as ShaderMaterial).SetShaderParameter("strength", value);

        Set(Vignette, 2f - VignetteValue * 2f);
        Set(Scale, ScaleValue);
        Set(Blood, BloodValue * 1.4f - .4f);
    }
}
