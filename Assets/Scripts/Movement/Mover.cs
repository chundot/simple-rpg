using RPG.Combat;
using RPG.Core;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
  public class Mover : MonoBehaviour, IAction
  {
    [SerializeField] float _maxSpd = 6f;
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