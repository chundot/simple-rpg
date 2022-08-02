using System.Collections;
using RPG.Saving;
using UnityEngine;

namespace RPG.Scene
{
  public class SavingWrapper : MonoBehaviour
  {
    SavingSystem _system;
    string _saveFile = "save";
    IEnumerator Start()
    {
      _system = GetComponent<SavingSystem>();
      var fader = FindObjectOfType<Fader>();
      fader.InstantFadeOut();
      yield return _system.LoadLastScene(_saveFile);
      yield return fader.FadeIn(.5f);
    }
    // Update is called once per frame
    void Update()
    {
      if (Input.GetKeyDown(KeyCode.S))
        Save();
      if (Input.GetKeyDown(KeyCode.L))
        Load();
    }

    public void Save()
    {
      _system.Save(_saveFile);
    }

    public void Load()
    {
      _system.Load(_saveFile);
    }
  }

}