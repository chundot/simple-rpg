using System;
using RPG.Control;
using UnityEngine;

namespace RPG.Abilities.Targeting
{
  [CreateAssetMenu(fileName = "Directional Targeting", menuName = "RPG/Abilities/Targeting/Directional Targeting", order = 0)]
  public class DirectionalTargeting : TargetingStrategy
  {
    [SerializeField] LayerMask _layerMask;
    [SerializeField] float _groundOffset = .3f;
    public override void StartTargeting(AbilityData data, Action finished)
    {
      var ray = PlayerController.MouseRay;
      if (Physics.Raycast(ray, out var hit, 1000, _layerMask))
        data.TargetPoint = hit.point + _groundOffset / ray.direction.y * ray.direction;
      finished();
    }
  }
}
