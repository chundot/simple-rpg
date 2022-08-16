using RPG.Attributes;
using RPG.Control;
using UnityEngine;

namespace RPG.Combat
{
  [RequireComponent(typeof(Health))]
  public class CombatTarget : MonoBehaviour, IRaycastable
  {
    public CursorType CursorType => CursorType.Attack;

    public bool HandleRaycast(PlayerController playerCtrl)
    {
      if (!enabled) return false;
      if (!playerCtrl.Fighter.CanAttack(gameObject))
        return false;
      if (Input.GetMouseButton(0))
        playerCtrl.Fighter.Attack(gameObject);
      return true;
    }
  }

}