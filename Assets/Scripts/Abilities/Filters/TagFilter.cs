using System.Collections.Generic;
using UnityEngine;

namespace RPG.Abilities.Filters
{
  [CreateAssetMenu(fileName = "Tag Filter", menuName = "RPG/Abilities/Filters/Tag Filter", order = 0)]
  public class TagFilter : FilterStrategy
  {
    [SerializeField] string _tag;
    public override IEnumerable<GameObject> Filter(IEnumerable<GameObject> objsToFilter)
    {
      foreach (var obj in objsToFilter)
      {
        if (_tag == "" || obj.CompareTag(_tag))
          yield return obj;
      }
    }
  }
}