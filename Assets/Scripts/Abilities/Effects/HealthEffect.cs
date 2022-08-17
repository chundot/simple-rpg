using System;
using RPG.Attributes;
using UnityEngine;

namespace RPG.Abilities.Effects
{
  [CreateAssetMenu(fileName = "Health Effect", menuName = "RPG/Abilities/Effects/Health Effect", order = 0)]
  public class HealthEffect : EffectStartegy
  {
    [SerializeField] float _healthChange, _healthChangePercentage;
    [SerializeField] bool _heal;
    public override void StartEffect(AbilityData data, Action finished)
    {
      foreach (var target in data.Targets)
      {
        if (target.TryGetComponent(out Health health))
        {
          if (!_heal)
          {
            if (_healthChange > 0)
              health.TakeDamage(data.User, _healthChange);
            if (_healthChangePercentage > 0)
              health.TakeDmgByPercentage(data.User, _healthChangePercentage);
          }
          else
          {
            if (_healthChange > 0)
              health.Heal(_healthChange);
            if (_healthChangePercentage > 0)
              health.HealByPercentage(_healthChangePercentage);
          }
        }
      }
    }
  }

}