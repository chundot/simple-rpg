using System;
using RPG.Attributes;
using RPG.Combat;
using UnityEngine;

namespace RPG.Abilities.Effects
{
  [CreateAssetMenu(fileName = "Spawn Projectile Effect", menuName = "RPG/Abilities/Effects/Spawn Projectile Effect", order = 0)]
  public class SpawnProjectileEffect : EffectStartegy
  {
    [SerializeField] Projectile _projectilePrefab;
    [SerializeField] float _dmg = 10;
    [SerializeField] bool _rightHand = false, _usePoint = false;
    public override void StartEffect(AbilityData data, Action finished)
    {
      var fighter = data.User.GetComponent<Fighter>();
      if (_usePoint)
        SpawnProjectilesUsePoint(data, fighter);
      else
        SpawnProjectiles(data, fighter);
      finished();
    }

    void SpawnProjectilesUsePoint(AbilityData data, Fighter fighter)
    {
      var instance = Instantiate(_projectilePrefab);
      instance.transform.position = fighter.GetHandTransform(_rightHand).position;
      instance.Init(data.TargetPoint, data.User, _dmg);
    }

    void SpawnProjectiles(AbilityData data, Fighter fighter)
    {
      foreach (var target in data.Targets)
      {
        if (target.TryGetComponent(out Health health))
        {
          var instance = Instantiate(_projectilePrefab);
          instance.transform.position = fighter.GetHandTransform(_rightHand).position;
          instance.Init(_dmg, health, data.User);
        }
      }
    }
  }

}