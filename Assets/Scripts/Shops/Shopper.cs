using System;
using UnityEngine;

namespace RPG.Shops
{
  public class Shopper : MonoBehaviour
  {
    public event Action OnActiveShopChange;
    Shop _activeShop;
    public Shop ActiveShop
    {
      get => _activeShop;
      set
      {
        if (_activeShop) _activeShop.CurShopper = null;
        _activeShop = value;
        if (_activeShop) _activeShop.CurShopper = this;
        OnActiveShopChange?.Invoke();
      }
    }
  }

}