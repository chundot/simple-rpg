using RPG.Manager;
using RPG.Stats;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
  public class TraitRowUI : MonoBehaviour
  {
    [SerializeField] Trait _trait;
    [SerializeField] TextMeshProUGUI _valueText;
    [SerializeField] Button _plusBtn, _minusBtn;
    TraitStore _store;
    void Start()
    {
      _store = SceneMgr.Self.Player.GetComponent<TraitStore>();
      _store.OnPointsChanged += Redraw;
      _plusBtn.onClick.AddListener(() => Allocate(1));
      _minusBtn.onClick.AddListener(() => Allocate(-1));
      Redraw();
    }
    public void Allocate(int points)
    {
      _store.AssignPoints(_trait, points);
      Redraw();
    }
    void Redraw()
    {
      _minusBtn.interactable = _store.CanAssignPoints(_trait, -1);
      _plusBtn.interactable = _store.CanAssignPoints(_trait, 1);
      _valueText.text = _store.GetProposedPoints(_trait).ToString();
    }
  }

}