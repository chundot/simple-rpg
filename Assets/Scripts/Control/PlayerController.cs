using System;
using System.Linq;
using RPG.Combat;
using RPG.Manager;
using RPG.Movement;
using RPG.Resx;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

namespace RPG.Control
{
  public class PlayerController : MonoBehaviour
  {
    [SerializeField] CursorMapping[] _mappings;
    [SerializeField] float _maxNavPathLength = 40, _maxNavProjectionDistance = 1;
    Mover _mover;
    Fighter _fighter;
    Health _health;
    static Ray MouseRay { get => Camera.main.ScreenPointToRay(Input.mousePosition); }
    public Fighter Fighter { get => _fighter; }
    public Mover Mover { get => _mover; }
    void Awake()
    {
      SceneMgr.Self.Player = transform;
      _mover = GetComponent<Mover>();
      _fighter = GetComponent<Fighter>();
      _health = GetComponent<Health>();
    }

    void Update()
    {
      if (InteractWithUI()) return;
      if (_health.IsDead)
      {
        SetCursor(CursorType.None);
        return;
      }
      if (InteractWithComponent()) return;
      if (InteractWithMovement()) return;
      SetCursor(CursorType.None);
    }

    bool InteractWithComponent()
    {
      var hits = RaycastSorted();
      foreach (var hit in hits)
      {
        var raycastbles = hit.transform.GetComponents<IRaycastable>();
        foreach (var raycastable in raycastbles)
        {
          if (raycastable.HandleRaycast(this))
          {
            SetCursor(raycastable.GetCursorType());
            return true;
          }
        }
      }
      return false;
    }

    static RaycastHit[] RaycastSorted()
    {
      var hits = Physics.RaycastAll(MouseRay);
      Array.Sort(hits.Select(h => h.distance).ToArray(), hits);
      return hits;
    }

    bool InteractWithUI()
    {
      if (EventSystem.current.IsPointerOverGameObject())
      {
        SetCursor(CursorType.UI);
        return true;
      }
      return false;
    }

    void SetCursor(CursorType type)
    {
      var mapping = GetCursorMapping(type);
      Cursor.SetCursor(mapping.Texture, mapping.Hotspot, CursorMode.Auto);
    }

    CursorMapping GetCursorMapping(CursorType type) => _mappings.First(m => m.Type == type);

    bool InteractWithMovement()
    {
      //var hasHit = Physics.Raycast(MouseRay, out var hit);
      var hasHit = RaycastNavmesh(out var target);
      if (hasHit)
      {
        if (Input.GetMouseButton(0))
          _mover.StartMoveAction(target);
        SetCursor(CursorType.Movement);
        return true;
      }
      return false;
    }
    bool RaycastNavmesh(out Vector3 target)
    {
      target = new();
      var hasHit = Physics.Raycast(MouseRay, out var hit);
      if (!hasHit) return false;
      var hasCastToNavMesh = NavMesh.SamplePosition(hit.point, out var navMeshHit, _maxNavProjectionDistance, NavMesh.AllAreas);
      if (!hasCastToNavMesh) return false;
      target = navMeshHit.position;
      NavMeshPath path = new();
      bool hasPath = NavMesh.CalculatePath(transform.position, target, NavMesh.AllAreas, path);
      if (!hasPath || path.status != NavMeshPathStatus.PathComplete) return false;
      if (_maxNavPathLength < GetPathLength(path)) return false;
      return true;
    }

    float GetPathLength(NavMeshPath path)
    {
      float total = 0;
      if (path.corners.Length < 2) return total;
      for (int i = 1; i < path.corners.Length; ++i)
        total += Vector3.Distance(path.corners[i], path.corners[i - 1]);
      return total;
    }

    [Serializable]
    struct CursorMapping
    {
      public CursorType Type;
      public Texture2D Texture;
      public Vector2 Hotspot;
    }
  }

}