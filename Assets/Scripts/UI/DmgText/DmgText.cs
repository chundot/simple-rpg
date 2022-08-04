using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.DmgText
{
  public class DmgText : MonoBehaviour
  {
    [SerializeField] Text _text;
    CanvasGroup _cg;
    void Awake()
    {
      _cg = GetComponent<CanvasGroup>();
    }
    void Start()
    {
      _cg.DOFade(0, .8f);
      transform.DOMoveY(transform.position.y + 1, .8f);
    }
    public void SetText(float dmg)
    {
      _text.text = $"{dmg:0}";
    }
    IEnumerator Destroy(float timer = .82f)
    {
      yield return new WaitForSeconds(timer);
      Destroy(this);
    }
  }
}