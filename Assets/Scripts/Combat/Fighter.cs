using System.Collections.Generic;
using RPG.Attributes;
using RPG.Core;
using RPG.Inventories;
using RPG.Movement;
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
    [SerializeField] WeaponConfig _defWeapon;
    WeaponConfig _curWeaponCfg;
    Equipment _equipment;
    LazyValue<Weapon> _curWeapon;
    Health _target;
    Mover _mover;
    ActionScheduler _scheduler;
    Animator _animator;
    BaseStats _stats;
    float _atkTimer;
    bool InRange
    {
      get => _target ? Vector3.Distance(_target.transform.position, transform.position) < _curWeaponCfg.Range : false;
    }
    public Health Target { get => _target; }
    public float Dmg { get => _stats.Dmg; }
    const string WEAPON_NAME = "Weapon";
    const string WEAPON_PATH = "Items";
    void Awake()
    {
      _mover = GetComponent<Mover>();
      _scheduler = GetComponent<ActionScheduler>();
      _animator = GetComponent<Animator>();
      _stats = GetComponent<BaseStats>();
      _curWeaponCfg = _defWeapon;
      _curWeapon = new(() => AttachWeapon(_defWeapon));
      if (TryGetComponent(out _equipment))
        _equipment.EquipmentUpdated += UpdateWeapon;
    }

    void UpdateWeapon()
    {
      var weapon1 = _equipment.GetItemInSlot(EquipLocation.Weapon) as WeaponConfig;
      EquipWeapon(weapon1 ? weapon1 : _defWeapon);
    }

    void Start()
    {
      _curWeapon.ForceInit();
    }

    public Transform GetHandTransform(bool right)
    {
      return right ? _rHandTransform : _lHandTransform;
    }
    void Update()
    {
      if (_atkTimer > 0) _atkTimer -= Time.deltaTime;
      if (!_target || _target.IsDead)
        return;
      if (!InRange)
        _mover.MoveTo(_target.transform.position);
      else
      {
        _mover.Cancel();
        AttackBehaviour();
      }
    }

    public void EquipWeapon(WeaponConfig weapon)
    {
      DestroyOldWeapon(_lHandTransform, _rHandTransform);
      _curWeaponCfg = weapon;
      _curWeapon.Value = AttachWeapon(weapon);
    }

    Weapon AttachWeapon(WeaponConfig weapon)
    {
      var weaponInstance = weapon.Spawn(_lHandTransform, _rHandTransform, _animator);
      if (weaponInstance != null)
        weaponInstance.name = WEAPON_NAME;
      return weaponInstance;
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
        _atkTimer = _curWeaponCfg.CD;
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
      if (!_mover.CanMoveTo(target.transform.position) && !InRange) return false;
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
      if (!_target) return;
      if (_curWeapon.Value != null)
        _curWeapon.Value.OnHit();
      if (_curWeaponCfg.HasProjectile)
      {
        _curWeaponCfg.LaunchProjectile(_lHandTransform, _rHandTransform, _target, gameObject, Dmg);
      }
      else if (InRange)
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
      return _curWeaponCfg.name;
    }

    public void RestoreState(object state)
    {
      if (state is not string s) return;
      var weapon = Resources.Load<WeaponConfig>($"{WEAPON_PATH}/{s}");
      EquipWeapon(weapon);
    }

    public IEnumerable<float> GetAdditiveModifier(StatsEnum stat)
    {
      if (stat is StatsEnum.Damage)
        yield return _curWeaponCfg.Dmg;
    }

    public IEnumerable<float> GetPercentModifier(StatsEnum stat)
    {
      if (stat is StatsEnum.Damage)
        yield return _curWeaponCfg.ExtraPercent;
    }
  }

}