using UnityEngine;
using RPG.Inventories;
using RPG.Core.UI.Dragging;

namespace RPG.UI.Inventories
{
  public class InventorySlotUI : MonoBehaviour, IItemHolder, IDragContainer<InventoryItem>
  {
    [SerializeField] InventoryItemIcon _icon = null;
    int _index;
    InventoryItem _item;
    Inventory _inventory;
    public InventoryItem Item => _inventory.GetItemInSlot(_index);
    public int Number => _inventory.GetNumberInSlot(_index);
    public void Setup(Inventory inventory, int index)
    {
      _inventory = inventory;
      _index = index;
      _icon.SetItem(inventory.GetItemInSlot(index), inventory.GetNumberInSlot(index));
    }

    public int MaxAcceptable(InventoryItem item)
    {
      return _inventory.HasSpaceFor(item) ? int.MaxValue : 0;
    }

    public void AddItems(InventoryItem item, int number)
    {
      _inventory.AddItemToSlot(_index, item, number);
    }

    public void RemoveItems(int number)
    {
      _inventory.RemoveFromSlot(_index, number);
    }
  }
}