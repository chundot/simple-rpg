using UnityEngine;

namespace RPG.Inventories
{
  [CreateAssetMenu(menuName = "RPG/InventorySystem/Action Item")]
  public class ActionItem : InventoryItem
  {
    [Tooltip("消耗品, 使用时消失.")]
    [SerializeField] bool _consumable = false;
    public bool IsConsumable => _consumable;

    public virtual void Use(GameObject user)
    {
      Debug.Log("Using action: " + this);
    }
  }
}