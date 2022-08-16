
using RPG.Abilities;
using RPG.Core.UI.Dragging;
using RPG.Inventories;
using RPG.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Inventories
{
  public class ActionSlotUI : MonoBehaviour, IItemHolder, IDragContainer<InventoryItem>
  {
    [SerializeField] InventoryItemIcon _icon = null;
    [SerializeField] int _index = 0;
    [SerializeField] Image _cooldownOverlay;
    ActionStore _store;
    CooldownStore _cdStore;
    public InventoryItem Item => _store.GetAction(_index);

    public int Number => _store.GetNumber(_index);

    void Awake()
    {
      _store = SceneMgr.Self.Player.GetComponent<ActionStore>();
      _cdStore = SceneMgr.Self.Player.GetComponent<CooldownStore>();
      _store.StoreUpdated += UpdateIcon;
    }

    void Update()
    {
      if (Item) _cooldownOverlay.fillAmount = _cdStore.GetFractionRemaining(Item);
    }

    public void AddItems(InventoryItem item, int number)
    {
      _store.AddAction(item, _index, number);
    }

    public int MaxAcceptable(InventoryItem item)
    {
      return _store.MaxAcceptable(item, _index);
    }

    public void RemoveItems(int number)
    {
      _store.RemoveItems(_index, number);
    }

    void UpdateIcon()
    {
      _icon.SetItem(Item, Number);
    }
  }
}
