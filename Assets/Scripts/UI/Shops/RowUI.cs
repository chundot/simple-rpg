using RPG.Shops;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Shops
{
  public class RowUI : MonoBehaviour
  {
    [SerializeField] Image _icon;
    [SerializeField] TextMeshProUGUI _name, _availability, _price, _quantity;
    Shop _curShop;
    ShopItem _item;
    public void Setup(Shop curShop, ShopItem item)
    {
      _curShop = curShop;
      _item = item;
      _icon.sprite = item.Icon;
      _name.text = item.Name;
      _availability.text = $"{item.Availabiliy}";
      _price.text = $"{item.Price}";
      _quantity.text = $"{item.Quantity}";
    }
    public void Add()
    {
      _curShop.AddToTransaction(_item.Item, 1);
    }
    public void Remove()
    {
      _curShop.AddToTransaction(_item.Item, -1);
    }
  }
}
