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
      _xp = GameObject.FindGameObjectWithTag("Player").GetComponent<Experience>();
      _text = GetComponent<Text>();
    }
    void Update()
    {
      _text.text = $"{_xp.CurNum}";
    }
  }
}