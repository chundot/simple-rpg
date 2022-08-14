using RPG.Inventories;
using RPG.Shops;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Shops
{
  public class FilterButtonUI : MonoBehaviour
  {
    [SerializeField] ItemCategory _category = ItemCategory.None;
    Button _btn;
    Shop _curShop;
    public Shop CurShop { set => _curShop = value; }
    void Awake()
    {
      _btn = GetComponent<Button>();
      _btn.onClick.AddListener(SelectFilter);
    }

    public void Redraw()
    {
      _btn.interactable = _curShop.Filter != _category;
    }

    void SelectFilter()
    {
      _curShop.Filter = _category;
    }
  }
}