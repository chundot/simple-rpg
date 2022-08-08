using System.Collections.Generic;
using UnityEngine;

namespace RPG.Inventories
{
  public abstract class InventoryItem : ScriptableObject, ISerializationCallbackReceiver
  {
    [Tooltip("自动生成的UUID.")]
    [SerializeField] string _itemID = null;
    [Tooltip("道具名.")]
    [SerializeField] string _displayName = null;
    [Tooltip("描述.")]
    [SerializeField][TextArea] string _description = null;
    [Tooltip("背包里的图标.")]
    [SerializeField] Sprite _icon = null;
    [Tooltip("掉落时的预制体.")]
    [SerializeField] Pickup _pickup = null;
    [Tooltip("是否可堆叠.")]
    [SerializeField] bool _stackable = false;

    static Dictionary<string, InventoryItem> _itemLookupCache;
    public Sprite Icon => _icon;
    public string ItemID => _itemID;

    public bool IsStackable => _stackable;

    public string DisplayName => _displayName;

    public string Description => _description;
    public static InventoryItem GetFromID(string itemID)
    {
      if (_itemLookupCache == null)
      {
        _itemLookupCache = new();
        var itemList = Resources.LoadAll<InventoryItem>("");
        foreach (var item in itemList)
        {
          if (item._itemID is null)
          {
            Debug.Log(item);
            continue;
          }
          if (_itemLookupCache.ContainsKey(item._itemID))
          {
            Debug.LogError(string.Format("duplicate RPG.UI.InventorySystem ID for objects: {0} and {1}", _itemLookupCache[item._itemID], item));
            continue;
          }

          _itemLookupCache[item._itemID] = item;
        }
      }

      if (itemID == null || !_itemLookupCache.ContainsKey(itemID)) return null;
      return _itemLookupCache[itemID];
    }

    public Pickup SpawnPickup(Vector3 position, int number)
    {
      var pickup = Instantiate(_pickup);
      pickup.transform.position = position;
      pickup.Setup(this, number);
      return pickup;
    }


    void ISerializationCallbackReceiver.OnBeforeSerialize()
    {
      // Generate and save a new UUID if this is blank.
      if (string.IsNullOrWhiteSpace(_itemID))
        _itemID = System.Guid.NewGuid().ToString();
    }

    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
      // Require by the ISerializationCallbackReceiver
    }
  }
}
