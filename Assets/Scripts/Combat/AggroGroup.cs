using RPG.Attributes;
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
        if (fighter.TryGetComponent(out CombatTarget target))
          target.enabled = active;
        if (fighter.TryGetComponent(out Health health))
          health.enabled = active;
        fighter.enabled = active;
      }
    }
  }

}