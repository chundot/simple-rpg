using System.Collections.Generic;
using RPG.Core;
using RPG.Movement;
using RPG.Resx;
using RPG.Saving;
using RPG.Stats;
using RPG.Utils;
using UnityEngine;

namespace RPG.Combat
{
  [RequireComponent(typeof(ActionScheduler))]
  public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider
  {
    [SerializeField] Transform _lHandTransform, _rHandTransform;
    [SerializeField] Weapon _defWeapon;
    [SerializeField] string _defWeaponName = "Unarmed";
    LazyValue<Weapon> _curWeapon;
    Health _target;
    Mover _mover;
    ActionScheduler _scheduler;
    Animator _animator;
    BaseStats _stats;
    float _atkTimer;
    bool InRange
    {
      get => Vector3.Distance(_target.transform.position, transform.position) < _curWeapon.Value.Range;
    }
    public Health Target { get => _target; }
    public float Dmg { get => _stats.Dmg; }
    const string WEAPON_NAME = "Weapon";
    void Awake()
    {
      _mover = GetComponent<Mover>();
      _scheduler = GetComponent<ActionScheduler>();
      _animator = GetComponent<Animator>();
      _stats = GetComponent<BaseStats>();
      _curWeapon = new(() =>
      {
        var def = Resources.Load<Weapon>(_defWeaponName);
        AttachWeapon(def);
        return def;
      });
    }
    void Start()
    {
      _curWeapon.ForceInit();
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

    public void EquipWeapon(Weapon weapon)
    {
      DestroyOldWeapon(_lHandTransform, _rHandTransform);
      _curWeapon.Value = weapon;
      AttachWeapon(weapon);
    }

    private void AttachWeapon(Weapon weapon)
    {
      var weaponInstance = weapon.Spawn(_lHandTransform, _rHandTransform, _animator);
      if (weaponInstance != null)
        weaponInstance.name = WEAPON_NAME;
    }

    void DestroyOldWeapon(Transform lHandTransform, Transform rHandTransform)
    {
      var lOldWeapon = lHandTransform.Find(WEAPON_NAME);
      var rOldWeapon = rHandTransform.Find(WEAPON_NAME);
      if (lOldWeapon != null)
      {
        lOldWeapon.name = "Destroyed";
        Destroy(lOldWeapon.gameObject);
      }
      if (rOldWeapon != null)
      {
        rOldWeapon.name = "Destroyed";
        Destroy(rOldWeapon.gameObject);
      }
    }

    void AttackBehaviour()
    {
      transform.LookAt(_target.transform);
      if (_atkTimer <= 0)
      {
        _animator.ResetTrigger("StopAttack");
        // Trigger Hit()
        _animator.SetTrigger("Attack");
        _atkTimer = _curWeapon.Value.CD;
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
      _mover.Cancel();
    }
    public void Hit()
    {
      if (_target == null) return;
      if (_curWeapon.Value.HasProjectile)
      {
        _curWeapon.Value.LaunchProjectile(_lHandTransform, _rHandTransform, _target, gameObject, Dmg);
      }
      else
      {
        _target.TakeDamage(gameObject, Dmg);
      }
    }
    public void Shoot()
    {
      Hit();
    }

    public object CaptureState()
    {
      return _curWeapon.Value.name;
    }

    public void RestoreState(object state)
    {
      if (state is not string s) return;
      var weapon = Resources.Load<Weapon>(s);
      EquipWeapon(weapon);
    }

    public IEnumerable<float> GetAdditiveModifier(StatsEnum stat)
    {
      if (stat is StatsEnum.Damage)
        yield return _curWeapon.Value.Dmg;
    }

    public IEnumerable<float> GetPercentModifier(StatsEnum stat)
    {
      if (stat is StatsEnum.Damage)
        yield return _curWeapon.Value.ExtraPercent;
    }
  }

}