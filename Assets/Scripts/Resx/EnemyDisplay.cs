using RPG.Combat;
using RPG.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Resx
{
  public class EnemyDisplay : MonoBehaviour
  {
    Fighter _player;
    Text _text;
    void Awake()
    {
      _text = GetComponent<Text>();
    }
    void Start()
    {
      _player = SceneMgr.Self.Player.gameObject.GetComponent<Fighter>();
    }
    void Update()
    {
      var target = _player.Target;
      if (!target) _text.text = "N/A";
      else _text.text = $"{target.Percentage:0}%";
    }
  }
}