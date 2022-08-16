using RPG.Attributes;
using RPG.Resx;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
  public class Projectile : MonoBehaviour
  {
    [SerializeField] float _spd = 8, _maxLifeTime = 10, _lifeAfterImpact = .2f;
    [SerializeField] bool _homing = false;
    [SerializeField] GameObject _hitFX;
    [SerializeField] GameObject[] _destoryOnHit;
    [SerializeField] UnityEvent _onHit;
    Health _target;
    float _dmg;
    Vector3 AimLocation { get => _target.transform.position + Vector3.up; }
    GameObject _from;
    void Start()
    {
      transform.LookAt(AimLocation);
      Destroy(gameObject, _maxLifeTime);
    }
    public void Init(float dmg, Health target, GameObject from)
    {
      _dmg = dmg;
      _target = target;
      _from = from;
    }
    void Update()
    {
      if (!_target) return;
      if (_homing && !_target.IsDead)
        transform.LookAt(AimLocation);
      transform.Translate(Time.deltaTime * _spd * Vector3.forward);
    }
    void OnTriggerEnter(Collider other)
    {
      if (other.GetComponent<Health>() != _target || _target.IsDead) return;
      _onHit.Invoke();
      _spd = 0;
      _target.TakeDamage(_from, _dmg);
      if (_hitFX != null)
        Instantiate(_hitFX, AimLocation, transform.rotation);
      foreach (var obj in _destoryOnHit)
        Destroy(obj);
      Destroy(gameObject, _lifeAfterImpact);
    }
  }
}