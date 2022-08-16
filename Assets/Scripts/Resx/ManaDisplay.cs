using RPG.Attributes;
using RPG.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Resx
{
  public class ManaDisplay : MonoBehaviour
  {
    Mana _mana;
    Text _text;
    void Awake()
    {
      _mana = SceneMgr.Self.Player.GetComponent<Mana>();
      _text = GetComponent<Text>();
    }
    void Update()
    {
      _text.text = $"{_mana.Percentage:0}%";
    }
  }
}