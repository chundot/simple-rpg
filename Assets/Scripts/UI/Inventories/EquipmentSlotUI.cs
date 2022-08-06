using UnityEngine;
using RPG.Core.UI.Dragging;
using RPG.Inventories;

namespace RPG.UI.Inventories
{
  public class EquipmentSlotUI : MonoBehaviour, IItemHolder, IDragContainer<InventoryItem>
  {
    // CONFIG DATA

    [SerializeField] InventoryItemIcon icon = null;
    [SerializeField] EquipLocation equipLocation = EquipLocation.Weapon;

    Equipment playerEquipment;
    public InventoryItem Item => playerEquipment.GetItemInSlot(equipLocation);
    public int Number => Item != null ? 1 : 0;

    void Awake()
    {
      var player = GameObject.FindGameObjectWithTag("Player");
      playerEquipment = player.GetComponent<Equipment>();
      playerEquipment.EquipmentUpdated += RedrawUI;
    }

    void Start()
    {
      RedrawUI();
    }

    public int MaxAcceptable(InventoryItem item)
    {
      EquipableItem equipableItem = item as EquipableItem;
      if (!equipableItem) return 0;
      if (equipableItem.AllowedEquipLocation != equipLocation) return 0;
      if (Item) return 0;

      return 1;
    }

    public void AddItems(InventoryItem item, int number)
    {
      playerEquipment.AddItem(equipLocation, (EquipableItem)item);
    }

    public void RemoveItems(int number)
    {
      playerEquipment.RemoveItem(equipLocation);
    }

    void RedrawUI()
    {
      icon.SetItem(playerEquipment.GetItemInSlot(equipLocation));
    }
  }
}