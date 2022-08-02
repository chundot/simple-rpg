using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
  public class WeaponPickup : MonoBehaviour
  {
    Collider _collider;
    void Awake()
    {
      _collider = GetComponent<Collider>();
    }
    [SerializeField] Weapon _weapon;
    void OnTriggerEnter(Collider other)
    {
      if (other.CompareTag("Player"))
      {
        other.GetComponent<Fighter>().EquipWeapon(_weapon);
        StartCoroutine(HideForSec());
      }
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
  }
}