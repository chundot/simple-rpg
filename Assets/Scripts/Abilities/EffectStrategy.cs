using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Abilities
{
  public abstract class EffectStartegy : ScriptableObject
  {
    public abstract void StartEffect(AbilityData data, Action finished);
  }
}
