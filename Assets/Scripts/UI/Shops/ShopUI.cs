using RPG.Manager;
using RPG.Shops;
using TMPro;
using UnityEngine;

namespace RPG.UI.Shops
{
  public class ShopUI : MonoBehaviour
  {
    [SerializeField] TextMeshProUGUI _shopName;
    Shopper _shopper;
    Shop _curShop;
    void Start()
    {
      _shopper = SceneMgr.Self.Player.GetComponent<Shopper>();
      if (_shopper) _shopper.OnActiveShopChange += ShopChanged;
      ShopChanged();
    }

    void ShopChanged()
    {
      _curShop = _shopper.ActiveShop;
      _shopName.text = _curShop.Name;
      gameObject.SetActive(_curShop);
    }

    public void Close()
    {
      _shopper.ActiveShop = null;
      ShopChanged();
    }
  }

}