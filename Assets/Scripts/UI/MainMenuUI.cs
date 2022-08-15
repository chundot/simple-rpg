using RPG.Scene;
using RPG.Utils;
using TMPro;
using UnityEngine;

namespace RPG.UI
{
  public class MainMenuUI : MonoBehaviour
  {
    [SerializeField] TMP_InputField _saveField;
    LazyValue<SavingWrapper> _savingWrapper;
    void Awake()
    {
      _savingWrapper = new(() => FindObjectOfType<SavingWrapper>());
    }
    public void NewGame()
    {
      _savingWrapper.Value.NewGame(_saveField.text);
    }
    public void ContinueGame()
    {
      _savingWrapper.Value.ContinueGame();
    }
    public void Quit()
    {
#if UNITY_EDITOR
      UnityEditor.EditorApplication.isPlaying = false;
#else
      Application.Quit();
#endif
    }
  }

}