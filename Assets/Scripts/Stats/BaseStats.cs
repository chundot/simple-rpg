using System;
using RPG.Utils;
using UnityEngine;

namespace RPG.Stats
{
  public class BaseStats : MonoBehaviour
  {
    [Range(1, 99)]
    [SerializeField] int _level = 1;
    [SerializeField] CharClass _charClass;
    [SerializeField] Progression _progression;
    [SerializeField] GameObject _levelUpFX;
    [SerializeField] bool _canUseModifier = false;
    LazyValue<int> _curLevel;
    Experience _exp;
    public float Dmg
    {
      get => GetStat(StatsEnum.Damage);
    }

    public event Action OnLevelUp;
    public float MaxHealth { get => GetStat(StatsEnum.Health); }
    public float XPReward { get => GetStat(StatsEnum.XPReward); }
    public float MaxMana { get => GetStat(StatsEnum.MaxMana); }
    public float ManaRegen { get => GetStat(StatsEnum.ManaRegen); }
    public int Level { get => _curLevel.Value; }
    public int CalculatedLevel
    {
      get
      {
        if (!TryGetComponent<Experience>(out var xp)) return _level;
        var curXP = xp.CurNum;
        var maxLevel = _progression.GetLevels(_charClass);
        for (int level = 1; level <= maxLevel; ++level)
        {
          var xpToLevelUp = _progression.GetStat(_charClass, StatsEnum.XPToLevelUp, level);
          if (xpToLevelUp > curXP)
            return level;
        }
        return maxLevel + 1;
      }
    }
    void Awake()
    {
      _exp = GetComponent<Experience>();
      _curLevel = new(() => CalculatedLevel);
    }
    void OnEnable()
    {
      if (_exp != null)
        _exp.OnXPGained += UpdateLevel;
    }
    void OnDisable()
    {
      if (_exp != null)
        _exp.OnXPGained -= UpdateLevel;
    }
    void UpdateLevel()
    {
      int newLevel = CalculatedLevel;
      if (newLevel > _curLevel.Value)
      {
        _curLevel.Value = newLevel;
        LevelUp();
      }
    }
    void LevelUp()
    {
      OnLevelUp?.Invoke();
      if (_levelUpFX != null)
        Instantiate(_levelUpFX, transform);
    }
    float GetStat(StatsEnum stat) => (_progression.GetStat(_charClass, stat, _curLevel.Value) + GetAddtiveModifier(stat)) * GetPercentModifier(stat);

    float GetPercentModifier(StatsEnum stat)
    {
      float res = 100;
      if (_canUseModifier)
        foreach (var component in GetComponents<IModifierProvider>())
          foreach (var mod in component.GetPercentModifier(stat))
            res += mod;
      return res / 100;
    }

    float GetAddtiveModifier(StatsEnum stat)
    {
      float sum = 0;
      if (_canUseModifier)
        foreach (var component in GetComponents<IModifierProvider>())
          foreach (var mod in component.GetAdditiveModifier(stat))
            sum += mod;
      return sum;
    }
  }
}