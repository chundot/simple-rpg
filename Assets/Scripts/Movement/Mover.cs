using RPG.Combat;
using RPG.Core;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
  public class Mover : MonoBehaviour, IAction
  {
    NavMeshAgent _agent;
    Animator _animator;
    ActionScheduler _scheduler;
    Health _health;
    void Start()
    {
      _agent = GetComponent<NavMeshAgent>();
      _animator = GetComponent<Animator>();
      _scheduler = GetComponent<ActionScheduler>();
      _health = GetComponent<Health>();
    }
    void Update()
    {
      _agent.enabled = !_health.IsDead;
      UpdateAnimator();
    }
    public void StartMoveAction(Vector3 dest)
    {
      _scheduler.StartAction(this);
      MoveTo(dest);
    }

    public void MoveTo(Vector3 dest)
    {
      _agent.destination = dest;
      _agent.isStopped = false;
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
  }
}