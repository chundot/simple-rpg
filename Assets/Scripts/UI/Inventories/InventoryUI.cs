using UnityEngine;
using RPG.Inventories;

namespace RPG.UI.Inventories
{
  public class InventoryUI : MonoBehaviour
  {
    [SerializeField] InventorySlotUI _inventoryItemPrefab = null;
    Inventory _playerInventory;

    void Awake()
    {
      _playerInventory = Inventory.PlayerInventory;
      _playerInventory.InventoryUpdated += Redraw;
    }

    void Start()
    {
      Redraw();
    }

    void Redraw()
    {
      foreach (Transform child in transform)
        Destroy(child.gameObject);

      for (int i = 0; i < _playerInventory.Size; i++)
      {
        var itemUI = Instantiate(_inventoryItemPrefab, transform);
        itemUI.Setup(_playerInventory, i);
      }
    }
  }
}