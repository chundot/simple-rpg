using System;
using RPG.Attributes;
using RPG.Core;
using RPG.Extensions;
using RPG.Saving;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
  public class Mover : MonoBehaviour, IAction, ISaveable
  {
    [SerializeField] float _maxSpd = 6f, _maxNavPathLength = 40;
    NavMeshAgent _agent;
    Animator _animator;
    ActionScheduler _scheduler;
    Health _health;
    void Awake()
    {
      _agent = GetComponent<NavMeshAgent>();
      _animator = GetComponent<Animator>();
      _scheduler = GetComponent<ActionScheduler>();
      _health = GetComponent<Health>();
    }
    void Start()
    {
      _agent.enabled = true;
    }
    void Update()
    {
      _agent.enabled = !_health.IsDead;
      UpdateAnimator();
    }
    public void StartMoveAction(Vector3 dest, float fraction = 1f)
    {
      _scheduler.StartAction(this);
      MoveTo(dest, fraction);
    }

    public void MoveTo(Vector3 dest, float fraction = 1f)
    {
      _agent.speed = _maxSpd * Mathf.Clamp01(fraction);
      _agent.destination = dest;
      _agent.isStopped = false;
    }
    public bool CanMoveTo(Vector3 dest)
    {
      NavMeshPath path = new();
      bool hasPath = NavMesh.CalculatePath(transform.position, dest, NavMesh.AllAreas, path);
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
    public void Cancel()
    {
      _agent.isStopped = true;
    }

    void UpdateAnimator()
    {
      var velocity = _agent.velocity;
      var localVelocity = transform.InverseTransformDirection(velocity);
      var spd = localVelocity.z;
      _animator.SetFloat("ZMove", spd);
    }
    public void Warp(Vector3 pos)
    {
      _agent.Warp(pos);
    }
    public object CaptureState()
    {
      MoveData moveData = new() { pos = new(transform.position), angles = new(transform.eulerAngles) };
      return moveData;
    }

    public void RestoreState(object state)
    {
      if (state is not MoveData md) return;
      _agent.Warp(md.pos);
      transform.eulerAngles = md.angles.ToVector();
    }
    [Serializable]
    struct MoveData
    {
      public SerializableVector3 pos, angles;
    }
  }
}