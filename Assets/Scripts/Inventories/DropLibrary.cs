using System.Collections.Generic;
using UnityEngine;

namespace RPG.Inventories
{
  [CreateAssetMenu(menuName = "RPG/Inventory/Drop Library")]
  public class DropLibrary : ScriptableObject
  {
    [SerializeField] DropCfg[] _potentialDrops;
    [SerializeField] float[] _dropChance;
    [SerializeField] int[] _minDrops, _maxDrops;

    int RndNumOfDrops(int level) => Random.Range(GetByLevel(_minDrops, level), GetByLevel(_maxDrops, level));

    bool IsRnd(int level) => Random.Range(0, 100) < GetByLevel(_dropChance, level);

    [System.Serializable]
    class DropCfg
    {
      public InventoryItem Item;
      public float[] Chance;
      public int[] MinNum, MaxNum;
      public int GetRndNum(int level) => Item.IsStackable ? Random.Range(GetByLevel(MinNum, level), GetByLevel(MaxNum, level) + 1) : 1;
    }
    public struct Dropped
    {
      public InventoryItem Item;
      public int Num;
    }
    public IEnumerable<Dropped> GetRndDrops(int level)
    {
      if (!IsRnd(level))
        yield break;
      for (int i = 0; i < RndNumOfDrops(level); ++i)
        yield return GetRndDrop(level);
    }
    Dropped GetRndDrop(int level)
    {
      var drop = GetRndItem(level);
      Dropped res = new()
      {
        Item = drop.Item,
        Num = drop.GetRndNum(level)
      };
      return res;
    }
    DropCfg GetRndItem(int level)
    {
      var rndRoll = Random.Range(0, GetTotalChance(level));
      float chanceTotal = 0;
      foreach (var drop in _potentialDrops)
      {
        chanceTotal += GetByLevel(drop.Chance, level);
        if (chanceTotal > rndRoll)
          return drop;
      }
      return null;
    }

    public float GetTotalChance(int level)
    {
      float total = 0;
      foreach (var drop in _potentialDrops)
        total += GetByLevel(drop.Chance, level);
      return total;
    }
    static T GetByLevel<T>(T[] values, int level)
    {
      if (values.Length == 0)
        return default;
      if (level > values.Length)
        return values[^1];
      if (level <= 0) return default;
      return values[level - 1];
    }
  }

}