using UnityEngine;

namespace RPG.UI
{
  public class HealthBar : MonoBehaviour
  {
    [SerializeField] bool _show = true;
    [SerializeField] RectTransform _frame, _fill;
    void Start()
    {
      ChangeBar(false);
    }
    public void SetFill(float percent)
    {
      if (!_show) return;
      if (!gameObject.activeSelf)
        ChangeBar(true);
      _fill.localScale = new(percent, 1, 1);
    }
    public void ChangeBar(bool show)
    {
      gameObject.SetActive(show);
    }
  }

}