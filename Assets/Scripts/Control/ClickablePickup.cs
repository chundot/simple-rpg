using RPG.Inventories;
using UnityEngine;

namespace RPG.Control
{
  [RequireComponent(typeof(Pickup))]
  public class ClickablePickup : MonoBehaviour, IRaycastable
  {
    public CursorType CursorType => _pickup.CanBePickedUp ? CursorType.Loot : CursorType.Full;
    Pickup _pickup;
    void Awake()
    {
      _pickup = GetComponent<Pickup>();
    }

    public bool HandleRaycast(PlayerController playerCtrl)
    {
      if (Input.GetMouseButtonDown(0))
        _pickup.PickupItem();
      return true;
    }
  }

}