using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.Stats
{
  [CreateAssetMenu(fileName = "Progression", menuName = "RPG/Progression", order = 0)]
  public class Progression : ScriptableObject
  {
    [SerializeField] ProgressionCharClass[] _charClass;
    Dictionary<CharClass, Dictionary<StatsEnum, float[]>> _lookup;

    public float GetDamage(CharClass charClass, int level) => GetStat(charClass, StatsEnum.Damage, level);

    public int GetLevels(CharClass charClass, StatsEnum stat = StatsEnum.XPToLevelUp)
    {
      BuildLookup();
      var levels = _lookup[charClass][stat];
      return levels.Length;
    }
    public float GetStat(CharClass charClass, StatsEnum stat, int level)
    {
      BuildLookup();
      if (!_lookup[charClass].ContainsKey(stat))
        return 0;
      var levels = _lookup[charClass][stat];
      if (levels.Length == 0)
        return 0;
      return levels[Mathf.Min(level, levels.Length) - 1];
    }

    void BuildLookup()
    {
      if (_lookup != null) return;
      _lookup = _charClass.ToDictionary(cc => cc.CharClass, cc => cc.Stats.ToDictionary(s => s.Stat, s => s.Levels));
    }

    [Serializable]
    class ProgressionCharClass
    {
      public CharClass CharClass;
      public ProgressionStat[] Stats;
      //public float[] Health;
    }
    [Serializable]
    class ProgressionStat
    {
      public StatsEnum Stat;
      public float[] Levels;
    }
  }
}