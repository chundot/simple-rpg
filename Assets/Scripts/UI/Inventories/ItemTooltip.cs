using UnityEngine;
using TMPro;
using RPG.Inventories;

namespace RPG.UI.Inventories
{
  public class ItemTooltip : MonoBehaviour
  {
    [SerializeField] TextMeshProUGUI _titleText = null;
    [SerializeField] TextMeshProUGUI _bodyText = null;

    public void Setup(InventoryItem item)
    {
      _titleText.text = item.DisplayName;
      _bodyText.text = item.Description;
    }
  }
}
