using RPG.Inventories;

namespace RPG.UI.Inventories
{
  public interface IItemHolder
  {
    InventoryItem Item { get; }
  }
}