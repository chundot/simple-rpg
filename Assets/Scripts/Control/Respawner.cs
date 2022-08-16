using System.Collections;
using RPG.Attributes;
using RPG.Scene;
using RPG.Utils;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Control
{
  public class Respawner : MonoBehaviour
  {
    [SerializeField] Transform _startLoc;
    [SerializeField] float _enemyHealthRegen = 20;
    Health _health;
    LazyValue<NavMeshAgent> _agent;
    LazyValue<SavingWrapper> _savingWrapper;
    void Awake()
    {
      _agent = new(() => GetComponent<NavMeshAgent>());
      _savingWrapper = new(() => GetComponent<SavingWrapper>());
      _health = GetComponent<Health>();
      _health.OnDie.AddListener(Respawn);
    }
    void Start()
    {
      if (_health.IsDead) Respawn();
    }
    void Respawn()
    {
      StartCoroutine(RespawnRoutine());
    }
    IEnumerator RespawnRoutine()
    {
      yield return new WaitForSeconds(2.5f);
      ResetPlayer();
      ResetEnemy();
      _savingWrapper.Value.Save();
    }

    void ResetEnemy()
    {
      foreach (var ai in FindObjectsOfType<AIController>())
      {
        if (ai.TryGetComponent<Health>(out var health) && !health.IsDead)
        {
          ai.Reset();
          health.HealByPercentage(_enemyHealthRegen);
        }
      }
    }

    void ResetPlayer()
    {
      _agent.Value.Warp(_startLoc.position);
      _health.HealByPercentage(60);
    }
  }
}