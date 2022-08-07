using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Inventories
{
  public class RandomDropper : ItemDropper
  {
    [Tooltip("掉落物零散距离.")]
    [SerializeField] float _scatterDistance = 1f;
    [SerializeField] InventoryItem[] _dropLib;
    [SerializeField] int _numOfDrops = 2;
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
      for (int i = 0; i < _numOfDrops; ++i)
      {
        var item = _dropLib[Random.Range(0, _dropLib.Length)];
        DropItem(item, 1);
      }
    }
  }
}
