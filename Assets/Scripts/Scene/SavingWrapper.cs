using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Saving;
using RPG.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Scene
{
  public class SavingWrapper : MonoBehaviour
  {
    [SerializeField] int _firstSceneBuildIdx = 1, _menuSceneBuildIdx = 0;
    LazyValue<Fader> _fader;
    SavingSystem _system;
    const string _saveFile = "save";
    string CurSave
    {
      get => PlayerPrefs.GetString("curSaveName", _saveFile);
      set => PlayerPrefs.SetString("curSaveName", value);
    }
    public void Awake()
    {
      _system = GetComponent<SavingSystem>();
      _fader = new(() => FindObjectOfType<Fader>());
    }
    public void ContinueGame()
    {
      StartCoroutine(LoadLastScene());
    }

    public void NewGame(string saveFile)
    {
      CurSave = saveFile;
      StartCoroutine(LoadFirstScene());
    }

    public void LoadGame(string save)
    {
      CurSave = save;
      ContinueGame();
    }
    public void DeleteSave(string save)
    {
      CurSave = save;
      Del();
      CurSave = _saveFile;
    }
    public void LoadMenu()
    {
      StartCoroutine(LoadMenuScene());
    }
    IEnumerator LoadScene(int idx)
    {
      yield return _fader.Value.FadeOut(.5f);
      yield return SceneManager.LoadSceneAsync(idx);
      yield return _fader.Value.FadeIn(.5f);
    }
    IEnumerator LoadFirstScene()
    {
      yield return LoadScene(_firstSceneBuildIdx);
    }
    IEnumerator LoadMenuScene()
    {
      yield return LoadScene(_menuSceneBuildIdx);
    }

    IEnumerator LoadLastScene()
    {
      yield return _fader.Value.FadeOut(.5f);
      yield return _system.LoadLastScene(CurSave);
      yield return _fader.Value.FadeIn(.5f);
    }

    void Update()
    {
      if (Input.GetKeyDown(KeyCode.F5))
        Save();
      if (Input.GetKeyDown(KeyCode.F6))
        Load();
      if (Input.GetKeyDown(KeyCode.Delete))
        Del();
    }

    public void Save()
    {
      _system.Save(CurSave);
    }

    public void Load()
    {
      _system.Load(CurSave);
    }
    public void Del()
    {
      _system.Delete(CurSave);
    }
    public IEnumerable<string> ListSaves()
    {
      return _system.ListSaves();
    }
  }

}