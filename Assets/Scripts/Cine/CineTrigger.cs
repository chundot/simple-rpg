using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cine
{
  public class CineTrigger : MonoBehaviour
  {
    [SerializeField] bool _onlyOnce = true;
    bool _triggered = false;
    private void OnTriggerEnter(Collider other)
    {
      if (_onlyOnce && _triggered) return;
      if (other.CompareTag("Player"))
      {
        GetComponent<PlayableDirector>().Play();
        _triggered = true;
      }
    }
  }

}