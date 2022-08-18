using RPG.Saving;
using RPG.Stats;
using RPG.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Attributes
{
  public class Mana : MonoBehaviour, ISaveable
  {
    [SerializeField] UnityEvent<float> _onFractionChange;
    [SerializeField] float _maxMana = 200;
    LazyValue<float> _curMana;
    BaseStats _stats;
    public float CurMana { get => _curMana.Value; set { _curMana.Value = value; _onFractionChange.Invoke(Percentage / 100); } }
    public float MaxMana => _stats.GetStat(StatsEnum.MaxMana);
    public float ManaRegen => _stats.GetStat(StatsEnum.ManaRegen);
    public float Percentage => CurMana / MaxMana * 100;

    void Awake()
    {
      _stats = GetComponent<BaseStats>();
      _curMana = new(() => MaxMana);
    }
    void Update()
    {
      if (CurMana < _maxMana)
        CurMana = Mathf.Min(MaxMana, CurMana + ManaRegen * Time.deltaTime);
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