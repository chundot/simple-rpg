using RPG.Manager;
using UnityEngine;

namespace RPG.Quests
{
  public class QuestCompletion : MonoBehaviour
  {
    [SerializeField] Quest _quest;
    [SerializeField] string _objectiveRefer;
    public void CompleteObjective()
    {
      var list = SceneMgr.Self.Player.GetComponent<QuestList>();
      list.CompleteObjective(_quest, _objectiveRefer);
    }
  }

}