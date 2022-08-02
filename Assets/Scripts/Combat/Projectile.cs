using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
  public class Projectile : MonoBehaviour
  {
    [SerializeField] float _spd = 8, _maxLifeTime = 10, _lifeAfterImpact = .2f;
    [SerializeField] bool _homing = false;
    [SerializeField] GameObject _hitFX;
    [SerializeField] GameObject[] _destoryOnHit;
    Health _target;
    float _dmg;

    Vector3 AimLocation { get => _target.transform.position + Vector3.up; }
    public Health Target { set => _target = value; }
    public float Dmg { set => _dmg = value; }
    void Start()
    {
      transform.LookAt(AimLocation);
      Destroy(gameObject, _maxLifeTime);
    }
    void Update()
    {
      if (_target == null) return;
      if (_homing && !_target.IsDead)
        transform.LookAt(AimLocation);
      transform.Translate(Time.deltaTime * _spd * Vector3.forward);
    }
    void OnTriggerEnter(Collider other)
    {
      if (other.GetComponent<Health>() != _target || _target.IsDead) return;
      _spd = 0;
      _target.TakeDamage(_dmg);
      if (_hitFX != null)
        Instantiate(_hitFX, AimLocation, transform.rotation);
      foreach (var obj in _destoryOnHit)
        Destroy(obj);
      Destroy(gameObject, _lifeAfterImpact);
    }
  }
}