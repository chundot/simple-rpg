using RPG.Attributes;
using RPG.Core;
using RPG.Inventories;
using UnityEngine;

namespace RPG.Abilities
{
  [CreateAssetMenu(fileName = "Ability", menuName = "RPG/Abilities/Ability", order = 0)]
  public class Ability : ActionItem
  {
    [SerializeField] TargetingStrategy _targetingStrategy;
    [SerializeField] FilterStrategy[] _filterStrategies;
    [SerializeField] EffectStartegy[] _effectStrategies;
    [SerializeField] float _manaCost;
    public override bool Use(GameObject user)
    {
      if (_manaCost > user.GetComponent<Mana>().CurMana)
        return false;
      if (user.GetComponent<CooldownStore>().GetTimeRemaining(this) > 0)
        return false;
      AbilityData data = new(user);
      user.GetComponent<ActionScheduler>().StartAction(data);
      _targetingStrategy.StartTargeting(data, () =>
      {
        TargetAcquired(data);
      });
      return true;
    }

    void TargetAcquired(AbilityData data)
    {
      if (data.IsCancelled) return;
      if (!data.User.GetComponent<Mana>().UserMana(_manaCost))
        return;
      data.User.GetComponent<CooldownStore>().StartCooldown(this, Cooldown);
      foreach (var filter in _filterStrategies)
        data.Targets = filter.Filter(data.Targets);

      foreach (var effect in _effectStrategies)
        effect.StartEffect(data, EffectFinished);
    }

    void EffectFinished()
    {
    }
  }
}