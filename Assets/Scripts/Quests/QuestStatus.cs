using System;
using System.Collections.Generic;
using RPG.Saving;
using UnityEngine;

namespace RPG.Quests
{
  [Serializable]
  public class QuestStatus
  {
    [SerializeField] Quest _quest;
    [SerializeField] List<string> _completedObjectives = new();

    public QuestStatus(Quest quest)
    {
      _quest = quest;
    }

    public QuestStatus(object obj)
    {
      if (obj is not QuestStatusRecord record) return;
      _quest = Quest.GetByName(record.QuestName);
      _completedObjectives = record.CompletedObjectives;
    }

    public Quest Quest => _quest;
    public int CompletedCount => _completedObjectives.Count;
    public bool IsCompleted(string objective) => _completedObjectives.Contains(objective);

    public void CompleteObjective(string objective)
    {
      if (HasObjective(objective)) return;
      _completedObjectives.Add(objective);
    }
    public bool HasObjective(string objective) => _completedObjectives.Contains(objective);

    public object CaptureState()
    {
      return new QuestStatusRecord()
      {
        QuestName = _quest.Title,
        CompletedObjectives = _completedObjectives
      };
    }
    [Serializable]
    class QuestStatusRecord
    {
      public string QuestName;
      public List<string> CompletedObjectives;
    }
  }
}
