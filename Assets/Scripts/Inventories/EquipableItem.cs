using UnityEngine;

namespace RPG.Inventories
{
  [CreateAssetMenu(menuName = "RPG/InventorySystem/Equipable Item")]
  public class EquipableItem : InventoryItem
  {
    [Tooltip("装备位置.")]
    [SerializeField] EquipLocation _allowedEquipLocation = EquipLocation.Weapon;

    public EquipLocation AllowedEquipLocation => _allowedEquipLocation;
  }
}