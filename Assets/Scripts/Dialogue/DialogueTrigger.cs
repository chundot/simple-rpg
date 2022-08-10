using UnityEngine;
using UnityEngine.Events;

namespace RPG.Dialogue
{
  public class DialogueTrigger : MonoBehaviour
  {
    [SerializeField] string _action;
    [SerializeField] UnityEvent _onTrigger;

    public void Trigger(string action)
    {
      if (action == _action)
        _onTrigger.Invoke();
    }
  }

}