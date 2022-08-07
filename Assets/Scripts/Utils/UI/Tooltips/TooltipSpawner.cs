using UnityEngine;
using UnityEngine.EventSystems;

namespace RPG.Core.UI.Tooltips
{
  public abstract class TooltipSpawner : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
  {
    [Tooltip("tooltip.")]
    [SerializeField] GameObject _tooltipPrefab = null;

    GameObject tooltip = null;

    public abstract void UpdateTooltip(GameObject tooltip);

    public abstract bool CanCreateTooltip { get; }

    void OnDestroy()
    {
      ClearTooltip();
    }

    void OnDisable()
    {
      ClearTooltip();
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
      var parentCanvas = GetComponentInParent<Canvas>();

      if (tooltip && !CanCreateTooltip)
        ClearTooltip();

      if (!tooltip && CanCreateTooltip)
        tooltip = Instantiate(_tooltipPrefab, parentCanvas.transform);

      if (tooltip)
      {
        UpdateTooltip(tooltip);
        PositionTooltip();
      }
    }

    void PositionTooltip()
    {
      Canvas.ForceUpdateCanvases();

      var tooltipCorners = new Vector3[4];
      tooltip.GetComponent<RectTransform>().GetWorldCorners(tooltipCorners);
      var slotCorners = new Vector3[4];
      GetComponent<RectTransform>().GetWorldCorners(slotCorners);

      bool below = transform.position.y > Screen.height / 2;
      bool right = transform.position.x < Screen.width / 2;

      int slotCorner = GetCornerIndex(below, right);
      int tooltipCorner = GetCornerIndex(!below, !right);

      tooltip.transform.position = slotCorners[slotCorner] - tooltipCorners[tooltipCorner] + tooltip.transform.position;
    }

    int GetCornerIndex(bool below, bool right)
    {
      if (below && !right) return 0;
      else if (!below && !right) return 1;
      else if (!below && right) return 2;
      else return 3;

    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {

      if (!eventData.fullyExited)
        return;
      // if (eventData.pointerCurrentRaycast.gameObject.transform.IsChildOf(transform))
      //   return;
      ClearTooltip();
    }

    void ClearTooltip()
    {
      if (tooltip)
        Destroy(tooltip);
    }
  }
}