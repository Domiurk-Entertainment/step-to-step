using Godot;

namespace StepToStep.Sound;

public partial class SoundService : Node
{
    [Export] private AudioStreamPlayer2D _hoverSound;
    [Export] private AudioStreamPlayer2D _clickSound;

    public void PlayHovered() => Play(_hoverSound);
    public void PlayClicked() => Play(_clickSound);

    private static void Play(AudioStreamPlayer2D sound)
    {
        if(sound.Playing)
            sound.Stop();
        sound.Play();
    }
}