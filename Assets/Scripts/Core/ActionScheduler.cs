using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
  public class ActionScheduler : MonoBehaviour
  {
    IAction _curAction;
    public void StartAction(IAction action)
    {
      if (_curAction == action) return;
      if (_curAction != null) _curAction.Cancel();
      _curAction = action;
    }

    public void CancelCurAction()
    {
      StartAction(null);
    }
  }

}