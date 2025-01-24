using Godot;

namespace StepToStep.Stats;

public partial class UIScripts : VBoxContainer
{
    [Export] private string template = "{0}:{1}";
    [Export] private Font font;
    [Export] private int size = 12;
    [Export] private StatHandler stats;

    public override void _Ready()
    {
        foreach(Stat stat in stats.All()){
            var label = new Label();
            label.LabelSettings = new LabelSettings();
            label.LabelSettings.Font = font;
            label.LabelSettings.FontSize = size;
            UpdateUI(label, stat);
            stat.UpdateStatValue += (n, v) => UpdateUI(label, stat);
            AddChild(label);
        }

        return;

        void UpdateUI(Label ui, Stat stat)
            => ui.Text = string.Format(template, stat.Name, stat.Value);
    }
}