using RPG.Manager;
using RPG.Stats;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Resx
{
  public class XPDisplay : MonoBehaviour
  {
    Experience _xp;
    Text _text;
    void Awake()
    {
      _text = GetComponent<Text>();
    }
    void Start()
    {
      _xp = SceneMgr.Self.Player.GetComponent<Experience>();
    }
    void Update()
    {
      _text.text = $"{_xp.CurNum}";
    }
  }
}