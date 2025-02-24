using Godot;
using System;
using System.Collections.Generic;

namespace StepToStep.InventorySpace
{
    public partial class Inventory : Node, IInventory
    {
        public event InventoryDelegate AddedItem;
        public event InventoryDelegate RemovedItem;
        public event InventoryDelegate TakenItem;

        private Item _currentBall;

        private Queue<Item> _balls = new();
        private List<Item> _items = new();

        public void AddItem(Item item)
        {
            _balls.Enqueue(item);
            AddedItem?.Invoke(item);
        }

        public void AddItems(Item[] items)
        {
            foreach(Item item in items)
                AddItem(item);
        }

        public Item GetBall()
        {
            if(_currentBall == null || _currentBall.Amount == 0){
                RemovedItem?.Invoke(_currentBall);
                _currentBall = _balls.Dequeue();
                TakenItem?.Invoke(_currentBall);
            }
            else{
                TakenItem?.Invoke(_currentBall);
            }

            _currentBall.Amount--;
            return _currentBall;
        }
    }

    public delegate void InventoryDelegate(Item item);

    public interface IInventory
    {
        event InventoryDelegate AddedItem;
        event InventoryDelegate RemovedItem;
        event InventoryDelegate TakenItem;

        public void AddItems(Item[] items);
        public void AddItem(Item item);
        public Item GetBall();
    }
}