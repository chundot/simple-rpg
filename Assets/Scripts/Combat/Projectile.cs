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
    Vector3 _targetPoint;
    float _dmg;
    Vector3 AimLocation { get => _target ? _target.transform.position + Vector3.up : _targetPoint; }
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
    public void Init(GameObject from, float dmg, Health target = null, Vector3 point = default)
    {
      _targetPoint = point;
      _dmg = dmg;
      _target = target;
      _from = from;
    }
    public void Init(Vector3 point, GameObject from, float dmg)
    {
      _targetPoint = point;
      _dmg = dmg;
      _from = from;
    }
    void Update()
    {
      if (_target && _homing && !_target.IsDead)
        transform.LookAt(AimLocation);
      transform.Translate(Time.deltaTime * _spd * Vector3.forward);
    }
    void OnTriggerEnter(Collider other)
    {
      if (other.gameObject == _from) return;
      if (!other.TryGetComponent(out Health health)) return;
      if (_target && health != _target || health.IsDead) return;
      _onHit.Invoke();
      _spd = 0;
      health.TakeDamage(_from, _dmg);
      if (_hitFX != null)
        Instantiate(_hitFX, AimLocation, transform.rotation);
      foreach (var obj in _destoryOnHit)
        Destroy(obj);
      Destroy(gameObject, _lifeAfterImpact);
    }
  }
}