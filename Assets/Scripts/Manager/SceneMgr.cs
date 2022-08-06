using RPG.Manager.Common;
using UnityEngine;

namespace RPG.Manager
{
  public class SceneMgr : BaseMgr<SceneMgr>
  {
    Transform _player;
    public Transform Player
    {
      get
      {
        if (!_player)
          _player = GameObject.FindGameObjectWithTag("Player").transform;
        return _player;
      }
      set
      {
        if (!_player)
          _player = value;
      }
    }
  }

}