using System;
using System.Collections.Generic;
using RPG.Saving;
using UnityEngine;

namespace RPG.Inventories
{
  public class ActionStore : MonoBehaviour, ISaveable
  {
    Dictionary<int, DockedItemSlot> _dockedItems = new();
    class DockedItemSlot
    {
      public ActionItem Item;
      public int Number;
    }

    public event Action StoreUpdated;

    public ActionItem GetAction(int index)
    {
      return _dockedItems.ContainsKey(index) ? _dockedItems[index].Item : null;
    }

    public int GetNumber(int index)
    {
      return _dockedItems.ContainsKey(index) ? _dockedItems[index].Number : 0;
    }

    public void AddAction(InventoryItem item, int index, int number)
    {
      if (_dockedItems.ContainsKey(index))
      {
        if (ReferenceEquals(item, _dockedItems[index].Item))
          _dockedItems[index].Number += number;
      }
      else
      {
        DockedItemSlot slot = new()
        {
          Item = item as ActionItem,
          Number = number
        };
        _dockedItems[index] = slot;
      }
      StoreUpdated?.Invoke();
    }

    public bool Use(int index, GameObject user)
    {
      if (!_dockedItems.ContainsKey(index)) return false;
      _dockedItems[index].Item.Use(user);
      if (_dockedItems[index].Item.IsConsumable)
        RemoveItems(index, 1);
      return true;

    }

    public void RemoveItems(int index, int number)
    {
      if (!_dockedItems.ContainsKey(index)) return;
      _dockedItems[index].Number -= number;
      if (_dockedItems[index].Number <= 0)
        _dockedItems.Remove(index);
      StoreUpdated?.Invoke();
    }

    public int MaxAcceptable(InventoryItem item, int index)
    {
      var actionItem = item as ActionItem;
      if (!actionItem) return 0;

      if (_dockedItems.ContainsKey(index) && !ReferenceEquals(item, _dockedItems[index].Item))
      {
        return 0;
      }
      if (actionItem.IsConsumable)
      {
        return int.MaxValue;
      }
      if (_dockedItems.ContainsKey(index))
      {
        return 0;
      }

      return 1;
    }

    [Serializable]
    struct DockedItemRecord
    {
      public string ItemID;
      public int Number;
    }

    object ISaveable.CaptureState()
    {
      Dictionary<int, DockedItemRecord> state = new();
      foreach (var pair in _dockedItems)
      {
        DockedItemRecord record = new()
        {
          ItemID = pair.Value.Item.ItemID,
          Number = pair.Value.Number
        };
        state[pair.Key] = record;
      }
      return state;
    }

    void ISaveable.RestoreState(object state)
    {
      var stateDict = (Dictionary<int, DockedItemRecord>)state;
      foreach (var pair in stateDict)
        AddAction(InventoryItem.GetFromID(pair.Value.ItemID), pair.Key, pair.Value.Number);
    }
  }
}