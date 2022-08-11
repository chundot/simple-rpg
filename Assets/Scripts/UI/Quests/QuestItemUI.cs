using RPG.Quests;
using TMPro;
using UnityEngine;

namespace RPG.UI.Quests
{
  public class QuestItemUI : MonoBehaviour
  {
    [SerializeField] TextMeshProUGUI _title, _progress;
    QuestStatus _status;
    public QuestStatus Status => _status;
    public void Setup(QuestStatus status)
    {
      _status = status;
      _title.text = status.Quest.Title;
      _progress.text = $"{status.CompletedCount} / {status.Quest.ObjectiveCount}";
    }
  }
}