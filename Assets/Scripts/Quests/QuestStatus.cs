using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
  [Serializable]
  public class QuestStatus
  {
    [SerializeField] Quest _quest;
    [SerializeField] List<string> _completedObjectiveRefers = new();

    public QuestStatus(Quest quest)
    {
      _quest = quest;
    }

    public QuestStatus(object obj)
    {
      if (obj is not QuestStatusRecord record) return;
      _quest = Quest.GetByName(record.QuestName);
      _completedObjectiveRefers = record.CompletedObjectives;
    }

    public Quest Quest => _quest;
    public int CompletedCount => _completedObjectiveRefers.Count;

    public bool IsCompleted
    {
      get
      {
        foreach (var obj in _quest.Objectives)
          if (!_completedObjectiveRefers.Contains(obj.Reference))
            return false;
        return true;
      }
    }

    public bool IsObjectiveCompleted(string objectiveRefer) => _completedObjectiveRefers.Contains(objectiveRefer);

    public void CompleteObjective(string objectiveRefer)
    {
      if (HasObjective(objectiveRefer) && !_quest.HasObjective(objectiveRefer)) return;
      _completedObjectiveRefers.Add(objectiveRefer);
    }
    public bool HasObjective(string objectiveRefer) => _completedObjectiveRefers.Contains(objectiveRefer);

    public object CaptureState()
    {
      return new QuestStatusRecord()
      {
        QuestName = _quest.Title,
        CompletedObjectives = _completedObjectiveRefers
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
