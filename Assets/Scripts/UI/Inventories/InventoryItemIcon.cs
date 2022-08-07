using UnityEngine;
using UnityEngine.UI;
using RPG.Inventories;
using TMPro;

namespace RPG.UI.Inventories
{
  [RequireComponent(typeof(Image))]
  public class InventoryItemIcon : MonoBehaviour
  {
    [SerializeField] GameObject _textContainer = null;
    [SerializeField] TextMeshProUGUI _itemNumber = null;

    public void SetItem(InventoryItem item)
    {
      SetItem(item, 0);
    }

    public void SetItem(InventoryItem item, int number)
    {
      var iconImage = GetComponent<Image>();
      if (!item)
        iconImage.enabled = false;
      else
      {
        iconImage.enabled = true;
        iconImage.sprite = item.Icon;
      }
      if (_itemNumber)
      {
        if (number <= 1)
          _textContainer.SetActive(false);
        else
        {
          _textContainer.SetActive(true);
          _itemNumber.text = number.ToString();
        }
      }
    }
  }
}