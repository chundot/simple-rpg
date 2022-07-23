using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat
{
  [RequireComponent(typeof(ActionScheduler))]
  public class Fighter : MonoBehaviour, IAction
  {
    [SerializeField]
    float _weaponRange, _atkCD, _atkDmg;
    Health _target;
    Mover _mover;
    ActionScheduler _scheduler;
    Animator _animator;
    float _atkTimer;
    private bool InRange
    {
      get => Vector3.Distance(_target.transform.position, transform.position) < _weaponRange;
    }
    void Start()
    {
      _mover = GetComponent<Mover>();
      _scheduler = GetComponent<ActionScheduler>();
      _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
      if (_atkTimer > 0) _atkTimer -= Time.deltaTime;
      if (_target == null || _target.IsDead)
        return;
      if (!InRange)
        _mover.MoveTo(_target.transform.position);
      else
      {
        _mover.Cancel();
        AttackBehaviour();
      }
    }

    private void AttackBehaviour()
    {
      transform.LookAt(_target.transform);
      if (_atkTimer <= 0)
      {
        _animator.ResetTrigger("StopAttack");
        // Trigger Hit()
        _animator.SetTrigger("Attack");
        _atkTimer = _atkCD;
      }
    }

    public void Attack(GameObject target)
    {
      _scheduler.StartAction(this);
      _target = target.GetComponent<Health>();
    }
    public bool CanAttack(GameObject target)
    {
      if (target is null)
        return false;
      var targetHealth = target.GetComponent<Health>();
      return targetHealth != null && !targetHealth.IsDead;
    }
    public void Cancel()
    {
      _target = null;
      _animator.ResetTrigger("Attack");
      _animator.SetTrigger("StopAttack");
    }
    public void Hit()
    {
      if (_target == null) return;
      _target.TakeDamage(_atkDmg);
    }
  }

}