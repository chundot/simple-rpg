using System;
using System.Collections.Generic;
using RPG.Inventories;
using UnityEngine;

namespace RPG.Abilities
{
  public class CooldownStore : MonoBehaviour
  {
    readonly Dictionary<InventoryItem, float> _timers = new(), _initTimers = new();
    readonly List<InventoryItem> _keys = new();
    void Update()
    {
      _keys.Clear();
      _keys.AddRange(_timers.Keys);
      foreach (var ability in _keys)
      {
        _timers[ability] -= Time.deltaTime;
        if (_timers[ability] < 0)
        {
          _timers.Remove(ability);
          _initTimers.Remove(ability);
        }
      }
    }
    public void StartCooldown(InventoryItem ability, float cooldown)
    {
      _timers[ability] = cooldown;
      _initTimers[ability] = cooldown;
    }

    public float GetTimeRemaining(InventoryItem ability)
    {
      return _timers.ContainsKey(ability) ? _timers[ability] : 0;
    }

    public float GetFractionRemaining(InventoryItem item)
    {
      return _timers.ContainsKey(item) ? _timers[item] / _initTimers[item] : 0;
    }
  }

}