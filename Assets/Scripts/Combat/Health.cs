using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
  public class Health : MonoBehaviour
  {
    [SerializeField] float health = 100f;
    public bool IsDead { get => health == 0; }
    Animator _animator;
    void Start()
    {
      _animator = GetComponent<Animator>();
    }
    public void TakeDamage(float dmg)
    {
      if (IsDead)
        return;
      health = Mathf.Max(0, health - dmg);
      if (IsDead)
        Die();
    }

    private void Die()
    {

      _animator.SetTrigger("Die");
    }
  }
}