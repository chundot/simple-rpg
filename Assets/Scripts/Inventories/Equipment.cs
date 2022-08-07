using System;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;

namespace RPG.Inventories
{
  public class Equipment : MonoBehaviour, ISaveable
  {
    Dictionary<EquipLocation, EquipableItem> _equippedItems = new();
    public event Action EquipmentUpdated;
    public IEnumerable<EquipLocation> AllPopulatedSlots => _equippedItems.Keys;
    public EquipableItem GetItemInSlot(EquipLocation equipLocation)
    {
      return !_equippedItems.ContainsKey(equipLocation) ? null : _equippedItems[equipLocation];
    }
    public void AddItem(EquipLocation slot, EquipableItem item)
    {
      Debug.Assert(item.AllowedEquipLocation == slot);

      _equippedItems[slot] = item;

      EquipmentUpdated?.Invoke();
    }
    public void RemoveItem(EquipLocation slot)
    {
      _equippedItems.Remove(slot);
      EquipmentUpdated?.Invoke();
    }

    object ISaveable.CaptureState()
    {
      Dictionary<EquipLocation, string> equippedItemsForSerialization = new();
      foreach (var pair in _equippedItems)
        equippedItemsForSerialization[pair.Key] = pair.Value.ItemID;
      return equippedItemsForSerialization;
    }

    void ISaveable.RestoreState(object state)
    {
      _equippedItems = new Dictionary<EquipLocation, EquipableItem>();

      var equippedItemsForSerialization = state as Dictionary<EquipLocation, string>;

      foreach (var pair in equippedItemsForSerialization)
      {
        var item = InventoryItem.GetFromID(pair.Value) as EquipableItem;
        if (item != null)
          _equippedItems[pair.Key] = item;
      }
    }
  }
}