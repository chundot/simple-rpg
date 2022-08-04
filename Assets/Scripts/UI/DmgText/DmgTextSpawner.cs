using UnityEngine;

namespace RPG.UI.DmgText
{
  public class DmgTextSpawner : MonoBehaviour
  {
    [SerializeField] DmgText _dmgText;
    public void Spawn(float dmg)
    {
      var dmgText = Instantiate(_dmgText, transform);
      dmgText.SetText(dmg);
    }
  }

}