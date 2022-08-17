using System;
using System.Collections;
using UnityEngine;

namespace RPG.Abilities.Effects
{
  [CreateAssetMenu(fileName = "Delay Composite Effect", menuName = "RPG/Abilities/Effects/Delay Composite Effect", order = 0)]
  public class DelayCompositeEffect : EffectStartegy
  {
    [SerializeField] float _delay = 0;
    [SerializeField] EffectStartegy[] _delayedEffects;
    [SerializeField] bool _abortIfCancelled = false;
    public override void StartEffect(AbilityData data, Action finished)
    {
      data.StartCoroutine(DelayedEffects(data, finished));
    }

    IEnumerator DelayedEffects(AbilityData data, Action finished)
    {
      yield return new WaitForSeconds(_delay);
      if (_abortIfCancelled && data.IsCancelled) yield break;
      foreach (var effect in _delayedEffects)
        effect.StartEffect(data, finished);
    }
  }

}