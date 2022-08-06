using UnityEngine;

namespace RPG.Inventories
{
  public class Pickup : MonoBehaviour
  {
    InventoryItem _item;
    int _number = 1;

    Inventory _inventory;
    public InventoryItem Item => _item;

    public int Number => _number;

    public bool CanBePickedUp => _inventory.HasSpaceFor(_item);

    private void Awake()
    {
      var player = GameObject.FindGameObjectWithTag("Player");
      _inventory = player.GetComponent<Inventory>();
    }

    public void Setup(InventoryItem item, int number)
    {
      _item = item;
      if (!item.IsStackable)
        number = 1;
      _number = number;
    }

    public void PickupItem()
    {
      bool foundSlot = _inventory.AddToFirstEmptySlot(_item, _number);
      if (foundSlot)
        Destroy(gameObject);
    }
  }
}