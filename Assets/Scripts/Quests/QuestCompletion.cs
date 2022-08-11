using RPG.Manager;
using UnityEngine;

namespace RPG.Quests
{
  public class QuestCompletion : MonoBehaviour
  {
    [SerializeField] Quest _quest;
    [SerializeField] string _objective;
    public void CompleteObjective()
    {
      var list = SceneMgr.Self.Player.GetComponent<QuestList>();
      list.CompleteObjective(_quest, _objective);
    }
  }

}