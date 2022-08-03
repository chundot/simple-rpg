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
    PlayableDirector _pd;
    void Awake()
    {
      _pd = GetComponent<PlayableDirector>();
    }
    void OnEnable()
    {
      _pd.played += DisableCtrl;
      _pd.stopped += EnableCtrl;
    }
    void OnDisable()
    {
      _pd.played -= DisableCtrl;
      _pd.stopped -= EnableCtrl;
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