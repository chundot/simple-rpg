using System;
using RPG.Resx;
using UnityEngine;

namespace RPG.Inventories
{
  [CreateAssetMenu(menuName = "RPG/InventorySystem/Action Item")]
  public class ActionItem : InventoryItem
  {
    [Tooltip("消耗品, 使用时消失.")]
    [SerializeField] bool _consumable = false;
    [SerializeField] ActionEffect[] _actionEffects;
    public bool IsConsumable => _consumable;

    public virtual void Use(GameObject user)
    {
      foreach (var fx in _actionEffects)
      {
        if (fx.Effect is InventoryEffect.HealthRegen)
          if (user.TryGetComponent(out Health health))
            if (fx.Method is EffectMethod.Add)
              health.Heal(fx.Amount[0]);
            else if (fx.Method is EffectMethod.Multiply)
              health.HealByPercentage(fx.Amount[0] * 100);
      }
    }
    [Serializable]
    struct ActionEffect
    {
      public InventoryEffect Effect;
      public EffectMethod Method;
      public float[] Amount;
    }
  }
}