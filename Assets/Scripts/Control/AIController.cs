using System;
using RPG.Combat;
using RPG.Core;
using RPG.Manager;
using RPG.Movement;
using RPG.Resx;
using RPG.Utils;
using UnityEngine;

namespace RPG.Control
{
  public class AIController : MonoBehaviour
  {
    [SerializeField] float _chaseDistance = 5f, _suspcionTime = 5f, _waypointTolerance = 1f, _waypointWaitTime = 5f, _agroCDTime = 3, _shoutDistance = 4;
    [SerializeField][Range(0, 1)] float _patrolFraction = .3f;
    [SerializeField] PatrolPath _patrolPath;
    GameObject _target;

    GameObject Target
    {
      get
      {
        if (_target == null)
        {
          _target = SceneMgr.Self.Player.gameObject;
          //_target = GameObject.FindWithTag("Player");
        }
        return _target;
      }
    }

    Fighter _fighter;
    Health _health;
    Mover _mover;
    ActionScheduler _scheduler;
    LazyValue<Vector3> _guardPos;
    int _curWaypointIdx = 0;
    float _timeSinceSawPlayer = Mathf.Infinity, _timeSinceAtWaypoint = Mathf.Infinity, _timeSinceAggrevated = Mathf.Infinity;
    float Distance => Vector3.Distance(transform.position, Target.transform.position);
    bool InRange => Distance < _chaseDistance;
    bool IsAggrevated => InRange || _timeSinceAggrevated < _agroCDTime;
    Vector3 CurWaypoint => _patrolPath.GetPoint(_curWaypointIdx);
    bool AtWayPoint => Vector3.Distance(transform.position, CurWaypoint) < _waypointTolerance;
    void Awake()
    {
      _fighter = GetComponent<Fighter>();
      _health = GetComponent<Health>();
      _mover = GetComponent<Mover>();
      _scheduler = GetComponent<ActionScheduler>();
      _guardPos = new(() => transform.position);
    }

    // Update is called once per frame
    void Update()
    {
      if (_health.IsDead)
      {
        return;
      }

      if (IsAggrevated && _fighter.CanAttack(Target))
      {
        _timeSinceSawPlayer = 0;
        AttackBehaviour();
      }
      else if (_timeSinceSawPlayer < _suspcionTime)
      {
        SuspicionBehaviour();
      }
      else
      {
        PatrolBehaviour();
      }
      UpdateTimer();
    }

    void CycleWaypoint() => _curWaypointIdx = _patrolPath.GetNext(_curWaypointIdx);

    void SuspicionBehaviour()
    {
      _scheduler.CancelCurAction();
    }

    void PatrolBehaviour()
    {
      var nextPos = _guardPos.Value;
      if (_patrolPath != null)
      {
        if (AtWayPoint)
        {
          _timeSinceAtWaypoint = 0;
          CycleWaypoint();
        }
        nextPos = CurWaypoint;
      }
      if (_timeSinceAtWaypoint > _waypointWaitTime)
        _mover.StartMoveAction(nextPos, _patrolFraction);
    }

    void AttackBehaviour()
    {
      AggrevateNearbyEnemies();
      _fighter.Attack(Target);
    }

    void AggrevateNearbyEnemies()
    {
      var hits = Physics.SphereCastAll(transform.position, _shoutDistance, Vector3.up, 0);
      foreach (var hit in hits)
      {
        if (!hit.transform.TryGetComponent<AIController>(out var ctrl) || hit.transform == transform) continue;
        ctrl.Aggrevate();
      }
    }

    void UpdateTimer()
    {
      _timeSinceSawPlayer += Time.deltaTime;
      _timeSinceAtWaypoint += Time.deltaTime;
      _timeSinceAggrevated += Time.deltaTime;
    }
    public void Aggrevate()
    {
      _timeSinceAggrevated = 0;
    }
    void OnDrawGizmosSelected()
    {
      Gizmos.color = Color.blue;
      Gizmos.DrawWireSphere(transform.position, _chaseDistance);
    }
  }

}