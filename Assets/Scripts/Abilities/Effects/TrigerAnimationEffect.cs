using System;
using UnityEngine;

namespace RPG.Abilities.Effects
{
  [CreateAssetMenu(fileName = "TrigerAnimationEffect", menuName = "RPG/Abilities/Effects/Triger Animation Effect", order = 0)]
  public class TrigerAnimationEffect : EffectStartegy
  {
    [SerializeField] string _triggerName = "ability1";
    [SerializeField] bool _lookAtTarget = true;
    public override void StartEffect(AbilityData data, Action finished)
    {
      data.User.GetComponent<Animator>().SetTrigger(_triggerName);
      if (_lookAtTarget) data.User.transform.LookAt(data.TargetPoint);
    }
  }

}