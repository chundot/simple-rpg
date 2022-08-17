using System;
using System.Collections;
using UnityEngine;

namespace RPG.Abilities.Effects
{
  [CreateAssetMenu(fileName = "Spawn Target Prefab Effect", menuName = "RPG/Abilities/Effects/Spawn Target Prefab Effect", order = 0)]
  public class SpawnTargetPrefabEffect : EffectStartegy
  {
    [SerializeField] Transform _prefabToSpawn;
    [SerializeField] float _destoryDelay = -1;
    public override void StartEffect(AbilityData data, Action finished)
    {
      data.StartCoroutine(Effect(data, finished));
    }
    IEnumerator Effect(AbilityData data, Action finished)
    {
      var instance = Instantiate(_prefabToSpawn);
      instance.position = data.TargetPoint;
      if (_destoryDelay > 0)
      {
        yield return new WaitForSeconds(_destoryDelay);
        Destroy(instance.gameObject);
      }
      finished();
    }
  }

}