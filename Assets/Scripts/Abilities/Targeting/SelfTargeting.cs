using System;
using UnityEngine;

namespace RPG.Abilities.Targeting
{
  [CreateAssetMenu(fileName = "Self Targeting", menuName = "RPG/Abilities/Targeting/Self Targeting", order = 0)]
  public class SelfTargeting : TargetingStrategy
  {
    public override void StartTargeting(AbilityData data, Action finished)
    {
      data.Targets = new[] { data.User };
      data.TargetPoint = data.User.transform.position;
      finished();
    }
  }
}
