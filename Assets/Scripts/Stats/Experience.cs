using System;
using RPG.Saving;
using UnityEngine;

namespace RPG.Stats
{
  public class Experience : MonoBehaviour, ISaveable
  {
    float _xpPoints;
    public float CurNum { get => _xpPoints; }
    public event Action OnXPGained;
    public void GainXP(float xp)
    {
      _xpPoints += xp;
      OnXPGained?.Invoke();
    }
    public object CaptureState()
    {
      return _xpPoints;
    }
    public void RestoreState(object state)
    {
      _xpPoints = (float)state;
    }
  }

}