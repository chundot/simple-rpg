using UnityEngine;
using UnityEngine.EventSystems;

namespace RPG.Core.UI.Tooltips
{
  public abstract class TooltipSpawner : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
  {
    [Tooltip("tooltip.")]
    [SerializeField] protected GameObject TooltipPrefab = null;

    GameObject _tooltip = null;

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

      if (_tooltip && !CanCreateTooltip)
        ClearTooltip();

      if (!_tooltip && CanCreateTooltip)
        _tooltip = Instantiate(TooltipPrefab, parentCanvas.transform);

      if (_tooltip)
      {
        UpdateTooltip(_tooltip);
        PositionTooltip();
      }
    }

    void PositionTooltip()
    {
      Canvas.ForceUpdateCanvases();

      var tooltipCorners = new Vector3[4];
      _tooltip.GetComponent<RectTransform>().GetWorldCorners(tooltipCorners);
      var slotCorners = new Vector3[4];
      GetComponent<RectTransform>().GetWorldCorners(slotCorners);

      bool below = transform.position.y > Screen.height / 2;
      bool right = transform.position.x < Screen.width / 2;

      int slotCorner = GetCornerIndex(below, right);
      int tooltipCorner = GetCornerIndex(!below, !right);

      _tooltip.transform.position = slotCorners[slotCorner] - tooltipCorners[tooltipCorner] + _tooltip.transform.position;
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
      if (_tooltip)
        Destroy(_tooltip);
    }
  }
}