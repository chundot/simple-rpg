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
        _activeShop = value;
        OnActiveShopChange?.Invoke();
      }
    }

    void Start()
    {

    }

    void Update()
    {

    }
  }

}