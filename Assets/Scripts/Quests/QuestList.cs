using System;
using System.Collections.Generic;
using System.Linq;
using RPG.Core;
using RPG.Inventories;
using RPG.Manager;
using RPG.Saving;
using UnityEngine;

namespace RPG.Quests
{
  public class QuestList : MonoBehaviour, ISaveable, IPredicateEvaluator
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
    public QuestStatus GetQuestStatus(string title) => _statuses.FirstOrDefault(q => q.Quest.Title == title);
    public QuestStatus GetQuestStatus(Quest quest) => _statuses.FirstOrDefault(q => q.Quest == quest);
    public void CompleteObjective(Quest quest, string objectiveRefer)
    {
      var status = GetQuestStatus(quest);
      if (status == null) return;
      status.CompleteObjective(objectiveRefer);
      if (status.IsCompleted)
        GiveReward(quest);
      OnUpdate?.Invoke();
    }

    void GiveReward(Quest quest)
    {
      var inventory = GetComponent<Inventory>();
      var dropper = GetComponent<ItemDropper>();
      foreach (var reward in quest.Rewards)
      {
        if (!inventory.AddToFirstEmptySlot(reward.Item, reward.Num))
          dropper.DropItem(reward.Item, reward.Num);
      }
    }

    bool HasQuest(string title) => GetQuestStatus(title) != null;
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

    public bool? Evaluate(string predicate, string[] parameters)
    {
      if (predicate == "HasEnemy")
        return SceneMgr.Self.HasEnemy;
      else if (predicate == "HasQuest")
      {
        if (GetQuestStatus(parameters[0]) is not null)
          return true;
        else return false;
      }
      else if (predicate == "HasCompletedQuest")
      {
        if (GetQuestStatus(parameters[0]) is QuestStatus status)
          return status.IsCompleted;
        else return false;
      }
      return null;
    }
  }
}