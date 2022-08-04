using System.Collections;
using RPG.Control;
using RPG.Resx;
using UnityEngine;

namespace RPG.Combat
{
  public class WeaponPickup : MonoBehaviour, IRaycastable
  {
    [SerializeField] WeaponConfig _weapon;
    [SerializeField] float _healthRegen = 0;
    Collider _collider;
    void Awake()
    {
      _collider = GetComponent<Collider>();
    }
    void OnTriggerEnter(Collider other)
    {
      if (other.CompareTag("Player"))
      {
        Pickup(other.GetComponent<Fighter>());
      }
    }

    void Pickup(Fighter fighter)
    {
      if (_weapon != null)
        fighter.EquipWeapon(_weapon);
      else if (_healthRegen > 0)
        fighter.GetComponent<Health>().Heal(_healthRegen);
      StartCoroutine(HideForSec());
    }

    IEnumerator HideForSec(float time = 10f)
    {
      ChangePickUp(false);
      yield return new WaitForSeconds(time);
      ChangePickUp(true);
    }
    void ChangePickUp(bool show)
    {
      _collider.enabled = show;
      foreach (Transform child in transform)
        child.gameObject.SetActive(show);
    }

    public bool HandleRaycast(PlayerController playerCtrl)
    {
      if (Input.GetMouseButtonDown(0))
        Pickup(playerCtrl.GetComponent<Fighter>());
      return true;
    }

    public CursorType GetCursorType()
    {
      return CursorType.Loot;
    }
  }
}