using System.Collections;
using RPG.Saving;
using UnityEngine;

namespace RPG.Scene
{
  public class SavingWrapper : MonoBehaviour
  {
    SavingSystem _system;
    const string _saveFile = "save";
    void Awake()
    {
      StartCoroutine(LoadLastScene());
    }
    IEnumerator LoadLastScene()
    {
      _system = GetComponent<SavingSystem>();
      yield return _system.LoadLastScene(_saveFile);
      var fader = FindObjectOfType<Fader>();
      fader.InstantFadeOut();
      yield return fader.FadeIn(.5f);
    }
    // Update is called once per frame
    void Update()
    {
      if (Input.GetKeyDown(KeyCode.S))
        Save();
      if (Input.GetKeyDown(KeyCode.L))
        Load();
      if (Input.GetKeyDown(KeyCode.Delete))
        Del();
    }

    public void Save()
    {
      _system.Save(_saveFile);
    }

    public void Load()
    {
      _system.Load(_saveFile);
    }
    public void Del()
    {
      _system.Delete(_saveFile);
    }
  }

}