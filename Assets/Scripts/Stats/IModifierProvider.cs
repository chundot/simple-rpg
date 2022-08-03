using System.Collections;
using System.Collections.Generic;

namespace RPG.Stats
{
  public interface IModifierProvider
  {
    IEnumerable<float> GetAdditiveModifier(StatsEnum stat);
    IEnumerable<float> GetPercentModifier(StatsEnum stat);
  }
}