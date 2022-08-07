using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;
using UnityEngine.AI;

namespace RPG.Inventories
{
  public class ItemDropper : MonoBehaviour, ISaveable
  {
    List<Pickup> _droppedItems = new();
    public void DropItem(InventoryItem item, int number)
    {
      SpawnPickup(item, GetDropLocation(), number);
    }

    public void DropItem(InventoryItem item)
    {
      SpawnPickup(item, GetDropLocation(), 1);
    }

    protected virtual Vector3 GetDropLocation()
    {
      if (NavMesh.SamplePosition(transform.position, out var hit, .1f, NavMesh.AllAreas))
        return hit.position;
      return transform.position;
    }

    public void SpawnPickup(InventoryItem item, Vector3 spawnLocation, int number)
    {
      var pickup = item.SpawnPickup(spawnLocation, number);
      _droppedItems.Add(pickup);
    }

    [System.Serializable]
    struct DropRecord
    {
      public string ItemID;
      public SerializableVector3 Position;
      public int Number;
    }

    object ISaveable.CaptureState()
    {
      RemoveDestroyedDrops();
      var droppedItemsList = new DropRecord[_droppedItems.Count];
      for (int i = 0; i < droppedItemsList.Length; i++)
      {
        droppedItemsList[i].ItemID = _droppedItems[i].Item.ItemID;
        droppedItemsList[i].Position = new(_droppedItems[i].transform.position);
        droppedItemsList[i].Number = _droppedItems[i].Number;
      }
      return droppedItemsList;
    }

    void ISaveable.RestoreState(object state)
    {
      var droppedItemsList = state as DropRecord[];
      foreach (var item in droppedItemsList)
      {
        var pickupItem = InventoryItem.GetFromID(item.ItemID);
        var position = item.Position.ToVector();
        int number = item.Number;
        SpawnPickup(pickupItem, position, number);
      }
    }

    void RemoveDestroyedDrops()
    {
      List<Pickup> newList = new();
      foreach (var item in _droppedItems)
      {
        if (item != null)
          newList.Add(item);
      }
      _droppedItems = newList;
    }
  }
}