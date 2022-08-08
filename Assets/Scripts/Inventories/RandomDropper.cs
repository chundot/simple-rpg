using RPG.Stats;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Inventories
{
  public class RandomDropper : ItemDropper
  {
    [Tooltip("掉落物零散距离.")]
    [SerializeField] float _scatterDistance = 1f;
    [SerializeField] DropLibrary _dropLib;
    const int ATTEMPTS = 10;
    protected override Vector3 GetDropLocation()
    {
      for (int i = 0; i < ATTEMPTS; ++i)
      {
        var rndPos = transform.position + Random.insideUnitSphere * _scatterDistance;
        if (NavMesh.SamplePosition(rndPos, out var hit, .1f, NavMesh.AllAreas))
          return hit.position;
      }
      return transform.position;
    }
    public void RndDrop()
    {
      var stats = GetComponent<BaseStats>();
      foreach (var drop in _dropLib.GetRndDrops(stats.Level))
        DropItem(drop.Item, drop.Num);
    }
  }
}
