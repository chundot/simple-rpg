using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
  public class HealthBarUI : MonoBehaviour
  {
    [SerializeField] Image _bar;

    public void RedrawByPercentage(float percentage)
    {
      _bar.transform.localScale = new(percentage / 100, 1);
    }
    public void RedrawByFraction(float fraction)
    {
      _bar.transform.localScale = new(fraction, 1);
    }
  }

}