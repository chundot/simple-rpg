using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using RPG.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Attributes
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
    public float MaxHealth => _stats.GetStat(StatsEnum.Health);
    public float Percentage { get => Fraction * 100; }
    public float Fraction { get => CurHealth / MaxHealth; }
    public bool IsDead { get => CurHealth == 0; }
    void Awake()
    {
      _animator = GetComponent<Animator>();
      _stats = GetComponent<BaseStats>();
      _curHealth = new(() => MaxHealth);
    }
    public void HealByPercentage(float percentage) => Heal(percentage / 100 * MaxHealth);

    public void Heal(float healthRegen)
    {
      CurHealth = Mathf.Min(MaxHealth, CurHealth + healthRegen);
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
    void HealthRegenOnLevelUp() => CurHealth = Mathf.Min(MaxHealth, (MaxHealth - CurHealth) * .3f + CurHealth);
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
    public void TakeDmgByPercentage(GameObject from, float percentage)
    {
      TakeDamage(from, percentage / 100 * MaxHealth);
    }

    void AwardXP(GameObject from)
    {
      if (!from.TryGetComponent<Experience>(out var xp)) return;
      xp.GainXP(_stats.GetStat(StatsEnum.XPReward));
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