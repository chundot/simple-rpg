using System;
using RPG.Manager;
using UnityEngine;

namespace RPG.Quests
{
  public class QuestCompletion : MonoBehaviour
  {
    [SerializeField] QuestCompleter[] _completers;
    public void CompleteObjective()
    {
      var list = SceneMgr.Self.Player.GetComponent<QuestList>();
      foreach (var completer in _completers)
        foreach (var refer in completer.Refers)
          list.CompleteObjective(completer.QuestID, refer);
    }
    [Serializable]
    struct QuestCompleter
    {
      public Quest QuestID;
      public string[] Refers;
    }
  }

}