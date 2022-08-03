using System.Collections;
using UnityEngine;

namespace RPG.Scene
{
  public class Fader : MonoBehaviour
  {
    CanvasGroup _cg;
    CanvasGroup CG
    {
      get
      {
        if (_cg == null)
          _cg = GetComponent<CanvasGroup>();
        return _cg;
      }
    }
    public void InstantFadeOut()
    {
      CG.alpha = 1;
    }
    public IEnumerator FadeOut(float time)
    {
      while (CG.alpha < 1)
      {
        CG.alpha += Time.deltaTime / time;
        yield return null;
      }
    }
    public IEnumerator FadeIn(float time)
    {
      while (CG.alpha > 0)
      {
        CG.alpha -= Time.deltaTime / time;
        yield return null;
      }
    }
  }

}