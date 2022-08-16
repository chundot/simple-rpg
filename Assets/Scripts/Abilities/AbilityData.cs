using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using UnityEngine;

namespace RPG.Abilities
{
  public class AbilityData : IAction
  {
    GameObject _user;
    IEnumerable<GameObject> _targets;
    Vector3 _targetPoint;
    bool _cancelled = false;
    public Vector3 TargetPoint { get => _targetPoint; set => _targetPoint = value; }
    public GameObject User { get => _user; }
    public IEnumerable<GameObject> Targets { get => _targets; set => _targets = value; }
    public bool IsCancelled => _cancelled;
    public AbilityData(GameObject user)
    {
      _user = user;
    }
    public void StartCoroutine(IEnumerator coroutine)
    {
      _user.GetComponent<MonoBehaviour>().StartCoroutine(coroutine);
    }

    public void Cancel()
    {
      _cancelled = true;
    }
  }
}
