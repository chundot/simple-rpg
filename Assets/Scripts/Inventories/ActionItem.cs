using System;
using UnityEngine;

namespace RPG.Inventories
{
  [CreateAssetMenu(menuName = "RPG/InventorySystem/Action Item")]
  public class ActionItem : InventoryItem
  {
    [Tooltip("消耗品, 使用时消失.")]
    [SerializeField] bool _consumable = false;
    [SerializeField] protected float Cooldown;
    public bool IsConsumable => _consumable;

    public virtual bool Use(GameObject user)
    {
      return true;
    }
  }
}