using Godot;
using StepToStep.InventorySystem;
using StepToStep.Systems;

namespace StepToStep.Utils;

public partial class Currency : HSplitContainer
{
	public static Currency Instance;

	[Export] private ItemResource _resource;
	[field: Export] public int Coins { get; private set; } = 0;

	[Export] private SectionType _sectionType = SectionType.Player;

	private TextureRect _textureRect;
	private Label _label;

	public override void _Ready()
	{
		if(Instance != null && Instance != this){
			GD.Print("It duplicate.");
			QueueFree();
		}
		else{
			Instance = this;
		}

		_label = this.FindNode<Label>();
		_textureRect = this.FindNode<TextureRect>();
	}

	public void Update()
	{
		_label.Text = Coins.ToString();
		_textureRect.Texture = _resource.Icon;
	}

	public override void _EnterTree()
	{
		Hide();
		// Coins = SaveSystem.Instance.Get(_sectionType, GetKey("Coins"), 0).AsInt32();
	}

	public override void _ExitTree()
	{
		Save();
	}

	public void Save()
	{
		SaveSystem.Instance.Set(_sectionType, this.GetSaveKey(), Coins);
	}
}
