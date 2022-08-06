using Pinwheel.Jupiter;
using UnityEngine;

public class WorldTime : MonoBehaviour
{
  [SerializeField] float _fraction = .015f;
  JDayNightCycle _cycle;
  public float CurTime { get => _cycle.Time; }
  void Awake()
  {
    _cycle = GetComponent<JDayNightCycle>();
  }

  void Update()
  {
    _cycle.Time += Time.deltaTime * _fraction;
  }
}
