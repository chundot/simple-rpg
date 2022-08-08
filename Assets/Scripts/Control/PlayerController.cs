using System;
using System.Linq;
using RPG.Combat;
using RPG.Inventories;
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
    [SerializeField] float _maxNavProjectionDistance = 1, _raycastRadius = 1;
    Mover _mover;
    Fighter _fighter;
    Health _health;
    ActionStore _actionStore;
    bool _isDraggingUI = false;
    static Ray MouseRay { get => Camera.main.ScreenPointToRay(Input.mousePosition); }
    public Fighter Fighter { get => _fighter; }
    public Mover Mover { get => _mover; }
    void Awake()
    {
      SceneMgr.Self.Player = transform;
      _mover = GetComponent<Mover>();
      _fighter = GetComponent<Fighter>();
      _health = GetComponent<Health>();
      _actionStore = GetComponent<ActionStore>();
    }

    void Update()
    {
      InteractWithAction();
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

    void InteractWithAction()
    {
      if (Input.GetKeyDown(KeyCode.Alpha1))
        _actionStore.Use(0, gameObject);
      if (Input.GetKeyDown(KeyCode.Alpha2))
        _actionStore.Use(1, gameObject);
      if (Input.GetKeyDown(KeyCode.Alpha3))
        _actionStore.Use(2, gameObject);
      if (Input.GetKeyDown(KeyCode.Alpha4))
        _actionStore.Use(3, gameObject);
      if (Input.GetKeyDown(KeyCode.Alpha5))
        _actionStore.Use(4, gameObject);
      if (Input.GetKeyDown(KeyCode.Alpha6))
        _actionStore.Use(5, gameObject);
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
            SetCursor(raycastable.CursorType);
            return true;
          }
        }
      }
      return false;
    }

    RaycastHit[] RaycastSorted()
    {
      var hits = Physics.SphereCastAll(MouseRay, _raycastRadius);
      Array.Sort(hits.Select(h => h.distance).ToArray(), hits);
      return hits;
    }

    bool InteractWithUI()
    {
      if (Input.GetMouseButtonUp(0))
        _isDraggingUI = false;
      if (EventSystem.current.IsPointerOverGameObject())
      {
        SetCursor(CursorType.UI);
        if (Input.GetMouseButtonDown(0))
          _isDraggingUI = true;
        return true;
      }
      return _isDraggingUI;
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
        if (!_mover.CanMoveTo(target)) return false;
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
      return true;
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