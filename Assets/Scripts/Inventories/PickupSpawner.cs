using UnityEngine;
using RPG.Saving;

namespace RPG.Inventories
{
  public class PickupSpawner : MonoBehaviour, ISaveable
  {
    [SerializeField] InventoryItem _item = null;
    [SerializeField] int _number = 1;

    void Awake()
    {
      SpawnPickup();
    }

    public Pickup Pickup => GetComponentInChildren<Pickup>();

    public bool IsCollected => !Pickup;

    void SpawnPickup()
    {
      var spawnedPickup = _item.SpawnPickup(transform.position, _number);
      spawnedPickup.transform.SetParent(transform);
    }

    void DestroyPickup()
    {
      if (Pickup)
        Destroy(Pickup.gameObject);
    }

    object ISaveable.CaptureState()
    {
      return IsCollected;
    }

    void ISaveable.RestoreState(object state)
    {
      var shouldBeCollected = (bool)state;

      if (shouldBeCollected && !IsCollected)
        DestroyPickup();

      if (!shouldBeCollected && IsCollected)
        SpawnPickup();
    }
  }
}