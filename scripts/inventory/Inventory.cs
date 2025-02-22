using Godot;
using Godot.Collections;
using StepToStep.scripts;
using System;
using System.Linq;

namespace StepToStep.Inventory
{
    public partial class Inventory : ScrollContainer
    {
        [Export] private VBoxContainer _vBoxContainer;
        [Export] private PackedScene _itemTemplateScene;
        [Export] private Array<BallResource> _itemsTemplate;
        [Export] private int capacity = 5;
        [Export] private Vector2I amountRange = new Vector2I(1, 5);
        [Export] private Player _player;
        private Array<Item> _items;

        public override void _Ready()
        {
            _items = new Array<Item>();
            _player.ChangeStep += PlayerOnChangedStep;

            for(int i = 0; i < capacity; i++){
                CreateRandomItemInInventory();
            }

            _player.BallScene = _items[0].Resource.PackedScene;
            _player.CanShoot = _items[0].Amount > 0;
        }

        private void PlayerOnChangedStep(StepType stepType)
        {
            switch(stepType){
                case StepType.Start:
                    break;
                case StepType.Attacked:
                    Item item = _items[0];
                    item.Amount--;
                    GetFirstItem().UpdateItem();

                    if(item.Amount == 0){
                        DeleteFirstItem();
                        CreateRandomItemInInventory();
                        _player.CanShoot = _items.Count != 0;
                    }

                    break;
                case StepType.End:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(stepType), stepType, null);
            }
        }

        public void CreateRandomItemInInventory()
        {
            Node instance = _itemTemplateScene.Instantiate();
            BallResource itemResource = _itemsTemplate[GD.RandRange(0, _itemsTemplate.Count - 1)];
            Item item = new Item(itemResource, GD.RandRange(amountRange.X, amountRange.Y));
            _items.Add(item);

            if(instance is not ItemInInventory itemInInventory){
                GD.PrintErr($"{Name}: {instance.Name} is not required type scene");
                return;
            }

            itemInInventory.SetItem(item);
            _vBoxContainer.AddChild(itemInInventory);
        }

        private ItemInInventory GetFirstItem()
        {
            return _vBoxContainer.GetChildren()[0] as ItemInInventory;
        }

        private void DeleteFirstItem()
        {
            ItemInInventory firstItemInventory = GetFirstItem();
            _items.RemoveAt(0);
            firstItemInventory.QueueFree();
        }

        private void OnItemSelected(long i)
        {
            int index = (int)i;
            Item item = _items[index];
            if(item.Resource is not BallResource ballResource)
                return;
            _player.BallScene = ballResource.PackedScene;
            _player.CanShoot = item.Amount > 0;
        }
    }
}