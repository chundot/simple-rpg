using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Dialogue
{
  public class DialogueTrigger : MonoBehaviour
  {
    [SerializeField] DialogueEvent[] _events;
    Dictionary<string, UnityEvent> _lookup;
    public void Trigger(string action)
    {
      BuildLookUp();
      if (_lookup.ContainsKey(action))
        _lookup[action].Invoke();
    }

    void BuildLookUp()
    {
      if (_lookup != null) return;
      _lookup = _events.ToDictionary(p => p.ActionID, p => p.OnTrigger);
    }

    [Serializable]
    struct DialogueEvent
    {
      public string ActionID;
      public UnityEvent OnTrigger;
    }
  }

}