using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using RPG.Utils;
using UnityEngine;

namespace RPG.Resx
{
  public class Health : MonoBehaviour, ISaveable
  {
    Animator _animator;
    BaseStats _stats;
    LazyValue<float> _curHealth;
    public float Percentage { get => _curHealth.Value / _stats.MaxHealth * 100; }
    public bool IsDead { get => _curHealth.Value == 0; }

    void Awake()
    {
      _animator = GetComponent<Animator>();
      _stats = GetComponent<BaseStats>();
      _curHealth = new(() => _stats.MaxHealth);
    }
    void OnEnable()
    {
      _stats.OnLevelUp += HealthRegenOnLevelUp;
    }
    void OnDisable()
    {
      _stats.OnLevelUp -= HealthRegenOnLevelUp;
    }
    void HealthRegenOnLevelUp() => _curHealth.Value = Mathf.Min(_stats.MaxHealth, (_stats.MaxHealth - _curHealth.Value) * .3f + _curHealth.Value);
    public void TakeDamage(GameObject from, float dmg)
    {
      if (IsDead)
        return;
      _curHealth.Value = Mathf.Max(0, _curHealth.Value - dmg);
      if (IsDead)
      {
        Die();
        AwardXP(from);
      }
    }

    void AwardXP(GameObject from)
    {
      if (!from.TryGetComponent<Experience>(out var xp)) return;
      xp.GainXP(_stats.XPReward);
    }

    void Die()
    {
      _animator.SetTrigger("Die");
      GetComponent<ActionScheduler>().CancelCurAction();
    }

    public object CaptureState()
    {
      return _curHealth.Value;
    }

    public void RestoreState(object state)
    {
      _curHealth.Value = (float)state;
      if (_curHealth.Value == 0) Die();
    }
  }
}