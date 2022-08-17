using RPG.Manager;
using RPG.Stats;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
  public class TraitUI : MonoBehaviour
  {
    [SerializeField] TextMeshProUGUI _unassignedPoints;
    [SerializeField] Button _commitBtn;
    TraitStore _store;
    void Awake()
    {
      _store = SceneMgr.Self.Player.GetComponent<TraitStore>();
      _store.OnPointsChanged += Redraw;
      _commitBtn.onClick.AddListener(_store.Commit);
      Redraw();
    }
    void OnEnable()
    {
      Redraw();
    }

    void Redraw()
    {
      _unassignedPoints.text = _store.UnassignedPoints.ToString();
    }
  }
}