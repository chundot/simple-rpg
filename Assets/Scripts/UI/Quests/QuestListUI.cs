
using RPG.Manager;
using RPG.Quests;
using UnityEngine;

namespace RPG.UI.Quests
{
  public class QuestListUI : MonoBehaviour
  {
    [SerializeField] QuestItemUI _questPrefab;
    QuestList _list;
    void Start()
    {
      _list = SceneMgr.Self.Player.GetComponent<QuestList>();
      _list.OnUpdate += Redraw;
      Redraw();
    }

    void Redraw()
    {
      transform.DestroyAllChildren();
      foreach (var status in _list.Statuses)
      {
        var questInstance = Instantiate(_questPrefab, transform);
        questInstance.Setup(status);
      }
    }
  }
}
