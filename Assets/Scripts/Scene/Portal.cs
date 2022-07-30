using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Manager;
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
      yield return fader.FadeOut(.5f);
      yield return SceneManager.LoadSceneAsync(SceneToLoad);
      var otherPortal = GetOtherPortal();
      UpdatePlayer(otherPortal);
      print("Scene Loaded");
      yield return fader.FadeIn(.5f);
      Destroy(this);
    }

    private void UpdatePlayer(Portal otherPortal)
    {
      var player = GameObject.FindWithTag("Player").transform;
      var agent = player.GetComponent<NavMeshAgent>();
      agent.Warp(otherPortal.SpawnPoint.position);
      player.rotation = otherPortal.SpawnPoint.rotation;
    }

    private Portal GetOtherPortal()
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