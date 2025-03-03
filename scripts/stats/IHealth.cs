using Godot;

namespace StepToStep.Utils;

public interface IHealth
{
    void TakeDamage(Node sender, float damage);
}