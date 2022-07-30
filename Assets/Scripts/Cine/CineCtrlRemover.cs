using RPG.Control;
using RPG.Core;
using RPG.Manager;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cine
{
  public class CineCtrlRemover : MonoBehaviour
  {
    Transform Player { get => SceneMgr.Self.Player; }
    void Start()
    {
      GetComponent<PlayableDirector>().played += DisableCtrl;
      GetComponent<PlayableDirector>().stopped += EnableCtrl;
    }
    void DisableCtrl(PlayableDirector pd)
    {
      Player.GetComponent<ActionScheduler>().CancelCurAction();
      Player.GetComponent<PlayerController>().enabled = false;
    }
    void EnableCtrl(PlayableDirector pd)
    {
      Player.GetComponent<PlayerController>().enabled = true;
    }
  }

}