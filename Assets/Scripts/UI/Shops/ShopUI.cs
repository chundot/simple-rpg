using RPG.Manager;
using RPG.Shops;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Shops
{
  public class ShopUI : MonoBehaviour
  {
    [SerializeField] Transform _listRoot, _category;
    [SerializeField] RowUI _rowPrefab;
    [SerializeField] TextMeshProUGUI _shopName, _total, _confirmText, _switchText;
    [SerializeField] Button _confirmBtn, _switchBtn;
    Shopper _shopper;
    Shop _curShop;
    Color _original;
    static string[] STRS = { "切换到出售", "切换到购买", "购买", "出售" };
    void Start()
    {
      _shopper = SceneMgr.Self.Player.GetComponent<Shopper>();
      if (_shopper) _shopper.OnActiveShopChange += ShopChanged;
      _confirmBtn.onClick.AddListener(Confirm);
      _switchBtn.onClick.AddListener(SwitchMode);
      ShopChanged();
      _original = _total.color;
    }

    void ShopChanged()
    {
      if (_curShop) _curShop.OnChange -= Redraw;
      _curShop = _shopper.ActiveShop;
      gameObject.SetActive(_curShop);
      if (!_curShop) return;
      foreach (var btn in _category.GetComponentsInChildren<FilterButtonUI>())
        btn.CurShop = _curShop;
      _shopName.text = _curShop.Name;
      _curShop.OnChange += Redraw;
      Redraw();
    }

    void Redraw()
    {
      _listRoot.DestroyAllChildren();
      foreach (var item in _curShop.FilteredItems)
      {
        var rowInstance = Instantiate(_rowPrefab, _listRoot);
        rowInstance.Setup(_curShop, item);
      }
      _total.text = $"合计: {_curShop.TransactionTotal}";
      _total.color = !_curShop.NotEnoughMoney ? _original : Color.red;
      _confirmBtn.interactable = _curShop.CanTransact;
      foreach (var btn in _category.GetComponentsInChildren<FilterButtonUI>())
        btn.Redraw();
    }

    public void Close()
    {
      _shopper.ActiveShop = null;
      ShopChanged();
    }

    public void Confirm()
    {
      if (_curShop) _curShop.ConfirmTransaction();
    }
    public void SwitchMode()
    {
      _curShop.SelectMode(!_curShop.IsBuying);
      _switchText.text = _curShop.IsBuying ? STRS[0] : STRS[1];
      _confirmText.text = _curShop.IsBuying ? STRS[2] : STRS[3];
    }
  }

}