using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using RPG.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Resx
{
  public class Health : MonoBehaviour, ISaveable
  {
    public UnityEvent OnDie;
    [SerializeField] UnityEvent<float> _takeDmg, _setFill, _onHealthPercentageChanged;
    [SerializeField] UnityEvent<bool> _changeBar;
    Animator _animator;
    BaseStats _stats;
    LazyValue<float> _curHealth;
    float CurHealth { get => _curHealth.Value; set => _curHealth.Value = value; }
    bool _isDeadLastFrame;
    public float Percentage { get => Fraction * 100; }
    public float Fraction { get => CurHealth / _stats.MaxHealth; }
    public bool IsDead { get => CurHealth == 0; }
    void Awake()
    {
      _animator = GetComponent<Animator>();
      _stats = GetComponent<BaseStats>();
      _curHealth = new(() => _stats.MaxHealth);
    }
    public void HealByPercentage(float percentage) => Heal(percentage / 100 * _stats.MaxHealth);

    public void Heal(float healthRegen)
    {
      CurHealth = Mathf.Min(_stats.MaxHealth, CurHealth + healthRegen);
      _onHealthPercentageChanged.Invoke(Percentage);
      UpdateState();
    }

    void OnEnable()
    {
      _stats.OnLevelUp += HealthRegenOnLevelUp;
    }
    void OnDisable()
    {
      _stats.OnLevelUp -= HealthRegenOnLevelUp;
    }
    void HealthRegenOnLevelUp() => CurHealth = Mathf.Min(_stats.MaxHealth, (_stats.MaxHealth - CurHealth) * .3f + CurHealth);
    public void TakeDamage(GameObject from, float dmg)
    {
      if (IsDead)
        return;
      CurHealth = Mathf.Max(0, CurHealth - dmg);
      _onHealthPercentageChanged.Invoke(Percentage);
      if (IsDead)
      {
        OnDie.Invoke();
        AwardXP(from);
      }
      else
      {
        _setFill.Invoke(Fraction);
        _takeDmg.Invoke(dmg);
      }
      UpdateState();
    }

    void AwardXP(GameObject from)
    {
      if (!from.TryGetComponent<Experience>(out var xp)) return;
      xp.GainXP(_stats.XPReward);
    }

    void UpdateState()
    {
      if (!_isDeadLastFrame && IsDead)
      {
        _changeBar.Invoke(false);
        _animator.SetTrigger("Die");
        GetComponent<ActionScheduler>().CancelCurAction();
      }
      if (_isDeadLastFrame && !IsDead)
        _animator.Rebind();
      _isDeadLastFrame = IsDead;
    }

    public object CaptureState()
    {
      return CurHealth;
    }

    public void RestoreState(object state)
    {
      CurHealth = (float)state;
      UpdateState();
    }
  }
}