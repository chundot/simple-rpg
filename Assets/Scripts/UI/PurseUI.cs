using RPG.Inventories;
using RPG.Manager;
using TMPro;
using UnityEngine;

namespace RPG.UI
{
  public class PurseUI : MonoBehaviour
  {
    [SerializeField] TextMeshProUGUI _balance;
    Purse _purse;
    void Start()
    {
      _purse = SceneMgr.Self.Player.GetComponent<Purse>();
      if (_purse)
      {
        _purse.OnChange += Redraw;
        Redraw();
      }
    }

    void Redraw()
    {
      _balance.text = $"{_purse.Balance:0}";
    }
  }
}