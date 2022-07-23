using RPG.Combat;
using RPG.Core;
using RPG.Manager;
using RPG.Movement;
using UnityEngine;
namespace RPG.Control
{
  public class PlayerController : MonoBehaviour
  {
    private Mover _mover;
    private Fighter _fighter;
    private Health _health;

    private static Ray MouseRay { get => Camera.main.ScreenPointToRay(Input.mousePosition); }
    void Start()
    {
      _mover = GetComponent<Mover>();
      _fighter = GetComponent<Fighter>();
      _health = GetComponent<Health>();
      SceneMgr.Self.Player = transform;
    }

    void Update()
    {
      if (_health.IsDead) return;
      if (InteractWithCombat()) return;
      if (InteractWithMovement()) return;
    }

    private bool InteractWithCombat()
    {
      var hits = Physics.RaycastAll(MouseRay);
      foreach (var hit in hits)
      {
        var target = hit.transform.GetComponent<CombatTarget>();
        if (target is null) continue;
        if (!_fighter.CanAttack(target.gameObject)) continue;
        if (Input.GetMouseButton(0))
          _fighter.Attack(target.gameObject);
        return true;
      }
      return false;
    }

    private bool InteractWithMovement()
    {
      var hasHit = Physics.Raycast(MouseRay, out var hit);
      if (hasHit)
      {
        if (Input.GetMouseButton(0))
          _mover.StartMoveAction(hit.point);
        return true;
      }
      return false;
    }
  }

}