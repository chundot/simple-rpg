using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
  public class ShowHideUI : MonoBehaviour
  {
    [SerializeField] KeyCode _toggleKey = KeyCode.Escape;
    [SerializeField] GameObject _uiContainer = null;

    void Start()
    {
      _uiContainer.SetActive(false);
    }

    void Update()
    {
      if (Input.GetKeyDown(_toggleKey))
      {
        Toggle();
      }
    }

    public void Toggle()
    {
      _uiContainer.SetActive(!_uiContainer.activeSelf);
    }
  }
}