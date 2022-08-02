using System.IO;
using RPG.Core;
using UnityEditor;
using UnityEngine;

namespace RPG.Combat
{
  [CreateAssetMenu(fileName = "New Weapon", menuName = "RPG/Weapon", order = 0)]
  public class Weapon : ScriptableObject
  {
    [SerializeField] AnimatorOverrideController _weaponAnimator;
    [SerializeField] GameObject _weaponPrefab;
    [SerializeField] float _range, _dmg, _cd;
    [SerializeField] bool _isRightHanded = true;
    [SerializeField] Projectile _projectile;
    public float Range { get => _range; }
    public float Dmg { get => _dmg; }
    public float CD { get => _cd; }
    public bool HasProjectile { get => _projectile != null; }
    public GameObject Spawn(Transform lHandTransform, Transform rHandTransform, Animator animator)
    {
      GameObject obj = null;
      if (_weaponPrefab != null)
        obj = Instantiate(_weaponPrefab, GetTransform(lHandTransform, rHandTransform));
      if (_weaponAnimator != null)
        animator.runtimeAnimatorController = _weaponAnimator;
      else if (animator.runtimeAnimatorController is AnimatorOverrideController animatorOverrideController)
        animator.runtimeAnimatorController = animatorOverrideController.runtimeAnimatorController;
      return obj;
    }

    public void LaunchProjectile(Transform lHandTransform, Transform rHandTransform, Health target)
    {
      if (!HasProjectile) return;
      var projectile = Instantiate(_projectile, GetTransform(lHandTransform, rHandTransform).position, Quaternion.identity);
      projectile.Dmg = Dmg;
      projectile.Target = target;
    }

    Transform GetTransform(Transform lHandTransform, Transform rHandTransform) => _isRightHanded ? rHandTransform : lHandTransform;
  }
}