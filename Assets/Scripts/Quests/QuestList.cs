using System;
using System.Collections.Generic;
using System.Linq;
using RPG.Saving;
using UnityEngine;

namespace RPG.Quests
{
  public class QuestList : MonoBehaviour, ISaveable
  {
    readonly List<QuestStatus> _statuses = new();
    public IEnumerable<QuestStatus> Statuses => _statuses;
    public event Action OnUpdate;
    public void AddQuest(Quest quest)
    {
      if (HasQuest(quest)) return;
      _statuses.Add(new(quest));
      OnUpdate?.Invoke();
    }
    public QuestStatus GetQuestStatus(Quest quest) => _statuses.FirstOrDefault(q => q.Quest == quest);
    public void CompleteObjective(Quest quest, string objective)
    {
      var status = GetQuestStatus(quest);
      if (status == null) return;
      status.CompleteObjective(objective);
      OnUpdate?.Invoke();
    }

    bool HasQuest(Quest quest) => GetQuestStatus(quest) != null;

    public object CaptureState()
    {
      return _statuses.Select(s => s.CaptureState()).ToArray();
    }

    public void RestoreState(object state)
    {
      if (state is not object[] list) return;
      _statuses.Clear();
      foreach (var obj in list)
        _statuses.Add(new(obj));
    }
  }
}