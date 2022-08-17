using System;
using System.Collections.Generic;
using System.Linq;
using RPG.Saving;
using UnityEngine;

namespace RPG.Stats
{
  public class TraitStore : MonoBehaviour, IModifierProvider, ISaveable
  {
    [SerializeField] TraitBonus[] _bonusCfgs;
    Dictionary<Trait, int> _assignedPoints = new(), _stagedPoints = new();
    Dictionary<StatsEnum, Dictionary<Trait, float>> _additiveCache, _percentageCache;
    BaseStats _stats;
    public int UnassignedPoints => AssignablePoints - TotalProposedPoints;
    public int AssignablePoints => (int)_stats.GetStat(StatsEnum.TotalTraitPoints);
    public int TotalProposedPoints => _assignedPoints.Sum(p => p.Value) + _stagedPoints.Sum(p => p.Value);
    public event Action OnPointsChanged;
    void Awake()
    {
      _stats = GetComponent<BaseStats>();
      BuildBonusCfgCache();
    }

    void BuildBonusCfgCache()
    {
      _additiveCache = new();
      _percentageCache = new();
      foreach (var cfg in _bonusCfgs)
      {
        if (cfg.AdditiveBonusPerPoint > 0)
        {
          if (!_additiveCache.ContainsKey(cfg.Stats)) _additiveCache[cfg.Stats] = new();
          _additiveCache[cfg.Stats][cfg.Trait] = cfg.AdditiveBonusPerPoint;
        }
        if (cfg.PercentageBonusPerPoint > 0)
        {
          if (!_percentageCache.ContainsKey(cfg.Stats)) _percentageCache[cfg.Stats] = new();
          _percentageCache[cfg.Stats][cfg.Trait] = cfg.PercentageBonusPerPoint;
        }
      }
    }

    public int GetProposedPoints(Trait trait)
    {
      return GetPoints(trait) + GetStagedPoints(trait);
    }
    public int GetPoints(Trait trait)
    {
      return _assignedPoints.ContainsKey(trait) ? _assignedPoints[trait] : 0;
    }
    public int GetStagedPoints(Trait trait)
    {
      return _stagedPoints.ContainsKey(trait) ? _stagedPoints[trait] : 0;
    }
    public void AssignPoints(Trait trait, int points)
    {
      if (!CanAssignPoints(trait, points)) return;
      _stagedPoints[trait] = GetStagedPoints(trait) + points;
      OnPointsChanged();
    }
    public bool CanAssignPoints(Trait trait, int points)
    {
      if (GetStagedPoints(trait) + points < 0) return false;
      return UnassignedPoints >= points;
    }
    public void Commit()
    {
      foreach (var trait in _stagedPoints.Keys)
        _assignedPoints[trait] = GetProposedPoints(trait);
      _stagedPoints.Clear();
      OnPointsChanged();
    }

    public IEnumerable<float> GetAdditiveModifier(StatsEnum stat)
    {
      if (!_additiveCache.ContainsKey(stat)) yield break;
      foreach (var pair in _additiveCache[stat])
        yield return pair.Value * GetPoints(pair.Key);
    }

    public IEnumerable<float> GetPercentModifier(StatsEnum stat)
    {
      if (!_percentageCache.ContainsKey(stat)) yield break;
      foreach (var pair in _percentageCache[stat])
        yield return pair.Value * GetPoints(pair.Key);
    }

    public object CaptureState()
    {
      return _assignedPoints;
    }

    public void RestoreState(object state)
    {
      _assignedPoints = state as Dictionary<Trait, int>;
    }

    [Serializable]
    class TraitBonus
    {
      public Trait Trait;
      public StatsEnum Stats;
      public float AdditiveBonusPerPoint = 0;
      public float PercentageBonusPerPoint = 0;
    }
  }
}
