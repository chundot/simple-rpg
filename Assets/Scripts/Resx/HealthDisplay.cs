using RPG.Attributes;
using RPG.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Resx
{
  public class HealthDisplay : MonoBehaviour
  {
    Health _health;
    Text _text;
    void Awake()
    {
      _health = SceneMgr.Self.Player.GetComponent<Health>();
      _text = GetComponent<Text>();
    }
    void Update()
    {
      _text.text = $"{_health.Percentage:0}%";
    }
  }
}