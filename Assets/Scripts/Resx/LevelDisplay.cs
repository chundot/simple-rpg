using RPG.Stats;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Resx
{
  public class LevelDisplay : MonoBehaviour
  {
    BaseStats _stats;
    Text _text;
    void Awake()
    {
      _stats = GameObject.FindGameObjectWithTag("Player").GetComponent<BaseStats>();
      _text = GetComponent<Text>();
    }
    void Update()
    {
      _text.text = $"{_stats.Level}";
    }
  }
}