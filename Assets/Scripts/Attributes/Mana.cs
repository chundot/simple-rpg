using RPG.Saving;
using RPG.Stats;
using RPG.Utils;
using UnityEngine;

namespace RPG.Attributes
{
  public class Mana : MonoBehaviour, ISaveable
  {
    [SerializeField] float _maxMana = 200, _regenRate = 2;
    LazyValue<float> _curMana;
    BaseStats _stats;
    public float CurMana { get => _curMana.Value; set => _curMana.Value = value; }
    public float MaxMana => _stats.MaxMana;

    public float Percentage => CurMana / MaxMana * 100;

    void Awake()
    {
      _stats = GetComponent<BaseStats>();
      _curMana = new(() => MaxMana);
    }
    void Update()
    {
      if (CurMana < _maxMana)
        CurMana = Mathf.Min(MaxMana, CurMana + _regenRate * Time.deltaTime);
    }
    public bool UserMana(float mana)
    {
      if (mana > CurMana) return false;
      CurMana -= mana;
      return true;
    }
    public object CaptureState()
    {
      return CurMana;
    }

    public void RestoreState(object state)
    {
      CurMana = (float)state;
    }
  }

}