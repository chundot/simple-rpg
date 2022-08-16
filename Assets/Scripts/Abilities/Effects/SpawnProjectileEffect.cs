using System;
using System.Collections;
using RPG.Attributes;
using RPG.Combat;
using UnityEngine;

namespace RPG.Abilities.Effects
{
  [CreateAssetMenu(fileName = "Spawn Projectile Effect", menuName = "RPG/Abilities/Effects/Spawn Projectile Effect", order = 0)]
  public class SpawnProjectileEffect : EffectStartegy
  {
    [SerializeField] Projectile _projectilePrefab;
    [SerializeField] float _dmg = 10, _destoryDelay = -1;
    [SerializeField] bool _rightHand = false;
    public override void StartEffect(AbilityData data, Action finished)
    {
      var fighter = data.User.GetComponent<Fighter>();
      foreach (var target in data.Targets)
      {
        if (target.TryGetComponent(out Health health))
        {
          var instance = Instantiate(_projectilePrefab);
          instance.transform.position = fighter.GetHandTransform(_rightHand).position;
          instance.Init(_dmg, health, data.User);
        }
      }
      finished();
    }
  }

}