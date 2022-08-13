using Pinwheel.Jupiter;
using RPG.Saving;
using UnityEngine;

public class WorldTime : MonoBehaviour, ISaveable
{
  [SerializeField] float _fraction = .015f;
  JDayNightCycle _cycle;
  public float CurTime { get => _cycle.Time; set => _cycle.Time = value; }
  void Awake()
  {
    _cycle = GetComponent<JDayNightCycle>();
  }

  void Update()
  {
    _cycle.Time += Time.deltaTime * _fraction;
  }

  public object CaptureState()
  {
    return CurTime;
  }

  public void RestoreState(object state)
  {
    CurTime = (float)state;
  }

}
