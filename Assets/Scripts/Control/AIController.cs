using System;
using RPG.Combat;
using RPG.Core;
using RPG.Manager;
using RPG.Movement;
using UnityEngine;

namespace RPG.Control
{
  public class AIController : MonoBehaviour
  {
    [SerializeField] private float _chaseDistance = 5f, _suspcionTime = 5f, _waypointTolerance = 1f, _waypointWaitTime = 5f;
    [SerializeField] private PatrolPath _patrolPath;
    private GameObject _target;

    private GameObject Target
    {
      get
      {
        if (_target == null)
        {
          _target = SceneMgr.Self.Player.gameObject;
        }

        return _target;
      }
    }

    private Fighter _fighter;
    private Health _health;
    private Mover _mover;
    private ActionScheduler _scheduler;
    private Vector3 _guardPos;
    private int _curWaypointIdx = 0;
    private float _timeSinceSawPlayer, _timeSinceAtWaypoint;
    private float Distance => Vector3.Distance(transform.position, Target.transform.position);
    private bool InRange => Distance < _chaseDistance;
    private Vector3 CurWaypoint { get => _patrolPath.GetPoint(_curWaypointIdx); }
    private bool AtWayPoint => Vector3.Distance(transform.position, CurWaypoint) < _waypointTolerance;
    private void Start()
    {
      _fighter = GetComponent<Fighter>();
      _health = GetComponent<Health>();
      _mover = GetComponent<Mover>();
      _scheduler = GetComponent<ActionScheduler>();
      _guardPos = transform.position;
    }

    // Update is called once per frame
    private void Update()
    {
      if (_health.IsDead)
      {
        return;
      }

      if (InRange && _fighter.CanAttack(Target))
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

    private void CycleWaypoint() => _curWaypointIdx = _patrolPath.GetNext(_curWaypointIdx);

    private void SuspicionBehaviour()
    {
      _scheduler.CancelCurAction();
    }

    private void PatrolBehaviour()
    {
      var nextPos = _guardPos;
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
        _mover.MoveTo(nextPos);
    }

    private void AttackBehaviour()
    {
      _fighter.Attack(Target);
    }
    private void UpdateTimer()
    {
      _timeSinceSawPlayer += Time.deltaTime;
      _timeSinceAtWaypoint += Time.deltaTime;
    }

    private void OnDrawGizmosSelected()
    {
      Gizmos.color = Color.blue;
      Gizmos.DrawWireSphere(transform.position, _chaseDistance);
    }
  }

}