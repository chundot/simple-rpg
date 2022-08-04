using System.Collections;
using UnityEngine;

namespace RPG.Scene
{
  public class Fader : MonoBehaviour
  {
    CanvasGroup _cg;
    Coroutine _curActiveFade;
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
    public Coroutine FadeOut(float time)
    {
      return Fade(1, time);
    }
    public Coroutine FadeIn(float time)
    {
      return Fade(0, time);
    }
    public Coroutine Fade(float target, float time)
    {
      if (_curActiveFade != null) StopCoroutine(_curActiveFade);
      _curActiveFade = StartCoroutine(FadeRoutine(target, time));
      return _curActiveFade;
    }
    IEnumerator FadeRoutine(float target, float time)
    {
      while (!Mathf.Approximately(_cg.alpha, target))
      {
        CG.alpha = Mathf.MoveTowards(_cg.alpha, target, Time.deltaTime / time);
        yield return null;
      }
    }
  }

}