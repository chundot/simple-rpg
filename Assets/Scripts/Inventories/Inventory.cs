using System;
using UnityEngine;
using RPG.Saving;
using RPG.Core;

namespace RPG.Inventories
{
  public class Inventory : MonoBehaviour, ISaveable, IPredicateEvaluator
  {
    [Tooltip("背包大小")]
    [SerializeField] int _inventorySize = 16;

    InventorySlot[] _slots;

    public struct InventorySlot
    {
      public InventoryItem Item;
      public int Number;
    }

    public event Action InventoryUpdated;

    public int Size => _slots.Length;

    public static Inventory PlayerInventory => GameObject.FindWithTag("Player").GetComponent<Inventory>();

    public bool HasSpaceFor(InventoryItem item)
    {
      return FindSlot(item) >= 0;
    }

    public bool AddToFirstEmptySlot(InventoryItem item, int number)
    {
      int i = FindSlot(item);
      if (i < 0)
        return false;

      _slots[i].Item = item;
      _slots[i].Number += number;
      InventoryUpdated?.Invoke();
      return true;
    }

    public bool HasItem(InventoryItem item)
    {
      for (int i = 0; i < _slots.Length; i++)
      {
        if (ReferenceEquals(_slots[i].Item, item))
          return true;
      }
      return false;
    }

    public bool HasItem(string itemName)
    {
      foreach (var slot in _slots)
        if (slot.Item && slot.Item.DisplayName == itemName)
          return true;
      return false;
    }

    public InventoryItem GetItemInSlot(int slot)
    {
      return _slots[slot].Item;
    }

    public int GetNumberInSlot(int slot)
    {
      return _slots[slot].Number;
    }

    public void RemoveFromSlot(int slot, int number)
    {
      _slots[slot].Number -= number;
      if (_slots[slot].Number <= 0)
      {
        _slots[slot].Number = 0;
        _slots[slot].Item = null;
      }
      InventoryUpdated?.Invoke();
    }

    public bool AddItemToSlot(int slot, InventoryItem item, int number)
    {
      if (_slots[slot].Item != null)
        return AddToFirstEmptySlot(item, number); ;

      var i = FindStack(item);
      if (i >= 0)
        slot = i;

      _slots[slot].Item = item;
      _slots[slot].Number += number;
      InventoryUpdated?.Invoke();
      return true;
    }

    private void Awake()
    {
      _slots = new InventorySlot[_inventorySize];
    }

    private int FindSlot(InventoryItem item)
    {
      int i = FindStack(item);
      if (i < 0)
        i = FindEmptySlot();
      return i;
    }

    int FindEmptySlot()
    {
      for (int i = 0; i < _slots.Length; i++)
      {
        if (!_slots[i].Item)
          return i;
      }
      return -1;
    }

    private int FindStack(InventoryItem item)
    {
      if (!item.IsStackable)
        return -1;

      for (int i = 0; i < _slots.Length; i++)
        if (ReferenceEquals(_slots[i].Item, item))
          return i;
      return -1;
    }

    [Serializable]
    struct InventorySlotRecord
    {
      public string ItemID;
      public int Number;
    }

    object ISaveable.CaptureState()
    {
      var slotStrings = new InventorySlotRecord[_inventorySize];
      for (int i = 0; i < _inventorySize; i++)
      {
        if (_slots[i].Item != null)
        {
          slotStrings[i].ItemID = _slots[i].Item.ItemID;
          slotStrings[i].Number = _slots[i].Number;
        }
      }
      return slotStrings;
    }

    void ISaveable.RestoreState(object state)
    {
      var slotStrings = state as InventorySlotRecord[];
      for (int i = 0; i < _inventorySize; i++)
      {
        _slots[i].Item = InventoryItem.GetFromID(slotStrings[i].ItemID);
        _slots[i].Number = slotStrings[i].Number;
      }
      InventoryUpdated?.Invoke();
    }

    public bool? Evaluate(string predicate, string[] parameters)
    {
      if (predicate == "HasItem")
        return HasItem(parameters[0]);
      return null;
    }
  }
}