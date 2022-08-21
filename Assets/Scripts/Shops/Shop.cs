using System;
using System.Collections.Generic;
using System.Linq;
using RPG.Control;
using RPG.Inventories;
using RPG.Manager;
using RPG.Stats;
using UnityEngine;

namespace RPG.Shops
{
  public class Shop : MonoBehaviour, IRaycastable
  {
    [SerializeField] string _shopName;
    [SerializeField] StockItemConfig[] _stockConfig;
    [Range(0, 100)][SerializeField] float _sellingDiscountPercentage = 75, _minCharmDiscount = 20;
    [SerializeField] bool _raycastable;
    Inventory _shopperInventory;
    readonly Dictionary<InventoryItem, int> _transaction = new(), _stock = new();
    Shopper _shopper;
    bool _isBuying = true;
    ItemCategory _filter = ItemCategory.None;
    public Shopper CurShopper { set => _shopper = value; }
    public ItemCategory Filter
    {
      get => _filter; set
      {
        _filter = value;
        OnChange?.Invoke();
      }
    }
    public IEnumerable<ShopItem> FilteredItems
    {
      get
      {
        foreach (var item in AllItems)
          if (_filter is ItemCategory.None || item.Item.Category == _filter)
            yield return item;
      }
    }
    public IEnumerable<ShopItem> AllItems
    {
      get
      {
        foreach (var cfg in _stockConfig)
        {
          _transaction.TryGetValue(cfg.Item, out var quantity);
          yield return new(cfg.Item, GetAvailability(cfg.Item), GetPrice(cfg), quantity);
        }
      }
    }

    public bool IsBuying => _isBuying;
    public bool CanTransact
    {
      get
      {
        if (IsEmpty || NotEnoughMoney || NotEnoughSpace) return false;
        return true;
      }
    }
    public float TransactionTotal => AllItems.Sum(i => i.Price * i.Quantity);
    public event Action OnChange;
    public string Name => _shopName;
    public CursorType CursorType => CursorType.Shop;
    public bool IsEmpty => _transaction.Count == 0;
    public bool NotEnoughMoney
    {
      get
      {
        if (!_isBuying) return false;
        var purse = SceneMgr.Self.Player.GetComponent<Purse>();
        if (!purse) return false;
        return purse.Balance < TransactionTotal;
      }
    }
    public bool NotEnoughSpace
    {
      get
      {
        if (!_isBuying) return false;
        List<InventoryItem> flatList = new(AllItems.Sum(i => i.Quantity));
        foreach (var shopItem in AllItems)
          for (int i = 0; i < shopItem.Quantity; i++)
            flatList.Add(shopItem.Item);
        return !_shopperInventory.HasSpaceFor(flatList);
      }
    }
    public float Discount => 1 - Mathf.Min(_minCharmDiscount, _shopper.GetComponent<BaseStats>().GetStat(StatsEnum.DiscountPercentage)) / 100;

    void Awake()
    {
      foreach (var cfg in _stockConfig)
        _stock[cfg.Item] = cfg.InitialStock;
    }
    void Start()
    {
      _shopperInventory = SceneMgr.Self.Player.GetComponent<Inventory>();
    }

    int GetAvailability(InventoryItem item)
    {
      if (IsBuying) return _stock[item];
      if (!_shopperInventory) return 0;
      int sum = 0;
      for (int i = 0; i < _shopperInventory.Size; i++)
        if (_shopperInventory.GetItemInSlot(i) == item)
          sum += _shopperInventory.GetNumberInSlot(i);
      return sum;
    }

    float GetPrice(StockItemConfig cfg)
    {
      if (IsBuying)
        return cfg.Item.Price * (100f - cfg.BuyingDiscountPercentage) / 100f * Discount;
      return cfg.Item.Price * _sellingDiscountPercentage / 100f / Discount;
    }

    public void SelectMode(bool isBuying)
    {
      _isBuying = isBuying;
      OnChange?.Invoke();
    }
    public void AddToTransaction(InventoryItem item, int quantity)
    {
      if (!_transaction.ContainsKey(item)) _transaction[item] = 0;
      var availability = GetAvailability(item);
      if (_transaction[item] + quantity > availability)
        _transaction[item] = availability;
      else
        _transaction[item] += quantity;
      if (_transaction[item] <= 0) _transaction.Remove(item);
      OnChange?.Invoke();
    }
    public void ConfirmTransaction()
    {
      if (!_shopperInventory || !_shopper.TryGetComponent<Purse>(out var purse)) return;
      foreach (var shopItem in AllItems)
      {
        for (var i = 0; i < shopItem.Quantity; ++i)
        {
          if (_isBuying)
            BuyItem(purse, shopItem);
          else
            SellItem(purse, shopItem);
        }
      }
    }

    void SellItem(Purse purse, ShopItem shopItem)
    {
      int slotIdx = _shopperInventory.FindFirstItemSlot(shopItem.Item);
      if (slotIdx < 0) return;
      _shopperInventory.RemoveFromSlot(slotIdx, 1);
      ++_stock[shopItem.Item];
      purse.UpdateBalance(shopItem.Price);
      AddToTransaction(shopItem.Item, -1);
    }

    void BuyItem(Purse purse, ShopItem shopItem)
    {
      if (shopItem.Price > purse.Balance) return;
      if (_shopperInventory.AddToFirstEmptySlot(shopItem.Item, 1))
      {
        --_stock[shopItem.Item];
        purse.UpdateBalance(-shopItem.Price);
        AddToTransaction(shopItem.Item, -1);
      }
    }

    public bool HandleRaycast(PlayerController playerCtrl)
    {
      if (!_raycastable) return false;
      if (Input.GetMouseButtonDown(0))
        ActivateShop(playerCtrl);
      return true;
    }

    public void ActivateShop(PlayerController playerCtrl)
    {
      playerCtrl.GetComponent<Shopper>().ActiveShop = this;
    }

    [Serializable]
    class StockItemConfig
    {
      public InventoryItem Item;
      public int InitialStock;
      [Range(0, 100)]
      public float BuyingDiscountPercentage;
    }
  }
}