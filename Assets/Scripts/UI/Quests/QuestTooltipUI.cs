using RPG.Quests;
using TMPro;
using UnityEngine;

namespace RPG.UI.Quests
{
  public class QuestTooltipUI : MonoBehaviour
  {
    [SerializeField] TextMeshProUGUI _title;
    [SerializeField] Transform _objectives;
    [SerializeField] GameObject _objectivePrefab, _objectiveIncompletePrefab;
    public void Setup(QuestStatus status)
    {
      _title.text = status.Quest.Title;
      _objectives.DestroyAllChildren();
      foreach (var objective in status.Quest.Objectives)
      {
        var objectiveInstance = Instantiate(status.IsCompleted(objective) ? _objectivePrefab : _objectiveIncompletePrefab, _objectives);
        objectiveInstance.GetComponentInChildren<TextMeshProUGUI>().text = objective;
      }
    }
  }
}