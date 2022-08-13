using System;
using System.Collections.Generic;
using RPG.Control;
using RPG.Inventories;
using UnityEngine;

namespace RPG.Shops
{
  public class Shop : MonoBehaviour, IRaycastable
  {
    [SerializeField] string _shopName;
    public IEnumerable<ShopItem> FilteredItems => null;
    public bool IsBuying => true;
    public bool CanTransact => true;
    public float TransactionTotal => 100;
    public event Action OnChange;
    public string Name => _shopName;
    public void SelectFilter(ItemCategory category)
    {

    }
    public ItemCategory Filter => ItemCategory.None;

    public CursorType CursorType => CursorType.Shop;

    public void SelectMode(bool isBuying)
    {

    }
    public void AddToTransaction(InventoryItem item, int quantity)
    {

    }
    public void ConfirmTransaction()
    {

    }

    public bool HandleRaycast(PlayerController playerCtrl)
    {
      if (Input.GetMouseButtonDown(0))
      {
        playerCtrl.GetComponent<Shopper>().ActiveShop = this;
      }
      return true;
    }

    public class ShopItem
    {
      public InventoryItem Item;
      public int Availabiliy, Quantity;
      public float Price;
    }
  }
}