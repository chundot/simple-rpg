using RPG.Core.UI.Dragging;
using RPG.Inventories;
using UnityEngine;

namespace RPG.UI.Inventories
{
  public class ActionSlotUI : MonoBehaviour, IItemHolder, IDragContainer<InventoryItem>
  {
    [SerializeField] InventoryItemIcon icon = null;
    [SerializeField] int index = 0;

    ActionStore store;
    public InventoryItem Item => store.GetAction(index);

    public int Number => store.GetNumber(index);

    void Awake()
    {
      store = GameObject.FindGameObjectWithTag("Player").GetComponent<ActionStore>();
      store.StoreUpdated += UpdateIcon;
    }

    public void AddItems(InventoryItem item, int number)
    {
      store.AddAction(item, index, number);
    }

    public int MaxAcceptable(InventoryItem item)
    {
      return store.MaxAcceptable(item, index);
    }

    public void RemoveItems(int number)
    {
      store.RemoveItems(index, number);
    }

    void UpdateIcon()
    {
      icon.SetItem(Item, Number);
    }
  }
}
