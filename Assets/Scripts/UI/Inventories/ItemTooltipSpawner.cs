using UnityEngine;
using RPG.Core.UI.Tooltips;

namespace RPG.UI.Inventories
{
  [RequireComponent(typeof(IItemHolder))]
  public class ItemTooltipSpawner : TooltipSpawner
  {
    public override bool CanCreateTooltip => GetComponent<IItemHolder>().Item;

    public override void UpdateTooltip(GameObject tooltip)
    {
      if (!tooltip.TryGetComponent<ItemTooltip>(out var itemTooltip)) return;
      var item = GetComponent<IItemHolder>().Item;
      itemTooltip.Setup(item);
    }
  }
}