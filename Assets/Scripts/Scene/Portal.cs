using System.Collections;
using RPG.Control;
using RPG.Extensions;
using RPG.Manager;
using RPG.Saving;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace RPG.Scene
{
  public class Portal : MonoBehaviour
  {
    public int SceneToLoad = 0, Dest = 0;
    public Transform SpawnPoint;
    bool _triggered = false;
    void OnTriggerEnter(Collider other)
    {
      if (_triggered) return;
      if (other.CompareTag("Player"))
      {
        StartCoroutine(StartTransition());
        _triggered = true;
      }
    }
    IEnumerator StartTransition()
    {
      if (SceneToLoad < 0) yield break;
      DontDestroyOnLoad(this);
      var fader = FindObjectOfType<Fader>();
      SceneMgr.Self.Player.GetComponent<PlayerController>().enabled = false;
      yield return fader.FadeOut(.5f);
      var wrapper = FindObjectOfType<SavingWrapper>();
      wrapper.Save();
      yield return SceneManager.LoadSceneAsync(SceneToLoad);
      SceneMgr.Self.Player.GetComponent<PlayerController>().enabled = false;
      wrapper.Load();
      var otherPortal = GetOtherPortal();
      UpdatePlayer(otherPortal);
      wrapper.Save();
      fader.FadeIn(.5f);
      SceneMgr.Self.Player.GetComponent<PlayerController>().enabled = true;
      Destroy(this);
    }

    void UpdatePlayer(Portal otherPortal)
    {
      var player = SceneMgr.Self.Player;
      var agent = player.GetComponent<NavMeshAgent>();
      agent.Warp(new SerializableVector3(otherPortal.SpawnPoint.position));
      player.rotation = otherPortal.SpawnPoint.rotation;
    }

    Portal GetOtherPortal()
    {
      foreach (var portal in FindObjectsOfType<Portal>())
      {
        if (portal == this || portal.Dest != Dest) continue;
        return portal;
      }
      return null;
    }
  }

}