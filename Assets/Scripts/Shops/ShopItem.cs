using RPG.Inventories;
using UnityEngine;

namespace RPG.Shops
{
  public class ShopItem
  {
    readonly InventoryItem _item;
    int _availabiliy, _quantity;
    float _price;
    public string Name => _item.DisplayName;
    public int Availabiliy => _availabiliy;
    public float Price => _price;
    public Sprite Icon => _item.Icon;
    public InventoryItem Item => _item;
    public int Quantity => _quantity;
    public ShopItem(InventoryItem item, int availabiliy, float price, int quantity)
    {
      _item = item;
      _availabiliy = availabiliy;
      _price = price;
      _quantity = quantity;
    }
  }
}