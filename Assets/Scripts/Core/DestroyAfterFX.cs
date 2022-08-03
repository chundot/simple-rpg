using UnityEngine;

namespace RPG.Core
{
  public class DestroyAfterFX : MonoBehaviour
  {
    [SerializeField] GameObject _targetToDestroy;
    void Update()
    {
      if (!GetComponent<ParticleSystem>().IsAlive())
        Destroy(_targetToDestroy != null ? _targetToDestroy : gameObject);
    }
  }
}