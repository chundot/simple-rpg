using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RPG.Inventories;
using UnityEngine;

namespace RPG.Quests
{
  [CreateAssetMenu(fileName = "Quest", menuName = "RPG/Quest", order = 0)]
  public class Quest : ScriptableObject
  {
    [SerializeField] string _title;
    [SerializeField] List<Objective> _objectives;
    [SerializeField] List<Reward> _rewards;
    public string Title => _title;
    public int ObjectiveCount => _objectives.Count;
    public IEnumerable<Objective> Objectives => _objectives;
    public IEnumerable<Reward> Rewards => _rewards;
    public static Quest GetByName(string name)
    {
      return Resources.LoadAll<Quest>("Quests").FirstOrDefault(q => q.Title == name);
    }
    [Serializable]
    public class Reward
    {
      public InventoryItem Item;
      public int Num;
    }
    [Serializable]
    public class Objective
    {
      public string Reference, Description;
    }

    public bool HasObjective(string objectiveRefer) => _objectives.Any(o => o.Reference == objectiveRefer);
  }
}