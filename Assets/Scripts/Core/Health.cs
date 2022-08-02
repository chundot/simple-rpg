using RPG.Saving;
using UnityEngine;

namespace RPG.Core
{
  public class Health : MonoBehaviour, ISaveable
  {
    [SerializeField] float _health = 100f;
    public bool IsDead { get => _health == 0; }
    Animator _animator;
    void Start()
    {
      _animator = GetComponent<Animator>();
    }
    public void TakeDamage(float dmg)
    {
      if (IsDead)
        return;
      _health = Mathf.Max(0, _health - dmg);
      if (IsDead)
        Die();
    }

    private void Die()
    {
      _animator.SetTrigger("Die");
      GetComponent<ActionScheduler>().CancelCurAction();
    }

    public object CaptureState()
    {
      return _health;
    }

    public void RestoreState(object state)
    {
      _health = (float)state;
      if (_health == 0) Die();
    }
  }
}