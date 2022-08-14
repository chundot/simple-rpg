using System;
using RPG.Saving;
using UnityEngine;

namespace RPG.Inventories
{
  public class Purse : MonoBehaviour, ISaveable
  {
    [SerializeField] float _startingBalance;
    float _balance;
    public event Action OnChange;
    public float Balance
    {
      get => _balance;
      set => _balance = value;
    }
    void Awake()
    {
      _balance = _startingBalance;
    }
    public void UpdateBalance(float amount)
    {
      _balance += amount;
      OnChange?.Invoke();
    }

    public object CaptureState()
    {
      return _balance;
    }

    public void RestoreState(object state)
    {
      Balance = (float)state;
    }
  }

}