using System.Collections.Generic;
using RPG.Stats;

namespace RPG.Inventories
{
  public class StatEquipment : Equipment, IModifierProvider
  {
    public IEnumerable<float> GetAdditiveModifier(StatsEnum stat)
    {
      foreach (var slot in AllPopulatedSlots)
      {
        if (GetItemInSlot(slot) is not IModifierProvider item) continue;
        foreach (var mod in item.GetAdditiveModifier(stat))
          yield return mod;
      }
    }

    public IEnumerable<float> GetPercentModifier(StatsEnum stat)
    {
      foreach (var slot in AllPopulatedSlots)
      {
        if (GetItemInSlot(slot) is not IModifierProvider item) continue;
        foreach (var mod in item.GetPercentModifier(stat))
          yield return mod;
      }
    }
  }
}
