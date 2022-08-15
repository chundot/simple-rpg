using RPG.Scene;
using UnityEngine;

namespace RPG.UI
{
  public class LoadSaveUI : MonoBehaviour
  {
    [SerializeField] Transform _contentRoot;
    [SerializeField] SaveSlotUI _slotPrefab;
    SavingWrapper _savingWrapper;
    void Awake()
    {
      _savingWrapper = FindObjectOfType<SavingWrapper>();
    }
    void OnEnable()
    {
      Redraw();
    }
    void Redraw()
    {
      if (!_savingWrapper) return;
      _contentRoot.DestroyAllChildren();
      foreach (var save in _savingWrapper.ListSaves())
      {
        var instance = Instantiate(_slotPrefab, _contentRoot);
        instance.Text.text = save;
        instance.LoadBtn.onClick.AddListener(() => _savingWrapper.LoadGame(save));
        instance.DelBtn.onClick.AddListener(() => { _savingWrapper.DeleteSave(save); Redraw(); });
      }
    }
  }

}