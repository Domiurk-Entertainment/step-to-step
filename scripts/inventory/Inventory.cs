using Godot;
using System.Collections.Generic;

namespace StepToStep.InventorySystem
{
    [GlobalClass]
    public partial class Inventory : Node, IInventory
    {
        public event InventoryDelegate AddedItem;
        public event InventoryDelegate RemovedItem;
        public event InventoryDelegate TakenItem;

        [Export] private Item _currentBall;

        private readonly Queue<Item> _balls = new();

        public void AddItem(Item item)
        {
            if(item.Amount <= 0){
                return;
            }

            AddedItem?.Invoke(item);

            if(_currentBall == null){
                _currentBall = item;
                TakenItem?.Invoke(_currentBall);
                return;
            }

            _balls.Enqueue(item);
        }

        public IReadOnlyCollection<Item> Balls => _balls;
        public IItem CurrentBall => _currentBall;

        public void AddItems(Item[] items)
        {
            foreach(Item item in items)
                AddItem(item);
        }

        public Item GetBall()
        {
            if(_balls.Count == 0 && _currentBall == null){
                return null;
            }

            Item result = _currentBall;

            if(result.Amount > 1){
                result.Amount--;
                TakenItem?.Invoke(result);
                return result;
            }

            RemovedItem?.Invoke(result);
            result.Amount--;

            if(!_balls.TryDequeue(out _currentBall)){
                RemovedItem?.Invoke(result);
                return result;
            }

            TakenItem?.Invoke(_currentBall);
            return result;
        }
    }

    public delegate void InventoryDelegate(Item item);
}