using System;
using UnityEngine;

namespace RPG.Combat
{
  public class AggroGroup : MonoBehaviour
  {
    [SerializeField] Fighter[] _fighters;
    [SerializeField] bool _activateOnStart;
    void Start()
    {
      Activate(_activateOnStart);
    }

    public void Activate(bool active)
    {
      foreach (var fighter in _fighters)
      {
        if (fighter.TryGetComponent<CombatTarget>(out var target))
          target.enabled = active;
        fighter.enabled = active;
      }
    }

    void Update()
    {

    }
  }

}