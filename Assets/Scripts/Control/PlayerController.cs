using RPG.Combat;
using RPG.Movement;
using UnityEngine;
namespace RPG.Control
{
  public class PlayerController : MonoBehaviour
  {
    private Mover _mover;
    private Fighter _fighter;

    private static Ray MouseRay { get => Camera.main.ScreenPointToRay(Input.mousePosition); }
    void Start()
    {
      _mover = GetComponent<Mover>();
      _fighter = GetComponent<Fighter>();
    }

    void Update()
    {
      if (InteractWithCombat()) return;
      if (InteractWithMovement()) return;
    }

    private bool InteractWithCombat()
    {
      var hits = Physics.RaycastAll(MouseRay);
      foreach (var hit in hits)
      {
        var target = hit.transform.GetComponent<CombatTarget>();
        if (!_fighter.CanAttack(target)) continue;
        if (Input.GetMouseButton(0))
          _fighter.Attack(target);
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