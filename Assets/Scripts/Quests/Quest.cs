using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.Quests
{
  [CreateAssetMenu(fileName = "Quest", menuName = "RPG/Quest", order = 0)]
  public class Quest : ScriptableObject
  {
    [SerializeField] string _title;
    [SerializeField] string[] _objectives;
    public string Title => _title;
    public int ObjectiveCount => _objectives.Length;
    public IEnumerable<string> Objectives => _objectives;
    public static Quest GetByName(string name)
    {
      return Resources.LoadAll<Quest>("Quests").FirstOrDefault(q => q.Title == name);
    }
  }
}