using RPG.Control;
using RPG.Manager;
using RPG.Scene;
using RPG.Utils;
using UnityEngine;

namespace RPG.UI
{
  public class PauseMenuUI : MonoBehaviour
  {
    LazyValue<PlayerController> _playerController;
    LazyValue<SavingWrapper> _savingWrapper;
    void Awake()
    {
      _savingWrapper = new(() => FindObjectOfType<SavingWrapper>());
      _playerController = new(() => SceneMgr.Self.Player.GetComponent<PlayerController>());
    }
    void OnEnable()
    {
      _playerController.Value.enabled = false;
      Time.timeScale = 0;
    }
    void OnDisable()
    {
      _playerController.Value.enabled = true;
      Time.timeScale = 1;
    }

    public void Save()
    {
      _savingWrapper.Value.Save();
    }

    public void SaveAndQuit()
    {
      Save();
      _savingWrapper.Value.LoadMenu();
    }
  }

}