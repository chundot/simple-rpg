using System.Collections;
using UnityEngine;

namespace RPG.Scene
{
  public class Fader : MonoBehaviour
  {
    CanvasGroup _cg;
    void Start()
    {
      _cg = GetComponent<CanvasGroup>();
    }
    public IEnumerator FadeOut(float time)
    {
      while (_cg.alpha < 1)
      {
        _cg.alpha += Time.deltaTime / time;
        yield return null;
      }
    }
    public IEnumerator FadeIn(float time)
    {
      while (_cg.alpha > 0)
      {
        _cg.alpha -= Time.deltaTime / time;
        yield return null;
      }
    }
  }

}