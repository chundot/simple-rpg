
using UnityEngine;

namespace RPG.UI
{
  public class UISwitcher : MonoBehaviour
  {
    [SerializeField] GameObject _initObj;
    void Awake()
    {
      SwitchTo(_initObj);
    }
    public void SwitchTo(GameObject display)
    {
      foreach (Transform child in transform)
        child.gameObject.SetActive(display == child.gameObject);
    }
  }

}