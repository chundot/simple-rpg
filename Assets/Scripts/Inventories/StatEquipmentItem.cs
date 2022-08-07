using System;
using System.Collections.Generic;
using RPG.Stats;
using UnityEngine;

namespace RPG.Inventories
{
  [CreateAssetMenu(menuName = "RPG/Inventory/Equipable Item")]
  public class StatEquipmentItem : EquipableItem, IModifierProvider
  {
    [SerializeField]
    Modifier[] _addtiveModifier;
    [SerializeField]
    Modifier[] _percentageModifier;
    [Serializable]
    struct Modifier
    {
      public StatsEnum Stat;
      public float Value;
    }

    public IEnumerable<float> GetAdditiveModifier(StatsEnum stat)
    {
      foreach (var mod in _addtiveModifier)
        if (mod.Stat == stat)
          yield return mod.Value;
    }

    public IEnumerable<float> GetPercentModifier(StatsEnum stat)
    {
      foreach (var mod in _percentageModifier)
        if (mod.Stat == stat)
          yield return mod.Value;
    }
  }
}